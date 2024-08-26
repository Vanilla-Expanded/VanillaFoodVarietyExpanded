using HarmonyLib;
using RimWorld;
using Verse;


namespace VanillaCookingExpandedVariety
{

    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("Ingested")]
    public static class VanillaCookingExpandedVariety_Thing_Ingested_Patch
    {


        [HarmonyPostfix]
        public static void CommunicateIngestion(Thing __instance, Pawn ingester)
        {
            if (GameComponent_FoodVariety.colonists_with_foodvariety_need.Contains(ingester))
            {
                GameComponent_FoodVariety.AddEatenThingsToList(ingester, __instance);

            }

        }


    }













}

