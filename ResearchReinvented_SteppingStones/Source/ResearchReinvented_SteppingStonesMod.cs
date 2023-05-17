using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Patches;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
using PeteTimesSix.ResearchReinvented_SteppingStones.PreregRebuilders;

namespace PeteTimesSix.ResearchReinvented_SteppingStones
{
    public class ResearchReinvented_SteppingStonesMod : Mod
    {
        public static ResearchReinvented_SteppingStonesMod ModSingleton { get; private set; }
        public static ResearchReinvented_SteppingStones_Settings Settings { get; set; }
        public static Harmony Harmony { get; internal set; }

        public ResearchReinvented_SteppingStonesMod(ModContentPack content) : base(content)
        {
            ModSingleton = this;

            Harmony = new Harmony("PeteTimesSix.ResearchReinvented_SteppingStones");
            Harmony.PatchAll();

			Settings = GetSettings<ResearchReinvented_SteppingStones_Settings>();
		}


		public override string SettingsCategory()
		{
			return "ResearchReinvented_SteppingStones_ModTitle".Translate();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Settings.DoSettingsWindowContents(inRect);
		}
	}

    [StaticConstructorOnStartup]
    public static class ResearchReinventedSteppingStones_PostInit
    {
        static ResearchReinventedSteppingStones_PostInit()
        {
            PreregRebuilder.SetPrerequisitesOnOrphans();
            MainTabWindow_Research_OffsetHacks.BuildTabOffsets();
        }
    }
}
