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

namespace PeteTimesSix.ResearchReinvented_SteppingStones
{
    public class ResearchReinvented_SteppingStonesMod : Mod
    {
        public static ResearchReinvented_SteppingStonesMod ModSingleton { get; private set; }

        public ResearchReinvented_SteppingStonesMod(ModContentPack content) : base(content)
        {
            ModSingleton = this;
        }

        /*public override string SettingsCategory()
        {
            return "ResearchReinvented_SteppingStones_ModTitle".Translate();
        }*/
	}

    [StaticConstructorOnStartup]
    public static class ResearchReinvented_PostInit
    {

    }
}
