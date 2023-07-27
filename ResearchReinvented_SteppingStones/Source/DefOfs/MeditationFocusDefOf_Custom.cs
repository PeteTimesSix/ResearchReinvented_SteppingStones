using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs
{
    [DefOf]
    public static class MeditationFocusDefOf_Custom
    {
        public static MeditationFocusDef Flame;

        static MeditationFocusDefOf_Custom()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf_Custom));
        }
    }
}
