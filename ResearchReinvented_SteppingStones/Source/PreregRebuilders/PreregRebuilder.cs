using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
using PeteTimesSix.ResearchReinvented_SteppingStones.PreregRebuilders;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.PreregRebuilders
{
    public static partial class PreregRebuilder
    {
        public static void SetPrerequisitesOnOrphans()
        {
            DoRecipes();
            DoBuildables();
            DoPlants();
            DoTerrains();
            DoProjects();

            ThingDefOf_Custom.RR_ThinkingSpot.researchPrerequisites = null;
        }

        private static HashSet<ResearchProjectDef> _superEarlyTechs;
        public static HashSet<ResearchProjectDef> SuperEarlyTechs
        {
            get
            {
                if (_superEarlyTechs == null)
                    _superEarlyTechs = new HashSet<ResearchProjectDef>() {
                        ResearchProjectDefOf_Custom.RR_Organization,
                        ResearchProjectDefOf_Custom.RR_LateralThinking,
                        ResearchProjectDefOf_Custom.RR_Agriculture,
                        ResearchProjectDefOf_Custom.RR_BasicMeleeWeapons,
                        ResearchProjectDefOf_Custom.RR_BasicRangedWeapons,
                        ResearchProjectDefOf_Custom.RR_BasicApparel,
                        ResearchProjectDefOf_Custom.RR_BasicCraftingFacilities,
                        ResearchProjectDefOf_Custom.RR_BasicFoodPrep,
                        ResearchProjectDefOf_Custom.RR_BasicHerbLore,
                        ResearchProjectDefOf_Custom.RR_BasicFurniture,
                        ResearchProjectDefOf_Custom.RR_BasicStructures };
                return _superEarlyTechs;
            }
        }

        private static HashSet<ResearchProjectDef> FilterOutSuperEarlyTechs(HashSet<ResearchProjectDef> projects)
        {
            if (projects == null)
                return null;
            return projects.Except(SuperEarlyTechs).ToHashSet();
        }
    }
}
