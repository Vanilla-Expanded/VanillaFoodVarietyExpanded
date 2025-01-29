using HarmonyLib;
using Mono.Cecil.Cil;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
using Verse.AI;
using static HarmonyLib.Code;

namespace VanillaCookingExpandedVariety
{
      [HarmonyPatch(typeof(CompIngredients), "MergeIngredients")]
      public static class VanillaCookingExpandedVariety_CompIngredients_MergeIngredients_Patch
    {
          public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
          {
              var codes = codeInstructions.ToList();
            var modoptions = AccessTools.Method(typeof(VanillaCookingExpandedVariety_CompIngredients_MergeIngredients_Patch), "GetNumber");

            for (var i = 0; i < codes.Count; i++)
              {
               

                  if (codes[i].opcode == OpCodes.Ldc_I4_3)
                  {

                      yield return new CodeInstruction(OpCodes.Call, modoptions);

                  }else yield return codes[i];
              }
          }



        public static int GetNumber()
        {
            return VanillaCookingExpandedVariety_Settings.numberOfIngredientStacked;
        }

    }

    
    
}
