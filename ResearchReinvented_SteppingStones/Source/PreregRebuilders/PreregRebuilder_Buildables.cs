﻿using PeteTimesSix.ResearchReinvented_SteppingStones.DefOfs;
using PeteTimesSix.ResearchReinvented_SteppingStones.Extensions;
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
        public static void DoBuildables()
        {
            var noProjectBuildableDefs = new HashSet<ThingDef>();
            var noProjectInstantBuildableDefs = new HashSet<ThingDef>();
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListForReading.Where(t =>
                t.BuildableByPlayer &&
                (t.researchPrerequisites == null || !t.researchPrerequisites.Any()))
                )
            {
                if (thingDef.IsInstantBuild())
                {
                    noProjectInstantBuildableDefs.Add(thingDef);
                }
                else
                {
                    noProjectBuildableDefs.Add(thingDef);
                }
            }

            foreach (var buildable in noProjectBuildableDefs)
            {
                try
                {
                    GivePrerequisitesToBuildables(buildable);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }

            foreach (var buildable in noProjectInstantBuildableDefs)
            {
                try
                {
                    GivePrerequisitesToInstantBuildable(buildable);
                }
                catch (Exception e)
                {
                    Log.Warning($"RR.SS: Error during research project assingment: {e}");
                }
            }
        }

        private static void GivePrerequisitesToBuildables(ThingDef buildable)
        {
            if (buildable.researchPrerequisites == null)
                buildable.researchPrerequisites = new List<ResearchProjectDef>();

            if (buildable.defName == "ResearchSampleBench") //avoid hard lockout with Research Data
                return;

            if (ResearchReinvented_SteppingStonesMod.Settings.doPowerPreregs && buildable.IsElectrical())
            {
                buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_ElectricityBasics);
            }

            if (buildable.researchPrerequisites.Count == 0) //no specifics, so go more basic
            {
                if (buildable.IsCraftingFacility())
                {
                    buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_BasicCraftingFacilities);
                }
                if (buildable?.building.isTrap ?? false)
                {
                    if (ResearchProjectDefOf_Manual.Trapping != null)
                        buildable.researchPrerequisites.Add(ResearchProjectDefOf_Manual.Trapping);
                }
                if (buildable.IsFireBased())
                {
                    if (buildable.defName.ToLowerInvariant().Contains("campfire"))
                        buildable.researchPrerequisites.Clear(); //hack to make campfires only require fire
                    if (ResearchProjectDefOf_Manual.Fire != null)
                        buildable.researchPrerequisites.Add(ResearchProjectDefOf_Manual.Fire);
                }
            }

            if (buildable.researchPrerequisites.Count == 0) //no specifics, so go more basic
            {
                if (buildable.IsFurniture())
                {
                    if (ResearchProjectDefOf_Manual.BasicFurniture != null)
                        buildable.researchPrerequisites.Add(ResearchProjectDefOf_Manual.BasicFurniture);
                }
                else if (buildable.IsStructure())
                {
                    if (ResearchProjectDefOf_Manual.BasicStructures != null)
                        buildable.researchPrerequisites.Add(ResearchProjectDefOf_Manual.BasicStructures);
                }
                else
                {
                    //buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_FundamentalConstruction);
                }
            }
        }

        private static void GivePrerequisitesToInstantBuildable(ThingDef buildable)
        {
            if (buildable.researchPrerequisites == null)
                buildable.researchPrerequisites = new List<ResearchProjectDef>();

            buildable.researchPrerequisites.Add(ResearchProjectDefOf_Custom.RR_Organization);
        }

    }
}
