using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs
{
    [DefOf]
    public static class ResearchProjectDefOf_Custom
    {
        public static ResearchProjectDef RR_Organization;
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
}
