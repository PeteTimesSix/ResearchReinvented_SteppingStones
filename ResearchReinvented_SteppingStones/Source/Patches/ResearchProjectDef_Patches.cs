using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static HarmonyLib.AccessTools;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Patches
{
    /*[HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.GenerateNonOverlappingCoordinates))]
    public static class ResearchProjectDef_GenerateNonOverlappingCoordinates_Patches
    {
        private static FieldRef<ResearchProjectDef, float> fieldX = AccessTools.FieldRefAccess<ResearchProjectDef, float>("x");
        private static FieldRef<ResearchProjectDef, float> fieldY = AccessTools.FieldRefAccess<ResearchProjectDef, float>("y");

        [HarmonyPrefix]
        public static bool Prefix()
        {

            foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
            {
                fieldX(researchProjectDef) = researchProjectDef.researchViewX;
                fieldY(researchProjectDef) = researchProjectDef.researchViewY;
            }
            return false;
        }
    }*/

    [HarmonyPatch(typeof(ResearchProjectDef), "ClampInCoordinateLimits")]
    public static class ResearchProjectDef_ClampInCoordinateLimits_Patches
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.ConfigErrors))]
    public static class ResearchProjectDef_ConfigErrors_Patches
    {
        [HarmonyPostfix]
        public static IEnumerable<string> Postfix(IEnumerable<string> __result)
        {
            foreach(string configError in __result) 
            {
                if(configError != "researchViewX and/or researchViewY not set")
                    yield return configError;
            }
        }

    }
}
