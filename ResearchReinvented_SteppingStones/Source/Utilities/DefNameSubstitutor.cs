using HarmonyLib;
using PeteTimesSix.ResearchReinvented_SteppingStones.ModCompat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Utilities
{
    public  class DefNameSubstitutor
    {
        public static Dictionary<string, string> defNameSubstitutions = new();

        public static string GetDefNameOrSub(string defName) => defNameSubstitutions.GetValueOrDefault(defName, defName);


        static DefNameSubstitutor()
        {
            if (ModsConfig.IsActive("OskarPotocki.VFE.Tribals"))
            {
                defNameSubstitutions["RR_Fire"] = "VFET_Fire";
                defNameSubstitutions["RR_Trapping"] = "VFET_Hunting";
                defNameSubstitutions["RR_BasicMeleeWeapons"] = "VFET_Weapons";
                defNameSubstitutions["RR_BasicRangedWeapons"] = "VFET_Bow";
                defNameSubstitutions["RR_BasicStructures"] = "VFET_Construction";
                defNameSubstitutions["RR_BasicFurniture"] = "VFET_Furniture";
                defNameSubstitutions["RR_Agriculture"] = "VFET_Agriculture";
                defNameSubstitutions["RR_BasicHerbLore"] = "VFET_Medicine";
                //VFET_Cultivation
                //VFET_Tribalwear
                //VFET_Mining
                //VFET_AnimalHandling
                //VFET_Culture
            }
        }
        public static void ReplaceLibCheck()
        {
            if (ReplaceLib.active)
            {
                //Log.Message("RR:SS: checking for substitutions from ReplaceLib...");
                var defDbType = typeof(DefDatabase<>).MakeGenericType(new Type[] { ReplaceLib.type_ReplacerDef });
                var defs = Traverse.Create(defDbType).Property("AllDefsListForReading").GetValue() as IEnumerable;
                foreach (object defObj in defs)
                {
                    var def = defObj as Def;
                    if (ReplaceLib.type_ReplacerDef.IsAssignableFrom(def.GetType()))
                    {
                        //Log.Message("found replacer def " + def.defName);
                        var replaceData = ReplaceLib.field_replacers(def);
                        foreach (var data in replaceData)
                        {
                            var replace = ReplaceLib.field_replace(data);
                            var with = ReplaceLib.field_with(data);

                            if (defNameSubstitutions.ContainsKey(replace))
                            {
                                //Log.Message("swapping key " + replace + " for " + with);
                                var value = defNameSubstitutions[replace];
                                defNameSubstitutions.Remove(replace);
                                defNameSubstitutions[with] = value;
                            }

                            List<string> valuesToSwap = new();
                            foreach (var (key, value) in defNameSubstitutions)
                            {
                                if (replace == value)
                                {
                                    valuesToSwap.Add(key);
                                }
                            }
                            foreach (var key in valuesToSwap)
                            {
                                //Log.Message("swapping value (with key " + key + ") " + replace + " for " + with);
                                defNameSubstitutions[key] = with;
                            }
                        }
                    }
                }
                //Log.Message("RR:SS: done checking for substitutions from ReplaceLib.");
            }
        }
    }
}
