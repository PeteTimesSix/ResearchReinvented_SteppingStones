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

        private static bool IsFurniture(this ThingDef buildable)
        {
            return buildable.IsBed || buildable.IsTable || buildable.building != null && buildable.building.isSittable;
        }

        private static bool IsStructure(this ThingDef buildable)
        {
            return buildable == ThingDefOf.Wall || buildable.IsFence || buildable.IsDoor || buildable == ThingDefOf.Column;
        }

        private static bool IsElectrical(this ThingDef buildable)
        {
            return buildable.HasComp(typeof(CompPower)) || buildable.HasComp(typeof(CompPowerTrader)) || buildable.HasComp(typeof(CompPowerBattery)) || buildable.HasComp(typeof(CompPowerTransmitter)) || buildable.HasComp(typeof(CompPowerPlant));
        }

    }
}
