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
    public static class ThingDefOf_Custom
    {
        public static ThingDef RR_ThinkingSpot;

        public static ThingDef Plant_Haygrass;

        static ThingDefOf_Custom()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf_Custom));
        }
    }
}
