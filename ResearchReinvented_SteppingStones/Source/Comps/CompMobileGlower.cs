using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace PeteTimesSix.ResearchReinvented_SteppingStones.Comps
{
    public class CompProperties_CarriedGlower: CompProperties
    {
        public CompProperties_Glower glowerProps;

        public CompProperties_CarriedGlower()
        {
            compClass = typeof(CompCarriedGlower);
        }
    }

    public class CompCarriedGlower : ThingComp
    {
        public CompProperties_CarriedGlower Props => (CompProperties_CarriedGlower)props;

        Pawn Wearer => (parent.ParentHolder as Pawn_EquipmentTracker)?.pawn;

        public IntVec3 glowerPosition;
        public CompGlower glower;
        public Map glowerMap;

        public override void PostDraw()
        {
            base.PostDraw();
            Log.Message("post draw");
            if (Wearer != null && Wearer.Spawned && PawnRenderUtility.CarryWeaponOpenly(Wearer))
            {
                Log.Message("post draw inner");
                CompFireOverlay.FireGraphic.Draw(glowerPosition.ToVector3(), Wearer.Rotation, Wearer);
            }
        }

        public override void CompTickInterval(int delta)
        {
            if (glower != null && glowerMap != null && (Wearer == null || !Wearer.Spawned || !PawnRenderUtility.CarryWeaponOpenly(Wearer)))
            {
                RemoveGlower();
                return;
            }

            if (Wearer != null && Wearer.Spawned && PawnRenderUtility.CarryWeaponOpenly(Wearer))
            {
                if (glower == null)
                {
                    AddGlower();
                }
                else if (Wearer.Map != glowerMap || Wearer.Position != glowerPosition)
                {
                    UpdateGlower();
                }
                //CompFireOverlay.FireGraphic.Draw(glowerPosition.ToVector3(), Wearer.Rotation, Wearer);
            }
        }

        private void UpdateGlower()
        {
            Log.Message($"Updating glower on {Wearer?.LabelCap ?? "NULL"}");
            //RemoveGlower();
            //AddGlower();
            glowerMap.glowGrid.DeRegisterGlower(glower);
            glowerMap.mapDrawer.MapMeshDirty(glowerPosition, MapMeshFlagDefOf.Things);
            glowerPosition = Wearer.Position;
            glowerMap.glowGrid.RegisterGlower(glower);
            glowerMap.mapDrawer.MapMeshDirty(glowerPosition, MapMeshFlagDefOf.Things);
        }

        private void AddGlower()
        {
            glower = new CompGlower();
            glower.parent = Wearer;
            glower.Initialize(Props.glowerProps);
            glowerPosition = Wearer.Position;
            glowerMap = Wearer.Map;
            glowerMap.glowGrid.RegisterGlower(glower);
            glowerMap.mapDrawer.MapMeshDirty(glowerPosition, MapMeshFlagDefOf.Things);
        }

        private void RemoveGlower()
        {
            glowerMap.glowGrid.DeRegisterGlower(glower);
            glowerMap.mapDrawer.MapMeshDirty(glowerPosition, MapMeshFlagDefOf.Things);
            glower = null;
            glowerMap = null;
        }
    }
}
