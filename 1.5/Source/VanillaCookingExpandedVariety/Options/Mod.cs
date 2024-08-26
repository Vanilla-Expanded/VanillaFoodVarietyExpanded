using RimWorld;
using UnityEngine;
using Verse;


namespace VanillaCookingExpandedVariety
{



    public class VanillaCookingExpandedVariety_Mod : Mod
    {


        public VanillaCookingExpandedVariety_Mod(ModContentPack content) : base(content)
        {
            GetSettings<VanillaCookingExpandedVariety_Settings>();
        }

        public override string SettingsCategory()
        {
            return "VCE - Food Variety";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            VanillaCookingExpandedVariety_Settings.DoWindowContents(inRect);
        }
    }


}
