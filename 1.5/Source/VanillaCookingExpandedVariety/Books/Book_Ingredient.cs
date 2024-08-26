
using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
namespace VanillaCookingExpandedVariety
{
    public class Book_Ingredient : Book
    {
        public ThingDef ingredient;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref ingredient, "ingredient");

        }


    }
}