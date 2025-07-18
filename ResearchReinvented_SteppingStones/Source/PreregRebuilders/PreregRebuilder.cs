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
            DoBuildables();
            DoPlants();
            DoTerrains();
            DoRecipes(); //for some reason Im not quite sure of right now, doing this first results in wrong assingments.
            DoProjects();

            {
                var spot = ThingDefOf_Custom.RR_ThinkingSpot;
                if(spot.researchPrerequisites != null)
                    spot.researchPrerequisites.RemoveAll(p => p == ResearchProjectDefOf_Custom.RR_Organization);
                if (spot.researchPrerequisites.Count == 0)
                    spot.researchPrerequisites = null;
            }

            /*{ 
                //remove lateral thinking entirely if its not used
                if(!DefDatabase<ResearchProjectDef>.AllDefs.Any(r => r.AllPrerequisiteProjects().Contains(ResearchProjectDefOf_Custom.RR_LateralThinking)))
                {
                }
            }*/
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
                        ResearchProjectDefOf_Manual.Agriculture,
                        ResearchProjectDefOf_Manual.BasicMeleeWeapons,
                        ResearchProjectDefOf_Manual.BasicRangedWeapons,
                        ResearchProjectDefOf_Custom.RR_BasicApparel,
                        ResearchProjectDefOf_Custom.RR_BasicCraftingFacilities,
                        ResearchProjectDefOf_Custom.RR_BasicFoodPrep,
                        ResearchProjectDefOf_Manual.BasicHerbLore,
                        ResearchProjectDefOf_Manual.BasicFurniture,
                        ResearchProjectDefOf_Manual.BasicStructures };

                if (_superEarlyTechs.Contains(null))
                    _superEarlyTechs.Remove(null);

                return _superEarlyTechs;
            }
        }

        private static HashSet<ResearchProjectDef> _blacklistTechs;
        public static HashSet<ResearchProjectDef> BlacklistTechs
        {
            get
            {
                if (_blacklistTechs == null)
                    _blacklistTechs = new HashSet<ResearchProjectDef>();

                var techBlockStarterTech = DefDatabase<ResearchProjectDef>.GetNamed("TB_NeolithicTechLock", false);
                if(techBlockStarterTech != null)
                    _blacklistTechs.Add(techBlockStarterTech);

                if (_blacklistTechs.Contains(null))
                    _blacklistTechs.Remove(null);

                return _blacklistTechs;
            }
        }

        private static HashSet<ResearchProjectDef> FilterOutUnwantedTechs(HashSet<ResearchProjectDef> projects)
        {
            if (projects == null)
                return null;
            return projects.Except(SuperEarlyTechs).Except(BlacklistTechs).ToHashSet();
        }
    }
}
