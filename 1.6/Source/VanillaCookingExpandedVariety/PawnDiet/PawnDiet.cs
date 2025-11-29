using System.Collections.Generic;
using Verse;

namespace VanillaCookingExpandedVariety
{
    public class PawnDiet: IExposable
    {

        public Pawn pawn;
        private Corpse tempCorpse;

        public List<ThingDef> last10Meals;
        public List<ThingDef> last10Ingredients;
        public List<ThingDef> favourites;

        public PawnDiet()
        {
            // Constructor needed for ExposeData
        }

        public PawnDiet(Pawn pawn) : this()
        {
            this.pawn = pawn;

            last10Meals = new List<ThingDef>();
            last10Ingredients = new List<ThingDef>();
            favourites = new List<ThingDef>();
        }

        public PawnDiet(Pawn pawn, PawnDiet original) : this()
        {
            this.pawn = pawn;

            last10Meals = [..original.last10Meals];
            last10Ingredients = [..original.last10Ingredients];
            favourites = [..original.favourites];
        }


        public void ExposeData()
        {
            Scribe_Collections.Look(ref last10Meals, "last10Meals", LookMode.Def);
            Scribe_Collections.Look(ref last10Ingredients, "last10Ingredients", LookMode.Def);
            Scribe_Collections.Look(ref favourites, "favourites", LookMode.Def);

            const string pawnLabel = "pawn";
            const string corpseLabel = "corpse";

            if (Scribe.mode == LoadSaveMode.Saving)
            {
                // When saving and the pawn is dead, only save the corpse (if it's present)
                if (pawn.Dead)
                {
                    tempCorpse = pawn.Corpse;
                    if (tempCorpse is { Bugged: false })
                        Scribe_References.Look(ref tempCorpse, corpseLabel);
                    tempCorpse = null;
                }
                // When saving and the pawn is alive, save the pawn themselves
                else
                    Scribe_References.Look(ref pawn, pawnLabel);
            }
            else
            {
                // When exposing data, try loading both the pawn and the corpse
                Scribe_References.Look(ref pawn, pawnLabel);
                Scribe_References.Look(ref tempCorpse, corpseLabel);

                if (Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    last10Meals ??= [];
                    last10Ingredients ??= [];
                    favourites ??= [];

                    // In PostLoadInit, grab the pawn from the corpse and set the corpse to null
                    if (tempCorpse != null)
                    {
                        pawn = tempCorpse.InnerPawn;
                        tempCorpse = null;
                    }
                }
            }
        }


    }
}
