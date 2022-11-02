using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Extensions
{
    public static class ThingDefExtensions
    {
        public static bool IsInstantBuild(this ThingDef x)
        {
            if (x.GetStatValueAbstract(StatDefOf.WorkToBuild) == 0f)
                return true;
            return false;
        }
    }
}
