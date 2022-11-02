using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Extensions
{
    public static class ResearchProjectDefExtensions
    {
        public static IEnumerable<ResearchProjectDef> AllPrerequisiteProjects(this ResearchProjectDef project)
        {
            List<ResearchProjectDef> allPrerequisites = new List<ResearchProjectDef>();
            if(project.prerequisites != null)
                allPrerequisites.AddRange(project.prerequisites);
            if (project.hiddenPrerequisites != null)
                allPrerequisites.AddRange(project.hiddenPrerequisites);
            return allPrerequisites;
        }
    }
}
