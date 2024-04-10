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
            Agriculture         = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_Agriculture"));
            BasicMeleeWeapons   = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_BasicMeleeWeapons"));
            BasicRangedWeapons  = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_BasicRangedWeapons"));
            BasicHerbLore       = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_BasicHerbLore"));
            BasicFurniture      = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_BasicFurniture"));
            BasicStructures     = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_BasicStructures"));
            Trapping            = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_Trapping"));
            Fire                = DefDatabase<ResearchProjectDef>.GetNamed(GetDefNameOrSub("RR_Fire"));
        }
    }
}
