using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace VanillaCookingExpandedVariety
{

    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.VanillaCookingExpandedVariety");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


    }

}
