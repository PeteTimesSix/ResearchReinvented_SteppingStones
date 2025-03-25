using HarmonyLib;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.ModCompat
{
    [StaticConstructorOnStartup]
    public static class ReplaceLib
    {
        public static bool active = false;

        public static Type type_ReplacerDef;
        public static Type type_ReplaceData;

        public static AccessTools.FieldRef<object, IEnumerable> field_replacers;
        public static AccessTools.FieldRef<object, string> field_replace;
        public static AccessTools.FieldRef<object, string> field_with;

        static ReplaceLib()
        {
            active = ModLister.GetActiveModWithIdentifier("ferny.ReplaceLib") != null;

            if(active)
            {
                type_ReplacerDef = AccessTools.TypeByName("ReplaceLib.ReplacerDef");
                type_ReplaceData = AccessTools.TypeByName("ReplaceLib.ReplaceData");

                field_replacers = AccessTools.FieldRefAccess<IEnumerable>(type_ReplacerDef, "replacers");

                field_replace = AccessTools.FieldRefAccess<string>(type_ReplaceData, "replace");
                field_with = AccessTools.FieldRefAccess<string>(type_ReplaceData, "with");
            }
        }
    }
}
