using PeteTimesSix.ResearchReinvented_SteppingStones.ModCompat;
using PeteTimesSix.ResearchReinvented_SteppingStones.Utilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static PeteTimesSix.ResearchReinvented_SteppingStones.Utilities.DefNameSubstitutor;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs
{
    [DefOf]
    public static class ResearchProjectDefOf_Custom
    {
        public static ResearchProjectDef RR_Organization;
        public static ResearchProjectDef RR_LateralThinking;

        public static ResearchProjectDef RR_BasicApparel;
        public static ResearchProjectDef RR_BasicCraftingFacilities;
        public static ResearchProjectDef RR_BasicFoodPrep;

        public static ResearchProjectDef RR_IncendiaryWeapons;

        public static ResearchProjectDef RR_ElectricityBasics;

        //vanilla
        public static ResearchProjectDef ComplexClothing;
        public static ResearchProjectDef Prosthetics;
        public static ResearchProjectDef DrugProduction;
        public static ResearchProjectDef PenoxycylineProduction;

        static ResearchProjectDefOf_Custom()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf_Custom));
        }
    }


    [StaticConstructorOnStartup]
    public static class ResearchProjectDefOf_Manual
    {

        public static ResearchProjectDef Agriculture;
        public static ResearchProjectDef BasicMeleeWeapons;
        public static ResearchProjectDef BasicRangedWeapons;
        public static ResearchProjectDef BasicHerbLore;
        public static ResearchProjectDef BasicFurniture;
        public static ResearchProjectDef BasicStructures;
        public static ResearchProjectDef Trapping;
        public static ResearchProjectDef Fire;


        /*public static ResearchProjectDef RR_Organization;
        public static ResearchProjectDef RR_LateralThinking;

        public static ResearchProjectDef RR_Agriculture;

        public static ResearchProjectDef RR_BasicMeleeWeapons;
        public static ResearchProjectDef RR_BasicRangedWeapons;
        public static ResearchProjectDef RR_BasicApparel;
        public static ResearchProjectDef RR_BasicCraftingFacilities;
        public static ResearchProjectDef RR_BasicFoodPrep;
        public static ResearchProjectDef RR_BasicHerbLore;
        public static ResearchProjectDef RR_BasicFurniture;
        public static ResearchProjectDef RR_BasicStructures;
        public static ResearchProjectDef RR_Trapping;
        public static ResearchProjectDef RR_Fire;

        public static ResearchProjectDef RR_ElectricityBasics;*/

        static ResearchProjectDefOf_Manual()
        {
            DefNameSubstitutor.ReplaceLibCheck();

            Agriculture         = GetNamedResearchDefWithSubstitution("RR_Agriculture");
            BasicMeleeWeapons   = GetNamedResearchDefWithSubstitution("RR_BasicMeleeWeapons");
            BasicRangedWeapons  = GetNamedResearchDefWithSubstitution("RR_BasicRangedWeapons");
            BasicHerbLore       = GetNamedResearchDefWithSubstitution("RR_BasicHerbLore");
            BasicFurniture      = GetNamedResearchDefWithSubstitution("RR_BasicFurniture");
            BasicStructures     = GetNamedResearchDefWithSubstitution("RR_BasicStructures");
            Trapping            = GetNamedResearchDefWithSubstitution("RR_Trapping");
            Fire                = GetNamedResearchDefWithSubstitution("RR_Fire");
        }

        static ResearchProjectDef GetNamedResearchDefWithSubstitution(string defName) 
        {
            var substitution = GetDefNameOrSub(defName);
            var def = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(substitution);
            if (def != null)
            {
                return def;
            }
            else
            {
                if(substitution != defName)
                {
                    def = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(defName);
                    if (def != null)
                    {
                        Log.Warning("RR:SS: Something has deleted substitution def " + substitution + ", defaulting to original " + defName);
                        return def;
                    }
                    else
                    {
                        Log.Warning("RR:SS: Something has deleted substitution def " + substitution + ", original " + defName + " no longer present. Research prerequisites for this def will not be properly reassigned.");
                        return null;
                    }
                }
                else 
                {
                    Log.Warning("RR:SS: Something has deleted def " + defName + ". Research prerequisites for this def will not be properly reassigned.");
                    return null;
                }
            }
        }
    }
}
