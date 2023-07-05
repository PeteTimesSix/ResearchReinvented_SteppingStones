using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
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
        private static Dictionary<BodyPartDef, List<BodyPartRecord>> bodyPartRecordCache = new Dictionary<BodyPartDef, List<BodyPartRecord>>();

        public static void DoRecipes()
        {
            var noProjectRecipeDefs = new HashSet<RecipeDef>();
            var noProjectSurgeryRecipeDefs = new HashSet<RecipeDef>();
            var noProjectFullBodySurgeryRecipeDefs = new HashSet<RecipeDef>();

            var indeterminateSurgeries = new HashSet<RecipeDef>();

            foreach (var recipeDef in DefDatabase<RecipeDef>.AllDefsListForReading.Where(r =>
                !r.AnyResearchPrerequisites())
                )
            {
                if (recipeDef.IsSurgery)
                {
                    if(recipeDef.IsDefinitelyASurgery())
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
                        indeterminateSurgeries.Add(recipeDef);
                    }
                }
                else
                {
                    noProjectRecipeDefs.Add(recipeDef);
                }
            }

            if (indeterminateSurgeries.Any()) 
            {
                Log.Warning($"RR.SS: Detected recipe(s) that both are and arent surgeries: {string.Join(",", indeterminateSurgeries)} - this is usually the result of invalid animal prosthetic patches.");
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

            if(ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode != SurgeryPreregsMode.Off)
			{
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
			}

            /*foreach (var recipe in noProjectFullBodySurgeryRecipeDefs)
            {
                try
                {
                    GivePrerequisitesToFullBodySurgery(recipe);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }*/

            bodyPartRecordCache.Clear();
            bodyPartRecordCache = null;
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

			if (recipe.AnyResearchPrerequisites())
				return;

			var firstProjects = recipe.FindEarliestPrerequisiteProjects();
            firstProjects = FilterOutSuperEarlyTechs(firstProjects);
            if (firstProjects != null && firstProjects.Any())
			{
				recipe.researchPrerequisites.AddRange(firstProjects);
            }

            if(recipe.workerClass.IsAssignableFrom(typeof(Recipe_InstallNaturalBodyPart)))
			{
				if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode == SurgeryPreregsMode.Easy)
				{
					recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.DrugProduction);
				}
				else
				{
					recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.PenoxycylineProduction);
				}
			}
            else if (recipe.workerClass.IsAssignableFrom(typeof(Recipe_InstallArtificialBodyPart)))
			{
                var props = recipe.addsHediff?.addedPartProps;
                if (props == null)
                {
                    Log.Warning($"RR.SS: recipe {recipe} has Recipe_InstallArtificialBodyPart as worker but does not define addedPartProps, skipping.");
                    return;
                }

                bool isExternal = recipe.appliedOnFixedBodyParts.Any(bp => GetRecordsOfBodyPartDef(bp).Any(r => r.depth == BodyPartDepth.Outside));

                if (isExternal)
				{
					if (props.betterThanNatural || props.partEfficiency >= 1)
					{
						if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode == SurgeryPreregsMode.Easy)
						{
							recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.ComplexClothing);
							recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicHerbLore);
						}
						else
						{
							recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.Prosthetics);
							recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.PenoxycylineProduction);
						}
					}
					else
					{
						if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode == SurgeryPreregsMode.Easy)
						{
							//recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicApparel);
						}
                        else
						{
							recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicApparel);
						}
					}
				}
                else
				{
                    if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode == SurgeryPreregsMode.Easy)
                    {
                        recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.DrugProduction);
                    }
                    else
                    {
						recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.PenoxycylineProduction);
					}
				}
			}
            else if (recipe.workerClass.IsAssignableFrom(typeof(Recipe_InstallImplant)))
			{
				if (ResearchReinvented_SteppingStonesMod.Settings.surgeryPreregsMode == SurgeryPreregsMode.Easy)
				{
					recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.DrugProduction);
				}
				else
				{
					recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.PenoxycylineProduction);
				}
			}
        }
        

        /*private static void GivePrerequisitesToFullBodySurgery(RecipeDef recipe) 
        {
            if (recipe.researchPrerequisites == null)
                recipe.researchPrerequisites = new List<ResearchProjectDef>();

            if (recipe.AnyResearchPrerequisites())
                return;

			Log.Message("checking full body no prereg recipe: " + recipe);

			var firstProjects = recipe.FindEarliestPrerequisiteProjects();
            firstProjects = FilterOutSuperEarlyTechs(firstProjects);
			if (firstProjects != null && firstProjects.Any())
			{
				Log.Message($"recipe {recipe}, adding preregs: {String.Join(", ", firstProjects)}");
				recipe.researchPrerequisites.AddRange(firstProjects);
            }
			//recipe.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalMedicine);
		}*/

		private static List<BodyPartRecord> GetRecordsOfBodyPartDef(BodyPartDef def)
		{
			if (!bodyPartRecordCache.ContainsKey(def))
			{
				List<BodyPartRecord> parts = new List<BodyPartRecord>();
				foreach (var body in DefDatabase<BodyDef>.AllDefsListForReading)
				{
					var matches = body.GetPartsWithDef(def);
					parts.AddRange(matches);
				}
				bodyPartRecordCache[def] = parts;
			}
			return bodyPartRecordCache[def];
		}
	}
}
