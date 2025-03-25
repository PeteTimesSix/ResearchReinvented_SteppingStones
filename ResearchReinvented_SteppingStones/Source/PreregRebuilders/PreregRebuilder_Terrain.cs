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
        public static void DoTerrains()
        {
            var noProjectTerrainDefs = new HashSet<TerrainDef>();
            foreach (var terrainDef in DefDatabase<TerrainDef>.AllDefsListForReading.Where(t =>
                t.BuildableByPlayer &&
                (t.researchPrerequisites == null || !t.researchPrerequisites.Any()))
                )
            {
                noProjectTerrainDefs.Add(terrainDef);
            }
            foreach (var terrain in noProjectTerrainDefs)
            {
                try
                {
                    GivePrerequisitesToTerrain(terrain);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }
        }

        public static void GivePrerequisitesToTerrain(TerrainDef terrain)
        {
            if (terrain.researchPrerequisites == null)
                terrain.researchPrerequisites = new List<ResearchProjectDef>();

            if (terrain.costList == null || !terrain.costList.Any())
            {
                if (ResearchProjectDefOf_Manual.Agriculture != null)
                    terrain.researchPrerequisites.Add(ResearchProjectDefOf_Manual.Agriculture);
            }
            else if(terrain.bridge)
            {
                if (ResearchProjectDefOf_Manual.BasicStructures != null)
                    terrain.researchPrerequisites.Add(ResearchProjectDefOf_Manual.BasicStructures);
            }
        }
    }
}
