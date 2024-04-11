using System;
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

        public static string GetDefNameOrSub(string defName)
        {
            return defNameSubstitutions.GetValueOrDefault(defName, defName);
        }

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
    }
}
