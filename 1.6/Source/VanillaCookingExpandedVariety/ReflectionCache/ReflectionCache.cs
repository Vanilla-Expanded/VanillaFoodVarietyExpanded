using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using Verse.Grammar;



namespace VanillaCookingExpandedVariety
{
    public class ReflectionCache
    {
        public static readonly AccessTools.FieldRef<RulePack, List<string>> rulesStrings =
           AccessTools.FieldRefAccess<RulePack, List<string>>(AccessTools.Field(typeof(RulePack), "rulesStrings"));
    }
}
