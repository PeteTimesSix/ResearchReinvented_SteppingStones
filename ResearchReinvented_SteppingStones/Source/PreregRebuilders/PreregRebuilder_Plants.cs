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
        private static void DoPlants()
        {
            var noProjectPlantDefs = new HashSet<ThingDef>();
            foreach (var plantDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(t =>
                t.plant != null &&
                t.plant.Sowable &&
                (t.plant.sowResearchPrerequisites == null || !t.plant.sowResearchPrerequisites.Any()))
                )
            {
                noProjectPlantDefs.Add(plantDef);
            }

            foreach (var plant in noProjectPlantDefs)
            {
                try
                {
                    GivePrerequisitesToPlant(plant);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }
        }

        private static void GivePrerequisitesToPlant(ThingDef plant)
        {
            if (plant.plant.sowResearchPrerequisites == null)
                plant.plant.sowResearchPrerequisites = new List<ResearchProjectDef>();

            plant.plant.sowResearchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_Agriculture);
        }
    }
}
