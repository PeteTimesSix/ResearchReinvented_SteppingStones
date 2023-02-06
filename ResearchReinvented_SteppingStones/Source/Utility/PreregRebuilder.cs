using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Utility
{
    public static class PreregRebuilder
    {
        public static void SetPrerequisitesOnOrphans()
        {
            var noProjectRecipeDefs = new HashSet<RecipeDef>();
            var noProjectSurgeryRecipeDefs = new HashSet<RecipeDef>();
            var noProjectFullBodySurgeryRecipeDefs = new HashSet<RecipeDef>();
            foreach (var recipeDef in DefDatabase<RecipeDef>.AllDefsListForReading.Where(r =>
                !r.AnyResearchPrerequisites())
                )
            {
                if (recipeDef.IsSurgery)
                {
                    if (recipeDef.targetsBodyPart)
                    {
                        noProjectSurgeryRecipeDefs.Add(recipeDef);
                    }
                    else
                    {
                        noProjectFullBodySurgeryRecipeDefs.Add(recipeDef);
                    }
                }
                else
                {
                    noProjectRecipeDefs.Add(recipeDef);
                }
            }
            var noProjectBuildableDefs = new HashSet<ThingDef>();
            var noProjectInstantBuildableDefs = new HashSet<ThingDef>();
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(t =>
                t.BuildableByPlayer &&
                (t.researchPrerequisites == null || !t.researchPrerequisites.Any()))
                )
            {
                if (thingDef.IsInstantBuild())
                {
                    noProjectInstantBuildableDefs.Add(thingDef);
                }
                else
                {
                    noProjectBuildableDefs.Add(thingDef);
                }
            }
            var noProjectPlantDefs = new HashSet<ThingDef>();
            foreach (var plantDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(t =>
                t.plant != null &&
                t.plant.Sowable &&
                (t.plant.sowResearchPrerequisites == null || !t.plant.sowResearchPrerequisites.Any()))
                )
            {
                noProjectPlantDefs.Add(plantDef);
            }
            var noProjectTerrainDefs = new HashSet<TerrainDef>();
            foreach (var terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading.Where(t =>
                t.BuildableByPlayer &&
                (t.researchPrerequisites == null || !t.researchPrerequisites.Any()))
                )
            {
                noProjectTerrainDefs.Add(terrainDef);
            }

            GivePrerequisitesToBuildables(noProjectBuildableDefs, noProjectInstantBuildableDefs);
            GivePrerequisitesToPlants(noProjectPlantDefs);
            GivePrerequisitesToTerrain(noProjectTerrainDefs);
            try
            {
                GivePrerequisitesToRecipes(noProjectRecipeDefs, noProjectSurgeryRecipeDefs, noProjectFullBodySurgeryRecipeDefs);
            }
            catch (InvalidOperationException e)
            {
                Log.Error("error while assinging recipes to projects: " + e.Message);
            }

            ThingDefOf_Custom.RR_ThinkingSpot.researchPrerequisites = null;
        }

        private static void GivePrerequisitesToBuildables(HashSet<ThingDef> noProjectBuildableDefs, HashSet<ThingDef> noProjectInstantBuildableDefs)
        {
            foreach (var buildable in noProjectBuildableDefs)
            {
                if (buildable.researchPrerequisites == null)
                    buildable.researchPrerequisites = new List<ResearchProjectDef>();

                if ((buildable.recipes != null && buildable.recipes.Any()) ||
                    (buildable.inspectorTabs != null && buildable.inspectorTabs.Contains(typeof(ITab_Bills))) ||
                    buildable.thingClass == typeof(Building_ResearchBench))
                {
                    buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicCraftingFacilities);
                }
                else if (buildable.IsBed || buildable.IsTable || buildable.building != null && buildable.building.isSittable)
                {
                    buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicFurniture);
                }
                else if (buildable == ThingDefOf.Wall || buildable.IsFence || buildable.IsDoor || buildable == ThingDefOf.Column)
                {
                    buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicStructures);
                }
                else
                {
                    //buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalConstruction);
                }
            }

            foreach (var buildable in noProjectInstantBuildableDefs)
            {
                if (buildable.researchPrerequisites == null)
                    buildable.researchPrerequisites = new List<ResearchProjectDef>();

                buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_Organization);
            }
        }

        private static void GivePrerequisitesToPlants(HashSet<ThingDef> noProjectPlantDefs)
        {
            foreach (var plant in noProjectPlantDefs)
            {
                if (plant.plant.sowResearchPrerequisites == null)
                    plant.plant.sowResearchPrerequisites = new List<ResearchProjectDef>();

                plant.plant.sowResearchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_Agriculture);
            }
        }

        private static void GivePrerequisitesToTerrain(HashSet<TerrainDef> noProjectTerrainDefs)
        {
            foreach (var terrain in noProjectTerrainDefs)
            {
                if (terrain.researchPrerequisites == null)
                    terrain.researchPrerequisites = new List<ResearchProjectDef>();

                if (terrain.costList == null || !terrain.costList.Any())
                {
                    terrain.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_Agriculture);
                }
                else
                {
                    //terrain.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalConstruction);
                }
            }
        }

        private static void GivePrerequisitesToRecipes(HashSet<RecipeDef> noProjectRecipeDefs, HashSet<RecipeDef> noProjectSurgeryRecipeDefs, HashSet<RecipeDef> noProjectFullBodySurgeryRecipeDefs)
        {
            foreach (var recipe in noProjectRecipeDefs)
            {
                if (recipe.researchPrerequisites == null)
                    recipe.researchPrerequisites = new List<ResearchProjectDef>();

                var firstProjects = recipe.FindEarliestPrerequisiteProjects();
                firstProjects = FilterOutSuperEarlyTechs(firstProjects);
                if (firstProjects != null && firstProjects.Any())
                {
                    recipe.researchPrerequisites.AddRange(firstProjects);
                    continue;
                }

                if (recipe.ProducedThingDef == null)
                {
                    //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalLabor);
                }
                else
                {
                    if (recipe.ProducedThingDef.IsWeapon)
                    {
                        if (recipe.ProducedThingDef.IsRangedWeapon)
                        {
                            recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicRangedWeapons);
                        }
                        else
                        {
                            recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicMeleeWeapons);
                        }
                    }
                    else if (recipe.ProducedThingDef.IsApparel)
                    {
                        recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicApparel);
                    }
                    else if (recipe.ProducedThingDef.IsIngestible)
                    {
                        if (recipe.ProducedThingDef.IsNutritionGivingIngestible)
                        {
                            recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicFoodPrep);
                        }
                        else
                        {
                            recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicHerbLore);
                        }
                    }
                    else
                    {
                        //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalCrafting);
                    }
                }
            }

            foreach (var recipe in noProjectSurgeryRecipeDefs)
            {
                if (recipe.researchPrerequisites == null)
                    recipe.researchPrerequisites = new List<ResearchProjectDef>();

                var firstProjects = recipe.FindEarliestPrerequisiteProjects();
                firstProjects = FilterOutSuperEarlyTechs(firstProjects);
                if (firstProjects != null && firstProjects.Any())
                {
                    recipe.researchPrerequisites.AddRange(firstProjects);
                    continue;
                }

                //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalSurgery);
            }

            foreach (var recipe in noProjectFullBodySurgeryRecipeDefs)
            {
                if (recipe.researchPrerequisites == null)
                    recipe.researchPrerequisites = new List<ResearchProjectDef>();

                var firstProjects = recipe.FindEarliestPrerequisiteProjects();
                firstProjects = FilterOutSuperEarlyTechs(firstProjects);
                if (firstProjects != null && firstProjects.Any())
                {
                    recipe.researchPrerequisites.AddRange(firstProjects);
                    continue;
                }

                //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalMedicine);
            }
        }

        private static HashSet<ResearchProjectDef> _superEarlyTechs;
        public static HashSet<ResearchProjectDef> SuperEarlyTechs
        {
            get
            {
                if (_superEarlyTechs == null)
                    _superEarlyTechs = new HashSet<ResearchProjectDef>() {
                        ResearchProjectDefOf_Custom.RR_Organization,
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
