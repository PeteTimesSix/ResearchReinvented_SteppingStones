using HarmonyLib;
using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
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
    [HarmonyPatch(typeof(MainTabWindow_Research), "DrawStartButton")]
    public static class MainTabWindow_Research_DrawRightRect_Patches
    {

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DrawRightRect(IEnumerable<CodeInstruction> instructions)
        {
            instructions = DoTranspiler(instructions);
            return instructions;
        }

        private static IEnumerable<CodeInstruction> DoTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            //so all this actually does is fix up the values produced by the dev repositioning tool
            var matcher = new CodeMatcher(instructions);

            var target = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Widgets), nameof(Widgets.DrawHighlight), new Type[] {typeof(Rect)})),
                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(OpCodes.Ldc_R4)
            };

            matcher.MatchEndForward(target);
            if (!matcher.IsValid)
            {
                Log.Error("RR_SS: Failed to apply research unavailable button resize hack!");
                return instructions;
            }
            else
            {
                var clone = matcher.Instruction.Clone();
                matcher.RemoveInstruction();
                clone.operand = 0.0f;
                matcher.Insert(clone);
            }
            return matcher.InstructionEnumeration();
        }
    }
}
