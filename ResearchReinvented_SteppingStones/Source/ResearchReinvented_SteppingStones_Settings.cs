using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones
{
    public enum SurgeryPreregsMode 
    {
        Off,
        Easy,
        Normal
    }

    public class ResearchReinvented_SteppingStones_Settings : ModSettings
    {
        public const float HEADER_HEIGHT = 35f;
        public const float HEADER_GAP_HEIGHT = 10f;

        public bool doPowerPreregs = true;

        public SurgeryPreregsMode surgeryPreregsMode = SurgeryPreregsMode.Normal;

		public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref doPowerPreregs, "doPowerPreregs", true);
			Scribe_Values.Look(ref surgeryPreregsMode, "surgeryPreregsMode", SurgeryPreregsMode.Normal);
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
            listingStandard.CheckboxLabeled("RR_SS_setting_doPowerPreregs".Translate(), ref doPowerPreregs, "RR_SS_setting_doPowerPreregs_tooltip".Translate());

            listingStandard.EnumSelector("RR_SS_setting_doSurgeryPreregs".Translate(), ref surgeryPreregsMode, "RR_SS_setting_doSurgeryPreregs_option_", tooltip: "RR_SS_setting_doSurgeryPreregs_tooltip".Translate());

			float remainingHeight = inRect.height - listingStandard.CurHeight;
			listingStandard.Gap(remainingHeight - Text.LineHeight * 1.5f);
            listingStandard.Label("RR_settings_restartWarning".Translate());

			listingStandard.End();


        }
    }
}
