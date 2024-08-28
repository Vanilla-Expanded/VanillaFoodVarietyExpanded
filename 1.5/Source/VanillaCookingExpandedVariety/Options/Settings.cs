using RimWorld;
using System;
using UnityEngine;
using Verse;


namespace VanillaCookingExpandedVariety
{


    public class VanillaCookingExpandedVariety_Settings : ModSettings

    {



        public static int moodMultiplier = baseMoodMultiplier;
        public const int baseMoodMultiplier = 5;

        public static float lowVarietyThreshold = baseLowVarietyThreshold;
        public const float baseLowVarietyThreshold = 0.3f;

        public static float highVarietyThreshold = baseHighVarietyThreshold;
        public const float baseHighVarietyThreshold = 0.6f;

        public static int numberOfMeals = baseNumberOfMeals;
        public const int baseNumberOfMeals = 7;

        public static int numberOfIngredients = baseNumberOfIngredients;
        public const int baseNumberOfIngredients = 7;


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref moodMultiplier, "moodMultiplier", baseMoodMultiplier);
            Scribe_Values.Look(ref lowVarietyThreshold, "lowVarietyThreshold", baseLowVarietyThreshold);
            Scribe_Values.Look(ref highVarietyThreshold, "highVarietyThreshold", baseHighVarietyThreshold);
            Scribe_Values.Look(ref numberOfMeals, "numberOfMeals", baseNumberOfMeals);
            Scribe_Values.Look(ref numberOfIngredients, "numberOfIngredients", baseNumberOfIngredients);

        }

      

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);

           
            var moodLabel = ls.LabelPlusButton("VCEV_MoodMultiplier".Translate() + ": " + moodMultiplier, "VCEV_MoodMultiplierDesc".Translate());
            moodMultiplier = (int)Math.Round(ls.Slider(moodMultiplier, 0, 40), 0);

            if (ls.Settings_Button("VCEV_Reset".Translate(), new Rect(0f, moodLabel.position.y + 35, 250f, 29f)))
            {
                moodMultiplier = baseMoodMultiplier;
            }

            var lowLabel = ls.LabelPlusButton("VCEV_LowVarietyThreshold".Translate() + ": " + lowVarietyThreshold, "VCEV_LowVarietyThresholdDesc".Translate());
            lowVarietyThreshold = (float)Math.Round(ls.Slider(lowVarietyThreshold, 0, highVarietyThreshold-0.1f), 1);

            if (ls.Settings_Button("VCEV_Reset".Translate(), new Rect(0f, lowLabel.position.y + 35, 250f, 29f)))
            {
                lowVarietyThreshold = baseLowVarietyThreshold;
            }

            var highLabel = ls.LabelPlusButton("VCEV_HighVarietyThreshold".Translate() + ": " + highVarietyThreshold, "VCEV_HighVarietyThresholdDesc".Translate());
            highVarietyThreshold = (float)Math.Round(ls.Slider(highVarietyThreshold, lowVarietyThreshold+0.1f, 1), 1);

            if (ls.Settings_Button("VCEV_Reset".Translate(), new Rect(0f, highLabel.position.y + 35, 250f, 29f)))
            {
                highVarietyThreshold = baseHighVarietyThreshold;
            }

            var mealsLabel = ls.LabelPlusButton("VCEV_NumberOfMeals".Translate() + ": " + numberOfMeals, "VCEV_NumberOfMealsDesc".Translate());
            numberOfMeals = (int)Math.Round(ls.Slider(numberOfMeals, 2, 20), 0);

            if (ls.Settings_Button("VCEV_Reset".Translate(), new Rect(0f, mealsLabel.position.y + 35, 250f, 29f)))
            {
                numberOfMeals = baseNumberOfMeals;
            }

            var ingredientsLabel = ls.LabelPlusButton("VCEV_NumberOfIngredients".Translate() + ": " + numberOfIngredients, "VCEV_NumberOfIngredientsDesc".Translate());
            numberOfIngredients = (int)Math.Round(ls.Slider(numberOfIngredients, 2, 20), 0);

            if (ls.Settings_Button("VCEV_Reset".Translate(), new Rect(0f, ingredientsLabel.position.y + 35, 250f, 29f)))
            {
                numberOfIngredients = baseNumberOfIngredients;
            }

            ls.End();
        }

       



    }










}
