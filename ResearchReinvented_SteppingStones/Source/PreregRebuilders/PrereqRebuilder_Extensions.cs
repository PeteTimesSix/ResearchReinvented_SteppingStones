using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
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

        private static bool IsCraftingFacility(this ThingDef buildable)
        {
            return (buildable.recipes != null && buildable.recipes.Any()) ||
                (buildable.inspectorTabs != null && buildable.inspectorTabs.Contains(typeof(ITab_Bills))) ||
                buildable.thingClass == typeof(Building_ResearchBench);
        }

        private static bool IsFireBased(this ThingDef buildable)
        {
            return  (buildable.HasComp(typeof(CompFireOverlay)) || buildable.HasComp(typeof(CompDarklightOverlay))) ||
                    (buildable.HasComp(typeof(CompMeditationFocus)) && buildable.GetCompProperties<CompProperties_MeditationFocus>().focusTypes.Contains(MeditationFocusDefOf_Custom.Flame));
        }

        private static bool IsFurniture(this ThingDef buildable)
        {
            return buildable.IsBed || buildable.IsTable || buildable.building != null && buildable.building.isSittable;
        }

        private static bool IsStructure(this ThingDef buildable)
        {
            return buildable.IsFence || buildable.IsDoor || buildable == ThingDefOf.Column || buildable.IsWall();
        }

        private static bool IsWall(this ThingDef buildable)
        {
            return buildable == ThingDefOf.Wall ||
                (
                    (buildable.graphicData?.linkFlags.HasFlag(LinkFlags.Wall) ?? false) &&
                    buildable.graphicData.linkType == LinkDrawerType.CornerFiller &&
                    (buildable.building?.isInert ?? false) &&
                    buildable.building.isPlaceOverableWall == true &&
                    buildable.fillPercent == 1 &&
                    buildable.passability == Traversability.Impassable &&
                    buildable.size == IntVec2.One
                );
        }

        private static bool IsElectrical(this ThingDef buildable)
        {
            return  buildable.HasComp(typeof(CompPower)) || 
                    buildable.HasComp(typeof(CompPowerTrader)) || 
                    buildable.HasComp(typeof(CompPowerBattery)) || 
                    buildable.HasComp(typeof(CompPowerTransmitter)) || 
                    buildable.HasComp(typeof(CompPowerPlant));
        }

    }
}
