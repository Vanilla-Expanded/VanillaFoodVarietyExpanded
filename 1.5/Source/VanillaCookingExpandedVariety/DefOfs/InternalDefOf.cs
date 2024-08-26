using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaCookingExpandedVariety
{

    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static TraitDef Cannibal;

        [MayRequire("VanillaExpanded.VBrewE")]
        public static ChemicalDef VBE_Caffeine;
    }
}