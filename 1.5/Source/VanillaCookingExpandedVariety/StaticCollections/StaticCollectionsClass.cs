
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace VanillaCookingExpandedVariety
{
    [StaticConstructorOnStartup]
    public static class StaticCollectionsClass
    {


        public static List<ThingDef> blacklistedIngredients = new List<ThingDef>();
        public static List<ThingDef> blacklistedFoods = new List<ThingDef>();


        static StaticCollectionsClass()
        {

            blacklistedIngredients.Clear();
            List<BlacklistedIngredientDefs> allBlacklistedIngredients = DefDatabase<BlacklistedIngredientDefs>.AllDefsListForReading;
            foreach (BlacklistedIngredientDefs individualList in allBlacklistedIngredients)
            {
                blacklistedIngredients.AddRange(individualList.blacklistedIngredients);
            }

            blacklistedFoods.Clear();
            List<BlacklistedFoodDefs> allBlacklistedFoods = DefDatabase<BlacklistedFoodDefs>.AllDefsListForReading;
            foreach (BlacklistedFoodDefs individualList in allBlacklistedFoods)
            {
                blacklistedFoods.AddRange(individualList.blacklistedFoods);
            }

        }





    }
}
