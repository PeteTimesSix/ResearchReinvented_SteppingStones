using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Utilities
{
    [StaticConstructorOnStartup]
    public static class TechLevelColorUtil
    {
        //public static Dictionary<TechLevel, Color> ColorCompleted = new Dictionary<TechLevel, Color>();
        public static Dictionary<TechLevel, Color> ColorAvailable = new Dictionary<TechLevel, Color>();
        //public static Dictionary<TechLevel, Color> ColorUnavailable = new Dictionary<TechLevel, Color>();
        //public static Color TechLevelColor = new Color(1f, 1f, 1f, .2f);


        private static List<TechLevel> _relevantTechLevels;
        public static List<TechLevel> RelevantTechLevels
        {
            get
            {
                if (_relevantTechLevels == null)
                    _relevantTechLevels = Enum.GetValues(typeof(TechLevel))
                                              .Cast<TechLevel>()
                                              // filter down to relevant tech levels only.
                                              .Where(
                                                   tl => DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any(
                                                       rp => rp.techLevel ==
                                                             tl))
                                              .ToList();
                return _relevantTechLevels;
            }
        }

        public static Color WithAlpha(this Color color, float a) 
        {
            return new Color(color.r, color.g, color.b, a);
        }

        static TechLevelColorUtil()
        {
            var techlevels = RelevantTechLevels;
            var n = techlevels.Count;
            for (var i = 0; i < n; i++)
            {
                //ColorCompleted[techlevels[i]] = Color.HSVToRGB(1f / n * i, .75f, .75f);
                ColorAvailable[techlevels[i]] = Color.HSVToRGB(1f / n * i, .33f, .33f).WithAlpha(0.65f);
                //ColorUnavailable[techlevels[i]] = Color.HSVToRGB(1f / n * i, .125f, .33f);
            }
        }
    }
}
