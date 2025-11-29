using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaCookingExpandedVariety;

public class GameComponent_FoodVariety : GameComponent
{

    public static List<Pawn> colonists_with_foodvariety_need = new List<Pawn>();
    public static Dictionary<Pawn, PawnDiet> pawns_and_diet = new Dictionary<Pawn, PawnDiet>();

    private List<PawnDiet> dietList;

    public static GameComponent_FoodVariety Instance;

    // Fields kept for backwards compatibility (all start with "old"). Safe to remove once we're no longer supporting the old version of the mod.
    public static Dictionary<Pawn, PawnDiet> old_pawns_and_diet = new Dictionary<Pawn, PawnDiet>();
    public static Dictionary<Pawn, List<ThingDef>> old_pawns_and_favourites = new Dictionary<Pawn, List<ThingDef>>();

    private List<Pawn> oldKeysList;
    private List<PawnDiet> oldValuesList;

    private List<Pawn> oldKeysListFavourites;
    private List<List<ThingDef>> oldValuesListFavourites;

    public GameComponent_FoodVariety(Game game) => Instance = this;



    public static void RefreshList(Pawn pawnJustAdded)
    {
       
        List<Pawn> pawns = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves;
        if (pawns.Count > 0 || pawnJustAdded != null)
        {
            if (pawnJustAdded != null && !pawns.Contains(pawnJustAdded))
            {
                pawns.Add(pawnJustAdded);
            }
            colonists_with_foodvariety_need.Clear();
            if (InternalDefOf.VREA_Power == null)
            {
                colonists_with_foodvariety_need.AddRange(pawns);
            }
            else
            {
                foreach (Pawn pawn in pawns)
                {
                    if (pawn?.genes?.HasActiveGene(InternalDefOf.VREA_Power) == false)
                    {
                        if (!colonists_with_foodvariety_need.Contains(pawn))
                        {
                            colonists_with_foodvariety_need.Add(pawn);
                        }                       
                    }
                }
            }
            foreach (Pawn pawn in pawns)
            {
                if (!pawns_and_diet.TryGetValue(pawn, out var diet))
                {
                    diet = new PawnDiet(pawn);
                    pawns_and_diet.Add(pawn, diet);
                    diet.last10Meals.AddRange(Util.GenerateNDistinctFoods(VanillaCookingExpandedVariety_Settings.numberOfMeals / 2, pawn));
                    diet.last10Ingredients.AddRange(Util.GenerateNDistinctIngredients(VanillaCookingExpandedVariety_Settings.numberOfIngredients / 2, pawn));
                    diet.favourites.Add(Util.GenerateFavouriteFood(pawn));
                }
                else
                {
                    diet.last10Meals ??= new List<ThingDef>();
                    diet.last10Ingredients ??= new List<ThingDef>();
                    diet.favourites ??= new List<ThingDef>();

                    if (diet.last10Meals.Count > VanillaCookingExpandedVariety_Settings.numberOfMeals)
                    {
                        diet.last10Meals.RemoveKeepLast(VanillaCookingExpandedVariety_Settings.numberOfMeals);
                    }
                    if (diet.last10Ingredients.Count > VanillaCookingExpandedVariety_Settings.numberOfIngredients)
                    {
                        diet.last10Ingredients.RemoveKeepLast(VanillaCookingExpandedVariety_Settings.numberOfIngredients);
                    }
                    diet.last10Meals.RemoveWhere(x => x is null);
                    diet.last10Ingredients.RemoveWhere(x => x is null);
                    diet.favourites.RemoveWhere(x => x is null);
                    if (!diet.favourites.Any())
                    {
                        diet.favourites.Add(Util.GenerateFavouriteFood(pawn));
                    }
                }

            }

        }




    }

    public static void AddEatenThingsToList(Pawn p, Thing thing)
    {
        var diet = pawns_and_diet[p];
        diet.last10Meals.Add(thing.def);

        if (thing.TryGetComp<CompIngredients>() != null)
        {
            if(thing.def != ThingDefOf.MealNutrientPaste)
            {
                foreach (ThingDef ingredient in thing.TryGetComp<CompIngredients>().ingredients)
                {
                    diet.last10Ingredients.Add(ingredient);
                }
            }else
            {
                diet.last10Ingredients.Add(ThingDefOf.MealNutrientPaste);
            }
            
        }

        if (diet.last10Meals.Count > VanillaCookingExpandedVariety_Settings.numberOfMeals)
        {
            diet.last10Meals.RemoveKeepLast(VanillaCookingExpandedVariety_Settings.numberOfMeals);
        }
        if (diet.last10Ingredients.Count > VanillaCookingExpandedVariety_Settings.numberOfIngredients)
        {
            diet.last10Ingredients.RemoveKeepLast(VanillaCookingExpandedVariety_Settings.numberOfIngredients);
        }
    }

    public override void FinalizeInit()
    {

        RefreshList(null);


    }

    public override void ExposeData()
    {
        // When saving, don't expose as a dictionary but just a list of diets
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            // First, lets cleanup the data.
            // Clear all discarded pawns, as well as dead pawns with no corpse, bugged corpse, or destroyed corpse.
            pawns_and_diet.RemoveAll(kvp => kvp.Key.Discarded || kvp.Key.Dead && (kvp.Key.Corpse == null || kvp.Key.Corpse.Bugged || kvp.Key.Corpse.Destroyed));
            
            dietList = pawns_and_diet.Values.ToList();
        }
        Scribe_Collections.Look(ref dietList, "pawns_diet_list", LookMode.Deep);
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            dietList = null;
        }
        else if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            pawns_and_diet ??= new Dictionary<Pawn, PawnDiet>();

            // Populate the dictionary using our list
            if (dietList != null)
            {
                foreach (var pawnDiet in dietList)
                {
                    if (pawnDiet.pawn != null)
                        pawns_and_diet[pawnDiet.pawn] = pawnDiet;
                }

                dietList = null;
            }
        }

        // When loading, load the old values for backwards compatibility.
        // The entire branch should be safe to remove once we want to drop the backwards compatibility.
        if (Scribe.mode != LoadSaveMode.Saving)
        {
            Scribe_Collections.Look(ref old_pawns_and_diet, "pawns_and_diet", LookMode.Reference, LookMode.Deep, ref oldKeysList, ref oldValuesList, false);
            Scribe_Collections.Look(ref old_pawns_and_favourites, "pawns_and_favourites", LookMode.Reference, LookMode.Def, ref oldKeysListFavourites, ref oldValuesListFavourites, false);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                // If old_pawns_and_diet isn't null, populate the new list using them
                if (old_pawns_and_diet != null)
                {
                    foreach (var oldDiet in old_pawns_and_diet)
                    {
                        var (pawn, diet) = oldDiet;
                        if (pawn != null && diet != null)
                        {
                            diet.pawn = pawn;
                            // Grab favourites from the other dictionary first, if any, and remove it from the dictionary
                            if (old_pawns_and_favourites != null && old_pawns_and_favourites.TryGetValue(pawn, out diet.favourites))
                                old_pawns_and_favourites.Remove(pawn);
                            // If there's no favourites, generate one
                            else
                                diet.favourites = new List<ThingDef>() { Util.GenerateFavouriteFood(pawn) };

                            pawns_and_diet[pawn] = diet;
                        }
                    }

                    old_pawns_and_diet = null;
                }

                if (old_pawns_and_favourites != null)
                {
                    // If old_pawns_and_favourites still has some values left (we removed any we used), use those to populate the new list
                    foreach (var oldFavourites in old_pawns_and_favourites)
                    {
                        var (pawn, favourites) = oldFavourites;
                        if (pawn != null && favourites != null)
                        {
                            var diet = new PawnDiet(pawn)
                            {
                                favourites = favourites
                            };
                            diet.last10Meals.AddRange(Util.GenerateNDistinctFoods(VanillaCookingExpandedVariety_Settings.numberOfMeals / 2, pawn));
                            diet.last10Ingredients.AddRange(Util.GenerateNDistinctIngredients(VanillaCookingExpandedVariety_Settings.numberOfIngredients / 2, pawn));
                        }
                    }

                    old_pawns_and_favourites = null;
                }
            }
        }
    }

    public static float MealVariety(Pawn p)
    {
        var diet = pawns_and_diet[p];
        int bonus = 0;
        foreach (ThingDef thingDef in diet.favourites)
        {
            if (diet.last10Meals.Contains(thingDef))
            {
                bonus += 2;
            }
        }
        return (diet.last10Meals.Distinct().Count() + bonus)  ;
    }

    public static float IngredientVariety(Pawn p)
    {
        var diet = pawns_and_diet[p];
        int bonus = 0;
        foreach (ThingDef thingDef in diet.favourites)
        {
            if (diet.last10Ingredients.Contains(thingDef))
            {
                bonus += 2;
            }
        }

        return (diet.last10Ingredients.Distinct().Count() + bonus) ;
    }

    public static float Variety(Pawn p)
    {



        return (
            (MealVariety(p) * (10f / VanillaCookingExpandedVariety_Settings.numberOfMeals))
            + (IngredientVariety(p) * (10f / VanillaCookingExpandedVariety_Settings.numberOfIngredients))
            )/20;

    }

}