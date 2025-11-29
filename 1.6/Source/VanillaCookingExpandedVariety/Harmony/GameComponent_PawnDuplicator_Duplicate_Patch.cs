using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaCookingExpandedVariety;

[HarmonyPatch(typeof(GameComponent_PawnDuplicator), nameof(GameComponent_PawnDuplicator.Duplicate))]
public static class GameComponent_PawnDuplicator_Duplicate_Patch
{
    private static void Postfix(Pawn pawn, Pawn __result)
    {
        if (GameComponent_FoodVariety.pawns_and_diet.TryGetValue(pawn, out var diet))
            GameComponent_FoodVariety.pawns_and_diet[__result] = new PawnDiet(__result, diet);
    }
}