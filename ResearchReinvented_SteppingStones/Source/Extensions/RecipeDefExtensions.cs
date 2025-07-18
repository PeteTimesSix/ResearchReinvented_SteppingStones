using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Extensions
{
    public static class RecipeDefExtensions
	{
		public static bool IsDefinitelyASurgery(this RecipeDef recipeDef) 
		{
			bool isSurgery = false;
			foreach (var recipeUser in recipeDef.AllRecipeUsers)
			{
				if (recipeUser.category == ThingCategory.Pawn)
				{
					isSurgery = true;
				}
				else 
				{
					return false;
				}
			}
			return isSurgery;
		}

		public static bool AnyResearchPrerequisites(this RecipeDef recipe)
		{
			HashSet<ResearchProjectDef> prerequisites = new HashSet<ResearchProjectDef>();
			if (recipe.researchPrerequisite != null)
				return true;
			if (recipe.researchPrerequisites != null)
				return recipe.researchPrerequisites.Any();
			return false;
		}

		public static HashSet<ResearchProjectDef> FindEarliestPrerequisiteProjects(this RecipeDef recipe) 
		{
			var allPrereqOptions = new List<HashSet<ResearchProjectDef>>();
			foreach(var user in recipe.AllRecipeUsers) 
			{
				var prereqs = user.researchPrerequisites;
				if (prereqs != null)
					allPrereqOptions.Add(prereqs.ToHashSet());
			}

			if (!allPrereqOptions.Any())
				return null;
			if (allPrereqOptions.Count == 1)
				return allPrereqOptions.First();

			var allPrereqOptionsSquashed = allPrereqOptions.SelectMany(l => l).ToHashSet();
			Dictionary<ResearchProjectDef, ResearchProjectDef> ancestorMapping = new Dictionary<ResearchProjectDef, ResearchProjectDef>();

			foreach(var userProjects in allPrereqOptions) 
			{
				foreach (var project in userProjects)
                {
                    if (ancestorMapping.ContainsKey(project))
						continue;

					var projectCrawler = project;
					try
                    {
                        var ancestor = RecursiveAncestorFind(project, allPrereqOptionsSquashed.Except(project), 0);
                        ancestorMapping[project] = ancestor;
                    }
					catch (RecursiveAncestorLoopException ex)
                    {
                        ex.list.Reverse();
                        Log.Error($"RR:SS: Reached a depth of 500 while recursively crawling the tech tree! This almost certainly means there's a loop in it. Looped projects: " + string.Join(" -> ", ex.list.Select(def => $"{def.label} ({def.defName})")));
                        continue;
                    }
				}
			}

			var projectsWithoutAncestors = ancestorMapping.Where(kv => kv.Value == null).ToList();
			if (projectsWithoutAncestors.Count == 1)
			{
				return new HashSet<ResearchProjectDef>() { projectsWithoutAncestors.First().Key };
			}

			//Log.Message($"recipe {recipe} has multiple dead-end ancestors: {string.Join(",", projectsWithoutAncestors.Select(kv => $"{kv.Key}:{kv.Value}"))}");
			return null;
		}

        private class RecursiveAncestorLoopException : InvalidOperationException
        {
            public List<ResearchProjectDef> list = new();

            public RecursiveAncestorLoopException() { }
            public RecursiveAncestorLoopException(ResearchProjectDef project)
            {
                list.Add(project);
            }
        }

        private static ResearchProjectDef RecursiveAncestorFind(ResearchProjectDef project, IEnumerable<ResearchProjectDef> possibleAncestors, int depth)
        {
			if(possibleAncestors.Contains(project))
				return project;

			if (depth > 500)
				throw new RecursiveAncestorLoopException(project);

			try
            {
                foreach (var ancestorNode in project.AllPrerequisiteProjects())
                {
					if (ancestorNode == project)
						continue;

                    var returnedAncestor = RecursiveAncestorFind(ancestorNode, possibleAncestors, depth + 1);
                    if (returnedAncestor != null)
                        return returnedAncestor;
                }
            }
            catch (RecursiveAncestorLoopException ex)
            {
                ex.list.Add(project);
                throw;
            }

            return null;
        }
    }
}
