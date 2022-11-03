using HarmonyLib;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utility;
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
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DrawRightRect(IEnumerable<CodeInstruction> instructions)
        {
            instructions = DoOffsetTranspiler(instructions);
            instructions = DoTechLevelTranspiler(instructions);
            return instructions;
        }

        private static IEnumerable<CodeInstruction> DoOffsetTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions);

            var targetSiteBefore = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(TexButton), nameof(TexButton.Copy))),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Widgets), nameof(Widgets.ButtonImageFitted), new Type[] { typeof(Rect), typeof(Texture2D) })),
                new CodeMatch(OpCodes.Brfalse),
                new CodeMatch(OpCodes.Newobj, AccessTools.Constructor(typeof(StringBuilder), new Type[]{})),
                new CodeMatch(OpCodes.Stloc_S)
            };

            var toggleOnInstructions = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Stsfld, AccessTools.Field(typeof(MainTabWindow_Research_OffsetHacks), nameof(MainTabWindow_Research_OffsetHacks.bypass))),
            };

            var targetSiteAfter = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(object), nameof(object.ToString))),
                new CodeMatch(OpCodes.Call, AccessTools.PropertySetter(typeof(GUIUtility), nameof(GUIUtility.systemCopyBuffer)))
            };

            var toggleOffInstructions = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Stsfld, AccessTools.Field(typeof(MainTabWindow_Research_OffsetHacks), nameof(MainTabWindow_Research_OffsetHacks.bypass))),
            };

            matcher.MatchEndForward(targetSiteBefore);
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
                    matcher.InsertAndAdvance(toggleOffInstructions);
                    return matcher.InstructionEnumeration();
                }
            }
        }

        private static IEnumerable<CodeInstruction> DoTechLevelTranspiler(IEnumerable<CodeInstruction> instructions)
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
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MainTabWindow_Research_OffsetHack_DrawRightRect), nameof(MainTabWindow_Research_OffsetHack_DrawRightRect.DrawTechLevel))),
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
        }

        public static void DrawTechLevel(Rect rect, ResearchProjectDef researchProject) 
        {
            var sideRect = rect.LeftPartPixels(5f).ContractedBy(1f);
            Widgets.DrawBoxSolid(sideRect, TechLevelColorUtil.ColorAvailable[researchProject.techLevel]);
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
