
using RimWorld;
using System;
namespace VanillaCookingExpandedVariety
{
    public class BookOutcomeProperties_JoyFactorModifierGetExtraFavourite : BookOutcomeProperties
    {
        public int ticksToGetFavouriteIngredient;

        public override Type DoerClass => typeof(ReadingOutcomeDoerJoyFactorModifierGetExtraFavourite);
    }
}