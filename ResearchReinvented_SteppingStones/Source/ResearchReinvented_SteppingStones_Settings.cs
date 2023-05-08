using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones
{
    public class ResearchReinvented_SteppingStones_Settings : ModSettings
    {
        public const float HEADER_HEIGHT = 35f;
        public const float HEADER_GAP_HEIGHT = 10f;

        public bool doPowerPreregs = true; 

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref doPowerPreregs, "doPowerPreregs", true);
        }
        public void DoSettingsWindowContents(Rect inRect)
        {
            Color preColor = GUI.color;
            var anchor = Text.Anchor;

            var headerRect = inRect.TopPartPixels(HEADER_HEIGHT);
            var restOfRect = inRect.BottomPartPixels(inRect.height - (HEADER_HEIGHT + HEADER_GAP_HEIGHT));

            /*Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(headerRect);

            listingStandard.EnumSelector(null, ref temp_activeTab, "RR_setting_tab_", lineHeightOverride: HEADER_HEIGHT);

            listingStandard.End();*/

            DoGlobalConfigTab(restOfRect);

            Text.Anchor = anchor;
            GUI.color = preColor;
        }

        private void DoGlobalConfigTab(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            float maxWidth = listingStandard.ColumnWidth;

            listingStandard.Indent(maxWidth / 4f);
            listingStandard.ColumnWidth = maxWidth / 2f;
            listingStandard.CheckboxLabeled("RR_SS_setting_doPowerPreregs".Translate(), ref doPowerPreregs, "RR_setting_doPowerPreregs_tooltip".Translate());

            listingStandard.End();
        }
    }
}
