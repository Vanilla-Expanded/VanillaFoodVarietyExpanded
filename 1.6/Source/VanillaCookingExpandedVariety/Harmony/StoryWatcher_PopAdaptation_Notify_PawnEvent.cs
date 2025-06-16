using HarmonyLib;
using RimWorld;
using Verse;


namespace VanillaCookingExpandedVariety
{

    [HarmonyPatch(typeof(StoryWatcher_PopAdaptation))]
    [HarmonyPatch("Notify_PawnEvent")]
    public static class VanillaCookingExpandedVariety_StoryWatcher_PopAdaptation_Notify_PawnEvent_Patch
    {


        [HarmonyPostfix]
        public static void RefreshNeedList(Pawn p, PopAdaptationEvent ev)
        {
            if (ev == PopAdaptationEvent.GainedColonist)
            {
                GameComponent_FoodVariety.RefreshList(p);
            }


        }


    }













}

