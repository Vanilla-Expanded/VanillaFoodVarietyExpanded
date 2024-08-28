using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaCookingExpandedVariety
{
    public static class Util
    {

        public static ThingDef GenerateFavouriteFood(Pawn p)
        {

            ThingDef favouriteFood = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => FoodValidator(x, p)).RandomElement();
        
            return favouriteFood;

        }

        public static bool FoodValidator(ThingDef x, Pawn p)
        {          
            return 
                
            x.ingestible != null 
            && !StaticCollectionsClass.blacklistedIngredients.Contains(x)
            && (!x.IsDrug || (x.IsDrug && (x.GetCompProperties<CompProperties_Drug>()?.chemical == ChemicalDefOf.Alcohol|| x.GetCompProperties<CompProperties_Drug>()?.chemical == InternalDefOf.VBE_Caffeine)))
            && (x.ingestible?.HumanEdible == true || x.ingredient?.mergeCompatibilityTags?.Contains("Condiments")==true)
            && !x.IsCorpse
            && x!=ThingDefOf.Penoxycyline
            && x.tradeTags?.Contains("Serum")!=true
            && !x.defName.Contains("VCE_Ruined")
            && x.thingCategories?.Contains(ThingCategoryDefOf.EggsFertilized)!=true
            && (x != ThingDefOf.HemogenPack || (x == ThingDefOf.HemogenPack && p?.genes?.HasActiveGene(GeneDefOf.Hemogenic)==true))
            && (x != ThingDefOf.Meat_Human 
                || 
                  (  
                        (x == ThingDefOf.Meat_Human) 
                        && (p?.story?.traits?.HasTrait(InternalDefOf.Cannibal) == true)                      
                        && (Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo?.PreceptsListForReading?.ContainsAny(y => y.def == PreceptDefOf.Cannibalism_Preferred==true || y.def == PreceptDefOf.Cannibalism_RequiredRavenous == true
                        || y.def == PreceptDefOf.Cannibalism_RequiredStrong == true) ==true)
                   )
               )
           
            && (
                    (Find.IdeoManager.classicMode)
                    ||
                    (Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo?.HasVegetarianRequiredPrecept() != true && Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo?.HasMeatEatingRequiredPrecept() != true) 
                    ||
                    (Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo?.HasVegetarianRequiredPrecept() == true && x != ThingDefOf.Pemmican && (x.ingestible?.foodType.HasFlag(FoodTypeFlags.VegetableOrFruit)==true|| (x.ingestible?.foodType == FoodTypeFlags.Meal&& !x.defName.Contains("_Meat")&&!x.defName.Contains("Grill")))) 
                    ||
                    (Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo?.HasMeatEatingRequiredPrecept() == true && (x.ingestible?.foodType.HasFlag(FoodTypeFlags.Meat) == true || x.ingestible?.foodType.HasFlag(FoodTypeFlags.AnimalProduct) == true || (x.ingestible?.foodType == FoodTypeFlags.Meal && !x.defName.Contains("_Veg")))    )

               );
          
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static List<TaggedString> ThingDefsToLabels(IEnumerable<ThingDef> source)
        {
            List<TaggedString> returnList = new List<TaggedString>();

            foreach (ThingDef thingDef in source)
            {
                if (thingDef != null)
                {
                    returnList.Add(thingDef.LabelCap);
                }

            }
            return returnList;

        }

        public static List<ThingDef> GenerateNDistinctFoods(int number,Pawn p )
        {
            return DefDatabase<ThingDef>.AllDefsListForReading.Where(x => FoodValidator(x,p) && (x.ingestible?.foodType == FoodTypeFlags.Meal|| x.ingestible?.foodType == FoodTypeFlags.Processed
            || x.ingestible?.foodType == FoodTypeFlags.Liquor)  ).InRandomOrder().Take(number).ToList();
        }

        public static List<ThingDef> GenerateNDistinctIngredients(int number, Pawn p)
        {
            return DefDatabase<ThingDef>.AllDefsListForReading.Where(x => FoodValidator(x,p) && x.ingestible?.foodType != FoodTypeFlags.Meal).InRandomOrder().Take(number).ToList();
        }


    }




}
