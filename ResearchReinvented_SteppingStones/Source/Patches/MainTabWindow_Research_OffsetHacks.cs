﻿using HarmonyLib;
using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Patches
{

    [HarmonyPatch(typeof(ResearchProjectDef), "ClampInCoordinateLimits")]
    public static class MainTabWindow_Research_OffsetHack_ClampDisable
    {
        [HarmonyPrefix]
        public static bool ClampInCoordinateLimits()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(ResearchProjectDef), nameof (ResearchProjectDef.ResearchViewX), MethodType.Getter)]
    public static class MainTabWindow_Research_OffsetHack_X
    {
        [HarmonyPostfix]
        public static void ResearchViewX(ref float __result, ResearchProjectDef __instance) 
        {
            if (!MainTabWindow_Research_OffsetHacks.offsetsBuilt || MainTabWindow_Research_OffsetHacks.bypass)
                return;
            var offsets = MainTabWindow_Research_OffsetHacks.TabOffsets[__instance.tab];
            __result -= offsets.x;
        }
    }

    [HarmonyPatch(typeof(ResearchProjectDef), nameof(ResearchProjectDef.ResearchViewY), MethodType.Getter)]
    public static class MainTabWindow_Research_OffsetHack_Y
    {
        [HarmonyPostfix]
        public static void ResearchViewY(ref float __result, ResearchProjectDef __instance)
        {
            if (!MainTabWindow_Research_OffsetHacks.offsetsBuilt || MainTabWindow_Research_OffsetHacks.bypass)
                return;
            var offsets = MainTabWindow_Research_OffsetHacks.TabOffsets[__instance.tab];
            __result -= offsets.y;
        }
    }

    [HarmonyPatch(typeof(MainTabWindow_Research), "DrawRightRect")]
    public static class MainTabWindow_Research_OffsetHack_DrawRightRect
    {
        [HarmonyPostfix]
        public static void Postfix()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf_Custom.RR_SteppingStones_Intro, OpportunityType.Important);
		}

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DrawRightRect(IEnumerable<CodeInstruction> instructions)
        {
            instructions = DoOffsetTranspiler(instructions);
            //instructions = DoTechLevelTranspiler(instructions);
            return instructions;
        }

        private static IEnumerable<CodeInstruction> DoOffsetTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            //so all this actually does is fix up the values produced by the dev repositioning tool
            var matcher = new CodeMatcher(instructions);

            var targetSiteBefore = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldstr, "  <researchViewX>{0:F2}</researchViewX>"),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(ResearchProjectDef), nameof(ResearchProjectDef.ResearchViewX)))
            };

            var toggleOnInstructions = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Stsfld, AccessTools.Field(typeof(MainTabWindow_Research_OffsetHacks), nameof(MainTabWindow_Research_OffsetHacks.bypass))),
            };

            var targetSiteAfter = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldstr, "  <researchViewY>{0:F2}</researchViewY>"),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(ResearchProjectDef), nameof(ResearchProjectDef.ResearchViewY)))
            };

            var toggleOffInstructions = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Stsfld, AccessTools.Field(typeof(MainTabWindow_Research_OffsetHacks), nameof(MainTabWindow_Research_OffsetHacks.bypass))),
            };

            matcher.MatchStartForward(targetSiteBefore);
            if (!matcher.IsValid)
            {
                Log.Error("RR_SS: Failed to apply research window patch for offsets (phase 1)!");
                return instructions;
            }
            else
            {
                matcher.InsertAndAdvance(toggleOnInstructions);
                matcher.MatchEndForward(targetSiteAfter);
                if (!matcher.IsValid)
                {
                    Log.Error("RR_SS: Failed to apply research window patch for offsets (phase 2)!");
                    return instructions;
                }
                else
                {
                    matcher.Advance(1);
                    matcher.InsertAndAdvance(toggleOffInstructions);
                    return matcher.InstructionEnumeration();
                }
            }
        }

        /*private static IEnumerable<CodeInstruction> DoTechLevelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions);

            var targetSite = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(MainTabWindow_Research), "TopCornerTex")),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(GUI), nameof(GUI.DrawTexture), new Type[] { typeof(Rect), typeof(Texture) }))
            };

            var drawTechLevelInstructions = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, 20), 
                new CodeInstruction(OpCodes.Ldloc_S, 18),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MainTabWindow_Research_OffsetHack_DrawRightRect), nameof(MainTabWindow_Research_OffsetHack_DrawRightRect.DrawExtras))),
            };

            matcher.MatchEndForward(targetSite);
            matcher.InsertAndAdvance(drawTechLevelInstructions);
            if (!matcher.IsValid)
            {
                Log.Error("RR_SS: Failed to apply research window patch for tech level drawing!");
                return instructions;
            }
            else
            {
                return matcher.InstructionEnumeration();
            }
        }*/

        public static void DrawExtras(Rect rect, ResearchProjectDef researchProject)
        {
            DrawTechLevel(rect, researchProject);
            if(researchProject.ProgressPercent > 0 && researchProject.ProgressPercent < 1)
                DrawProgress(rect, researchProject);
        }
        public static void DrawTechLevel(Rect rect, ResearchProjectDef researchProject) 
        {
            var sideRect = rect.LeftPartPixels(5f).ContractedBy(1f);
            Widgets.DrawBoxSolid(sideRect, TechLevelColorUtil.ColorAvailable[researchProject.techLevel]);
        }

        public static readonly Color SemiFinishedResearchColor = new ColorInt(0, 64, 16, 65).ToColor;
        public static void DrawProgress(Rect rect, ResearchProjectDef researchProject)
        {
            var progRect = rect.ContractedBy(1f).LeftPart(researchProject.ProgressPercent);
            Widgets.DrawBoxSolid(progRect, SemiFinishedResearchColor);
        }
    }

    public static class MainTabWindow_Research_OffsetHacks
    {

        public static Dictionary<ResearchTabDef, Vector2> TabOffsets = new Dictionary<ResearchTabDef, Vector2>();
        public static bool offsetsBuilt = false;
        public static bool bypass = false;

        public static void BuildTabOffsets()
        {
            var fieldX = AccessTools.FieldRefAccess<ResearchProjectDef, float>("x");
            var fieldY = AccessTools.FieldRefAccess<ResearchProjectDef, float>("y");

            foreach (var tab in DefDatabase<ResearchTabDef>.AllDefsListForReading)
            {
                var projects = DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(p => p.tab == tab || p.tab == null);
                if (!projects.Any())
                {
                    TabOffsets[tab] = new Vector2(0, 0);
                    continue;
                }
                var minX = projects.Min(p => fieldX(p));
                var minY = projects.Min(p => fieldY(p));
                var offsetX = Math.Min(0, minX);
                var offsetY = Math.Min(0, minY);
                /*if(offsetX < 0 || offsetY < 0) 
                {
                    foreach(var project in projects)
                    {
                        fieldX(project) -= offsetX;
                        fieldY(project) -= offsetY;
                    }
                }*/
                TabOffsets[tab] = new Vector2(offsetX, offsetY);
            }

            offsetsBuilt = true;
        }
    }
}
