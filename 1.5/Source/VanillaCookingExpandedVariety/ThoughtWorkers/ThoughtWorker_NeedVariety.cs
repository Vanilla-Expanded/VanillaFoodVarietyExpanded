using System;
using Verse;
using RimWorld;


namespace VanillaCookingExpandedVariety
{
    public class ThoughtWorker_NeedVariety : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.needs?.TryGetNeed<Need_FoodVariety>() == null)
            {
                return ThoughtState.Inactive;
            }

            Need_FoodVariety need = p.needs.TryGetNeed<Need_FoodVariety>();

            if (need.CurLevel <= VanillaCookingExpandedVariety_Settings.lowVarietyThreshold)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            else if (need.CurLevel >= VanillaCookingExpandedVariety_Settings.highVarietyThreshold) {
                return ThoughtState.ActiveAtStage(1);
            }

              
            return ThoughtState.Inactive;

              
            
        }

        public override float MoodMultiplier(Pawn p)
        {
            return VanillaCookingExpandedVariety_Settings.moodMultiplier;
        }
    }
}
