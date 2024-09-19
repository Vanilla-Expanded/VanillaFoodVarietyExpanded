using HarmonyLib;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaCookingExpandedVariety;

public class GameComponent_FoodVariety : GameComponent
{

    public static List<Pawn> colonists_with_foodvariety_need = new List<Pawn>();
    public static Dictionary<Pawn, PawnDiet> pawns_and_diet = new Dictionary<Pawn, PawnDiet>();
    public static Dictionary<Pawn, List<ThingDef>> pawns_and_favourites = new Dictionary<Pawn, List<ThingDef>>();

    private List<Pawn> keysList;
    private List<PawnDiet> valuesList;

    private List<Pawn> keysListFavourites;
    private List<List<ThingDef>> valuesListFavourites;

    public static GameComponent_FoodVariety Instance;

    public GameComponent_FoodVariety(Game game) => Instance = this;



    public static void RefreshList(Pawn pawnJustAdded)
    {
       
        List<Pawn> pawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists_NoSlaves;
        if (pawns.Count > 0)
        {
            if (pawnJustAdded != null)
            {
                pawns.Add(pawnJustAdded);
            }
            GeneDef gene = DefDatabase<GeneDef>.GetNamedSilentFail("VREA_Power");
            if (gene == null)
            {
                colonists_with_foodvariety_need = pawns;
            }
            else
            {
                foreach (Pawn pawn in pawns)
                {
                    if (pawn?.genes?.HasActiveGene(gene) == false)
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
                if (!pawns_and_diet.ContainsKey(pawn))
                {
                    pawns_and_diet.Add(pawn, new PawnDiet());
                    pawns_and_diet[pawn].last10Meals.AddRange(Util.GenerateNDistinctFoods(VanillaCookingExpandedVariety_Settings.numberOfMeals / 2, pawn));
                    pawns_and_diet[pawn].last10Ingredients.AddRange(Util.GenerateNDistinctIngredients(VanillaCookingExpandedVariety_Settings.numberOfIngredients / 2, pawn));
                }
                else
                {
                    if (pawns_and_diet[pawn].last10Meals?.Count > VanillaCookingExpandedVariety_Settings.numberOfMeals)
                    {
                        pawns_and_diet[pawn].last10Meals = Util.TakeLast(pawns_and_diet[pawn].last10Meals, VanillaCookingExpandedVariety_Settings.numberOfMeals).ToList();
                    }
                    if (pawns_and_diet[pawn].last10Ingredients?.Count > VanillaCookingExpandedVariety_Settings.numberOfIngredients)
                    {
                        pawns_and_diet[pawn].last10Ingredients = Util.TakeLast(pawns_and_diet[pawn].last10Ingredients, VanillaCookingExpandedVariety_Settings.numberOfIngredients).ToList();
                    }
                    pawns_and_diet[pawn].last10Meals.RemoveWhere(x => x is null);
                    pawns_and_diet[pawn].last10Ingredients.RemoveWhere(x => x is null);
                }

                if (!pawns_and_favourites.ContainsKey(pawn))
                {
                    pawns_and_favourites[pawn] = new List<ThingDef> { Util.GenerateFavouriteFood(pawn) };
                }
                else
                {
                    pawns_and_favourites[pawn].RemoveWhere(x => x is null);
                    if (pawns_and_favourites[pawn].Count() == 0) { pawns_and_favourites[pawn] = new List<ThingDef> { Util.GenerateFavouriteFood(pawn) }; }
                }

            }

        }
        

        

    }

    public static void AddEatenThingsToList(Pawn p, Thing thing)
    {

        pawns_and_diet[p].last10Meals?.Add(thing.def);

        if (thing.TryGetComp<CompIngredients>() != null)
        {
            foreach (ThingDef ingredient in thing.TryGetComp<CompIngredients>().ingredients)
            {
                pawns_and_diet[p].last10Ingredients.Add(ingredient);


            }
        }

        if (pawns_and_diet[p].last10Meals?.Count > VanillaCookingExpandedVariety_Settings.numberOfMeals)
        {
            pawns_and_diet[p].last10Meals = Util.TakeLast(pawns_and_diet[p].last10Meals, VanillaCookingExpandedVariety_Settings.numberOfMeals).ToList();
        }
        if (pawns_and_diet[p].last10Ingredients?.Count > VanillaCookingExpandedVariety_Settings.numberOfIngredients)
        {
            pawns_and_diet[p].last10Ingredients = Util.TakeLast(pawns_and_diet[p].last10Ingredients, VanillaCookingExpandedVariety_Settings.numberOfIngredients).ToList();
        }
    }

    public override void FinalizeInit()
    {

        RefreshList(null);


    }

    public override void ExposeData()
    {

       
            Scribe_Collections.Look(ref pawns_and_diet, "pawns_and_diet", LookMode.Reference, LookMode.Deep, ref keysList, ref valuesList);
            Scribe_Collections.Look(ref pawns_and_favourites, "pawns_and_favourites", LookMode.Reference, LookMode.Def, ref keysListFavourites, ref valuesListFavourites);
       
    }

    public static float MealVariety(Pawn p)
    {
        int bonus = 0;
        foreach (ThingDef thingDef in pawns_and_favourites[p])
        {
            if (pawns_and_diet[p].last10Meals.Contains(thingDef))
            {
                bonus += 2;
            }
        }
        return (pawns_and_diet[p].last10Meals.Distinct().Count() + bonus)  ;
    }

    public static float IngredientVariety(Pawn p)
    {
        int bonus = 0;
        foreach (ThingDef thingDef in pawns_and_favourites[p])
        {
            if (pawns_and_diet[p].last10Ingredients.Contains(thingDef))
            {
                bonus += 2;
            }
        }

        return (pawns_and_diet[p].last10Ingredients.Distinct().Count() + bonus) ;
    }

    public static float Variety(Pawn p)
    {



        return (
            (MealVariety(p) * (10f / VanillaCookingExpandedVariety_Settings.numberOfMeals))
            + (IngredientVariety(p) * (10f / VanillaCookingExpandedVariety_Settings.numberOfIngredients))
            )/20;

    }

}