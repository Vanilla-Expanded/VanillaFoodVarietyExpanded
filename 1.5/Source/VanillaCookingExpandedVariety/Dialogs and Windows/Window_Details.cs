using System;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;



namespace VanillaCookingExpandedVariety
{
    public class Window_Details : Window
    {
        public Pawn pawn;
       

        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);


        public Window_Details(Pawn pawn)
        {
            this.pawn = pawn;          
            draggable = true;
            closeOnClickedOutside = true;
            preventCameraMotion = false;

        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            var arrowRect = new Rect(0f, 0f, 32f, 32f);
            GUI.DrawTexture(arrowRect, ContentFinder<Texture2D>.Get("UI/VCEV_GoBack", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
            if (Widgets.ButtonInvisible(arrowRect))
            {
                Close();
            }

            var GoBackTextRect = new Rect(40, 5, 100f, 32f);
            Widgets.Label(GoBackTextRect, "VCEV_GoBack".Translate());
            if (Widgets.ButtonInvisible(GoBackTextRect))
            {
                Close();
            }

            if (Widgets.ButtonImage(new Rect(inRect.xMax-20, 0, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }


            outRect.yMin += 20f;

            var pawnImageRect = new Rect(0, outRect.yMin, 128f, 128f);
            Widgets.DrawBoxSolidWithOutline(pawnImageRect, fillColor, borderColor, 2);
            Rect pawnImageRectInside = pawnImageRect.ContractedBy(2);
            GUI.DrawTexture(pawnImageRectInside, PortraitsCache.Get(pawn,new Vector2(126f,126f), Rot4.South, Vector3.zero, 1.28205f), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);

            Rect pawnFavouriteFoodRect = new Rect(160, outRect.yMin, 500f, 30f);
            Widgets.Label(pawnFavouriteFoodRect, "VCE_FavouriteFoodTextOnly".Translate());
            Color color = Color.white;

            for (int i = 0; i < GameComponent_FoodVariety.pawns_and_favourites[pawn].Count; i++)
            {
                Rect pawnFavouriteFoodImage = new Rect((i*(64 +10)) + 160 , outRect.yMin + 30, 64, 64);
                
                GUI.DrawTexture(pawnFavouriteFoodImage, Util.GetTextureOrAnimalIfMeat(GameComponent_FoodVariety.pawns_and_favourites[pawn][i], out color), ScaleMode.ScaleToFit, alphaBlend: true, 0f, color, 0f, 0f);
                TooltipHandler.TipRegion(pawnFavouriteFoodImage, GameComponent_FoodVariety.pawns_and_favourites[pawn][i].LabelCap + ": " + GameComponent_FoodVariety.pawns_and_favourites[pawn][i].description + "\n\n" + "VCE_FavouriteFoodEffects".Translate());

            }

            Rect pawnDietDetails = new Rect(0, outRect.yMin +160, outRect.xMax, 30f);
            Widgets.Label(pawnDietDetails, "VCE_LastFoodsTextOnly".Translate());
            for(int i=0;i< VanillaCookingExpandedVariety_Settings.numberOfMeals; i++)
       
            {
                Rect pawnDietDetailsN = new Rect((SizeOfTiles(44, VanillaCookingExpandedVariety_Settings.numberOfMeals,54) + SizeOfTiles(10, VanillaCookingExpandedVariety_Settings.numberOfMeals, 15)) * i, outRect.yMin + 180, 
                SizeOfTiles(44, VanillaCookingExpandedVariety_Settings.numberOfMeals, 54), SizeOfTiles(44, VanillaCookingExpandedVariety_Settings.numberOfMeals, 54));
                Widgets.DrawBoxSolidWithOutline(pawnDietDetailsN, fillColor, borderColor, 1);
                Rect pawnDietDetailsNInside = pawnDietDetailsN.ContractedBy(2);
                if(i<GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals.Count())
                {
                    GUI.DrawTexture(pawnDietDetailsNInside, Util.GetTextureOrAnimalIfMeat(GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i], out color), ScaleMode.ScaleToFit, alphaBlend: true, 0f, color, 0f, 0f);
                    bool found = false;
                    foreach (ThingDef thingDef in GameComponent_FoodVariety.pawns_and_favourites[pawn])
                    {
                        if (GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i]==thingDef)
                        {
                            found = true;
                            Rect pawnDietDetailsNInsideHeart = new Rect(pawnDietDetailsNInside.xMax - 16, pawnDietDetailsNInside.y, 16, 16);
                            GUI.DrawTexture(pawnDietDetailsNInsideHeart, ContentFinder<Texture2D>.Get("UI/VCEV_HeartIcon", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
                        }
                    }
                    if (found)
                    {
                        TooltipHandler.TipRegion(pawnDietDetailsNInside, GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i].LabelCap + ": " + GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i].description + "\n\n" + "VCEV_ThisIsFavourite".Translate(pawn.Named("PAWN")));
                    }
                    else
                    {
                        TooltipHandler.TipRegion(pawnDietDetailsNInside, GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i].LabelCap + ": " + GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals[i].description);
                    }

                }
                else
                {
                    TooltipHandler.TipRegion(pawnDietDetailsNInside, "VCE_NotEatenYet".Translate());

                }
               


            }

            Rect pawnDietDetailsIngredients = new Rect(0, outRect.yMin + 250, outRect.xMax, 30f);
            Widgets.Label(pawnDietDetailsIngredients, "VCE_LastIngredientsTextOnly".Translate());
            for (int i = 0; i < VanillaCookingExpandedVariety_Settings.numberOfIngredients; i++)

            {
                Rect pawnDietDetailsIngredientsN = new Rect((SizeOfTiles(44,VanillaCookingExpandedVariety_Settings.numberOfIngredients, 54) + SizeOfTiles(10, VanillaCookingExpandedVariety_Settings.numberOfMeals, 15)) *i, outRect.yMin + 270,
                SizeOfTiles(44, VanillaCookingExpandedVariety_Settings.numberOfIngredients, 54), SizeOfTiles(44, VanillaCookingExpandedVariety_Settings.numberOfIngredients, 54));
                Widgets.DrawBoxSolidWithOutline(pawnDietDetailsIngredientsN, fillColor, borderColor, 1);
                Rect pawnDietDetailsIngredientsNInside = pawnDietDetailsIngredientsN.ContractedBy(2);
                if (i < GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients.Count())
                {
                    GUI.DrawTexture(pawnDietDetailsIngredientsNInside, Util.GetTextureOrAnimalIfMeat(GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i], out color), ScaleMode.ScaleToFit, alphaBlend: true, 0f, color, 0f, 0f);
                    bool found = false;
                    foreach (ThingDef thingDef in GameComponent_FoodVariety.pawns_and_favourites[pawn])
                    {
                        if (GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i]==thingDef)
                        {
                            found = true;
                            Rect pawnDietDetailsIngredientsNInsideHeart = new Rect(pawnDietDetailsIngredientsN.xMax - 16, pawnDietDetailsIngredientsN.y, 16, 16);
                            GUI.DrawTexture(pawnDietDetailsIngredientsNInsideHeart, ContentFinder<Texture2D>.Get("UI/VCEV_HeartIcon", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
                        }
                    }
                    if (found)
                    {
                        TooltipHandler.TipRegion(pawnDietDetailsIngredientsNInside, GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i].LabelCap + ": " + GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i].description+ "\n\n" + "VCEV_ThisIsFavouriteIng".Translate(pawn.Named("PAWN")));
                    }
                    else
                    {
                        TooltipHandler.TipRegion(pawnDietDetailsIngredientsNInside, GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i].LabelCap + ": " + GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients[i].description);
                    }
                }
                else
                {
                    TooltipHandler.TipRegion(pawnDietDetailsIngredientsNInside, "VCE_NotEatenYet".Translate());

                }

            }
            Rect foodVarietyRect = new Rect(0, outRect.yMin + 340, 160, 30f);
            Widgets.Label(foodVarietyRect, "VCE_MealVariety".Translate(GameComponent_FoodVariety.MealVariety(pawn), VanillaCookingExpandedVariety_Settings.numberOfMeals));
            bool mealHasFavourites = false;
            foreach (ThingDef thingDef in GameComponent_FoodVariety.pawns_and_favourites[pawn])
            {
                if (GameComponent_FoodVariety.pawns_and_diet[pawn].last10Meals.Contains(thingDef))
                {
                    mealHasFavourites = true;
                }
            }
            if(mealHasFavourites)
            {
                TooltipHandler.TipRegion(foodVarietyRect, "VCE_MealsHasFavourites".Translate(pawn.NameShortColored.ToStringSafe()));

            }



            Rect ingredientVarietyRect = new Rect(170, outRect.yMin + 340, 150, 30f);
            Widgets.Label(ingredientVarietyRect, "VCE_IngredientVariety".Translate(GameComponent_FoodVariety.IngredientVariety(pawn), VanillaCookingExpandedVariety_Settings.numberOfIngredients));
            bool ingredientsHasFavourites = false;
            foreach (ThingDef thingDef in GameComponent_FoodVariety.pawns_and_favourites[pawn])
            {
                if (GameComponent_FoodVariety.pawns_and_diet[pawn].last10Ingredients.Contains(thingDef))
                {
                    ingredientsHasFavourites = true;
                }
            }
            if (ingredientsHasFavourites)
            {
                TooltipHandler.TipRegion(ingredientVarietyRect, "VCE_IngredientsHasFavourites".Translate(pawn.NameShortColored.ToStringSafe()));

            }
            
            Rect totalVarietyRect = new Rect(350, outRect.yMin + 340, 150, 30f);
            Widgets.Label(totalVarietyRect, "VCE_TotalVariety".Translate(GameComponent_FoodVariety.Variety(pawn) * 100));

           
        }

        

        public float SizeOfTiles (int numberToScale,int slotNumber,int max)
        {


            return Math.Min(numberToScale * (10f / slotNumber),max);
        }


    }
}