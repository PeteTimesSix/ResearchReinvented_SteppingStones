using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Patches
{
    [HarmonyPatch(typeof(Zone_Growing), nameof(Zone_Growing.PlantDefToGrow), MethodType.Getter)]
    public static class Zone_Growing_Patches
    {
        //state is plant before getter

        [HarmonyPrefix]
        public static void Get_PlantDefToGrow_Prefix(Zone_Growing __instance, ThingDef ___plantDefToGrow, out ThingDef __state)
        {
            __state = ___plantDefToGrow;
        }

        [HarmonyPostfix]
        public static void Get_PlantDefToGrow_Postfix(Zone_Growing __instance, ref ThingDef __result, ref ThingDef ___plantDefToGrow, ThingDef __state)
        {
            if (Current.ProgramState != ProgramState.Playing) //only modify during game
                return;

            if(__state == null && !___plantDefToGrow.plant.UnlockedForSowing()) 
            {
                ThingDef growableHere = null;
                foreach (ThingDef thingDef in PlantUtility.ValidPlantTypesForGrowers(new List<IPlantToGrowSettable>() { __instance }))
                {
                    if (Command_SetPlantToGrow.IsPlantAvailable(thingDef, __instance.Map))
                    {
                        growableHere = thingDef;
                        break;
                    }
                }
                if(growableHere == null)
                    growableHere = ThingDefOf.Plant_Grass;

                ___plantDefToGrow = growableHere;
                __result = ___plantDefToGrow;
            }
        }

        private static bool UnlockedForSowing(this PlantProperties plant)
        {
            if (plant.sowResearchPrerequisites == null)
                return true;

            return plant.sowResearchPrerequisites.Any(r => !r.IsFinished) == false;
        }
    }
}
