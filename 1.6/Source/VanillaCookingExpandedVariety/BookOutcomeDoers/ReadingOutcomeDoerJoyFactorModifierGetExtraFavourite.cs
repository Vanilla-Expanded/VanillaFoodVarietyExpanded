
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;
namespace VanillaCookingExpandedVariety
{
    public class ReadingOutcomeDoerJoyFactorModifierGetExtraFavourite : BookOutcomeDoer
    {
        public int tickCounter = 0;
        RulePack rulepack;

        public new BookOutcomeProperties_JoyFactorModifierGetExtraFavourite Props => (BookOutcomeProperties_JoyFactorModifierGetExtraFavourite)props;

        public new Book_Ingredient Book => (Book_Ingredient)base.Parent;

        public override bool DoesProvidesOutcome(Pawn reader)
        {
            return false;
        }

        public override void OnBookGenerated(Pawn author = null)
        {
            base.Book.SetJoyFactor(BookUtility.GetNovelJoyFactorForQuality(base.Quality));
            Book.ingredient = Util.GenerateFavouriteFood(null);
            
            
        }

        public override string GetBenefitsString(Pawn reader = null)
        {
            return string.Format(" - {0}: x{1}", "BookJoyFactor".Translate(), base.Book.JoyFactor.ToStringPercent());
        }

        public override void OnReadingTick(Pawn reader, float factor)
        {
            base.OnReadingTick(reader, factor);
            tickCounter++;
            if(tickCounter > Props.ticksToGetFavouriteIngredient)
            {
                var diet = GameComponent_FoodVariety.pawns_and_diet[reader];
                if (diet.favourites?.Count < 2)
                {
                    diet.favourites.Add(Book.ingredient);
                    Messages.Message("VCEV_NewFavouriteGained".Translate(reader.NameShortColored, diet.favourites[1].LabelCap), MessageTypeDefOf.PositiveEvent, true);
                    this.Book.Destroy();
                }
                else if (diet.favourites?.Contains(Book.ingredient) == true)
                {
                    Messages.Message("VCEV_AlreadyFavourite".Translate(reader.NameShortColored, Book.ingredient.LabelCap), MessageTypeDefOf.RejectInput, true);
                    tickCounter = 0;
                }
                else{
                    Messages.Message("VCEV_NoMoreFavourites".Translate(reader.NameShortColored), MessageTypeDefOf.RejectInput, true);
                    tickCounter = 0;
                }
            }
        }

        public override IEnumerable<RulePack> GetTopicRulePacks()
        {
            rulepack = new RulePack();
            ReflectionCache.rulesStrings(rulepack) = new List<string> { "subject->" + Book.ingredient.LabelCap };
          
            return new List<RulePack> { rulepack };
        }

        public override void PostExposeData()
        {

            base.PostExposeData();
            Scribe_Values.Look(ref tickCounter, "cookingBookTickCounter");
         

        }
    }
}
