
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
            return true;
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
                if (GameComponent_FoodVariety.pawns_and_favourites[reader]?.Count < 2)
                {
                    GameComponent_FoodVariety.pawns_and_favourites[reader].Add(Book.ingredient);
                    Messages.Message("VCEV_NewFavouriteGained".Translate(reader.NameShortColored, GameComponent_FoodVariety.pawns_and_favourites[reader][1].LabelCap), MessageTypeDefOf.PositiveEvent, true);
                    this.Book.Destroy();
                }
                else if (GameComponent_FoodVariety.pawns_and_favourites[reader]?.Contains(Book.ingredient) == false)
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

        public override List<RulePack> GetTopicRulePacks()
        {
            rulepack = new RulePack();
            rulepack.rulesStrings = new List<string> { "subject->" + Book.ingredient.LabelCap };
            return new List<RulePack> { rulepack };
        }

        public override void PostExposeData()
        {

            base.PostExposeData();
            Scribe_Values.Look(ref tickCounter, "cookingBookTickCounter");
         

        }
    }
}
