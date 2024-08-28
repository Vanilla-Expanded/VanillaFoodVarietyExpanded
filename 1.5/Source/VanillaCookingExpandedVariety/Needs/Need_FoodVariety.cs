using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using UnityEngine.Diagnostics;
using Verse;

namespace VanillaCookingExpandedVariety;

public class Need_FoodVariety : Need
{



    public Need_FoodVariety(Pawn pawn) : base(pawn) =>
        threshPercents = new()
        {
            0.1f,
            0.2f,
            0.3f,
            0.4f,
            0.5f,
            0.6f,
            0.7f,
            0.8f,
            0.9f
        };



    public override bool ShowOnNeedList => GameComponent_FoodVariety.colonists_with_foodvariety_need.Contains(pawn);


    public override void NeedInterval()
    {
        if (ShowOnNeedList) { 
            CurLevel = GameComponent_FoodVariety.Variety(pawn); 
        }
        if (pawn.IsHashIntervalTick(6000))
        {
            GameComponent_FoodVariety.RefreshList(null);
        }
        
    }

    public override void SetInitialLevel()
    {
        base.SetInitialLevel();
        CurLevelPercentage = 0f;
        
            
        
       

    }

    public void OpenDetailsWindow()
    {
        Window_Details detailsImageWindow = new Window_Details(pawn);
        Find.WindowStack.Add(detailsImageWindow);
       
    }

    public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1, bool drawArrows = true, bool doTooltip = true,
        Rect? rectForTooltip = null, bool drawLabel = true)
    {
        var tooltipRect = rectForTooltip ?? rect;
        var margin = customMargin >= 0f ? customMargin : 14f + 15f;
        tooltipRect.height += (rect.width - margin * 2) / 10;
        var needRect = new Rect(rect.x + margin, rect.y + rect.height - 10, 16,16);

       
        GUI.DrawTexture(needRect, ContentFinder<Texture2D>.Get("UI/VCEV_MagnifyingGlass", true), ScaleMode.ScaleToFit);

        if (Mouse.IsOver(needRect))
        {
            Widgets.DrawHighlight(needRect);
            doTooltip = false;
            TooltipHandler.TipRegion(needRect, "VCE_FavouriteFood".Translate(Util.ThingDefsToLabels(GameComponent_FoodVariety.pawns_and_favourites[pawn]).ToStringSafeEnumerable()) + "\n\n" +
                "VCE_LastFoods".Translate(Util.ThingDefsToLabels(GameComponent_FoodVariety.pawns_and_diet[pawn]?.last10Meals).ToStringSafeEnumerable()) + "\n\n" +
                "VCE_LastIngredients".Translate(Util.ThingDefsToLabels(GameComponent_FoodVariety.pawns_and_diet[pawn]?.last10Ingredients).ToStringSafeEnumerable()));
        }
        if (Widgets.ButtonInvisible(needRect))
        {
            OpenDetailsWindow();
        }


        base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, tooltipRect, drawLabel);
    }

  

}
