using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
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
        public static void DoProjects()
        {
            var noProjectProjects = new HashSet<ResearchProjectDef>();
            foreach (var projectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(p =>
                (p.prerequisites == null || p.prerequisites.Count == 0) &&
                (p.hiddenPrerequisites == null || p.hiddenPrerequisites.Count == 0)))
            {
                noProjectProjects.Add(projectDef);
            }
            noProjectProjects = noProjectProjects.Except(SuperEarlyTechs).ToHashSet();

            foreach (var project in noProjectProjects)
            {
                try
                {
                    GivePrerequisitesToProject(project);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }
        }

        public static void GivePrerequisitesToProject(ResearchProjectDef project)
        {
            if (project.prerequisites == null)
                project.prerequisites = new List<ResearchProjectDef>();

            var unlockPoints = new Dictionary<ResearchProjectDef, int>();
            TotalUnlockPoints(project, unlockPoints);
            if (unlockPoints.Any())
            {
                var maxPoints = unlockPoints.Max(kv => kv.Value);
                var newPreregs = unlockPoints.Where(kv => kv.Value == maxPoints).Select(kv => kv.Key); //in case of equal amounts
                project.prerequisites.AddRange(newPreregs);
            }

            project.prerequisites.Add(ResearchProjectDefOf_Custom.RR_LateralThinking);
        }

        private static void TotalUnlockPoints(ResearchProjectDef project, Dictionary<ResearchProjectDef, int> unlockPoints)
        {
            var unlocks = project.UnlockedDefs;
            foreach (var def in unlocks)
            {
                if (def is TerrainDef terrain)
                {
                    if (terrain.BuildableByPlayer)
                    {
                        if (terrain.costList == null || !terrain.costList.Any())
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_Agriculture);
                        }
                        else
                        {
                            //buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalConstruction);
                        }
                    }
                }
                else if (def is ThingDef thing)
                {
                    if (thing.plant != null)
                    {
                        var plant = thing.plant;
                        if (plant.Sowable)
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_Agriculture);
                        }
                    }
                    else if (thing.BuildableByPlayer)
                    {
                        var buildable = thing;
                        if (buildable.IsElectrical())
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_ElectricityBasics);
                        }
                        if (buildable.IsCraftingFacility())
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicCraftingFacilities);
                        }
                        if (buildable.IsFurniture())
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicFurniture);
                        }
                        if (buildable.IsStructure())
                        {
                            unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicStructures);
                        }
                        //else
                        {
                            //buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalConstruction);
                        }
                    }
                    else
                    {
                        AddPointsForThing(unlockPoints, thing);
                    }
                }
                else if (def is RecipeDef recipe)
                {
                    if (recipe.IsSurgery)
                    {
                        if (recipe.targetsBodyPart)
                        {
                            //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalSurgery);
                        }
                        else
                        {
                            //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalMedicine);
                        }
                    }
                    else
                    {
                        if (recipe.ProducedThingDef == null)
                        {
                            //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalLabor);
                        }
                        else
                        {
                            var product = recipe.ProducedThingDef;
                            AddPointsForThing(unlockPoints, product);
                        }
                    }
                }
            }
        }

        private static void AddPointsForThing(Dictionary<ResearchProjectDef, int> unlockPoints, ThingDef product)
        {
            if (product.IsWeapon)
            {
                if (product.IsRangedWeapon)
                {
                    unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicRangedWeapons);
                }
                else
                {
                    unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicMeleeWeapons);
                }
            }
            else if (product.IsApparel)
            {
                unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicApparel);
            }
            else if (product.IsIngestible)
            {
                if (product.IsNutritionGivingIngestible)
                {
                    unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicFoodPrep);
                }
                else
                {
                    unlockPoints.AddPoint(ResearchProjectDefOf_Custom.RR_BasicHerbLore);
                }
            }
            else
            {
                //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalCrafting);
            }
        }

        private static void AddPoint(this Dictionary<ResearchProjectDef, int> dict, ResearchProjectDef project) 
        {
            if(!dict.ContainsKey(project))
                dict.Add(project, 1);
            else
                dict[project]++;
        }
    }
}
