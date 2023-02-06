using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
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

        public static void DoRecipes()
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

            foreach (var recipe in noProjectRecipeDefs)
            {
                try
                {
                    GivePrerequisitesToRecipe(recipe);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }

            foreach (var recipe in noProjectSurgeryRecipeDefs)
            {
                try
                {
                    GivePrerequisitesToSurgery(recipe);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }

            foreach (var recipe in noProjectFullBodySurgeryRecipeDefs)
            {
                try
                {
                    GivePrerequisitesToFullBodySurgery(recipe);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }
        }

        private static void GivePrerequisitesToRecipe(RecipeDef recipe) 
        {
            if (recipe.researchPrerequisites == null)
                recipe.researchPrerequisites = new List<ResearchProjectDef>();

            var firstProjects = recipe.FindEarliestPrerequisiteProjects();
            firstProjects = FilterOutSuperEarlyTechs(firstProjects);
            if (firstProjects != null && firstProjects.Any())
            {
                recipe.researchPrerequisites.AddRange(firstProjects);
                return;
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

        private static void GivePrerequisitesToSurgery(RecipeDef recipe) 
        {
            if (recipe.researchPrerequisites == null)
                recipe.researchPrerequisites = new List<ResearchProjectDef>();

            var firstProjects = recipe.FindEarliestPrerequisiteProjects();
            firstProjects = FilterOutSuperEarlyTechs(firstProjects);
            if (firstProjects != null && firstProjects.Any())
            {
                recipe.researchPrerequisites.AddRange(firstProjects);
            }
            //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalSurgery);
        }

        private static void GivePrerequisitesToFullBodySurgery(RecipeDef recipe) 
        { 
            if (recipe.researchPrerequisites == null)
                recipe.researchPrerequisites = new List<ResearchProjectDef>();

            var firstProjects = recipe.FindEarliestPrerequisiteProjects();
            firstProjects = FilterOutSuperEarlyTechs(firstProjects);
            if (firstProjects != null && firstProjects.Any())
            {
                recipe.researchPrerequisites.AddRange(firstProjects);
            }
            //recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalMedicine);
        }
    }
}
