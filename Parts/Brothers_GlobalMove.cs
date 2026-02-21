using System;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;
using XRL.UI;

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_GlobalMove : IPart
    {
        // Destination (zone + cell)
        public string TargetZone;
        public int TargetX;
        public int TargetY;

        // Optional behavior on arrival
        public bool removeOnArrival = false;
        public string setStateOnArrival = null;
        public string stateValueOnArrival = "1";

        // Optional failsafe timer to force arrival
        public long failSafeTicks = 0;
        public bool removeAfterFailsafe = false;
        private long startTick;
        private bool failSafeTickStarted = false;
        private bool failSafeTriggeredBuffer = false;
        private bool failsafeTriggered = false;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == PooledEvent<AIBoredEvent>.ID
                || ID == EndTurnEvent.ID
                || ID == GetZoneSuspendabilityEvent.ID;
        }

        // Only responsible for initiating global movement
        public override bool HandleEvent(AIBoredEvent E)
        {
            if (!string.IsNullOrEmpty(TargetZone))
            {
                ParentObject.SetIntProperty("AllowGlobalTraversal", 1);
                ParentObject.Brain.MoveToGlobal(TargetZone, TargetX, TargetY);
            }

            return false;
        }


        // Handles ticking, failsafe logic, and arrival detection
        public override bool HandleEvent(EndTurnEvent E)
        {
            if (The.Game == null || string.IsNullOrEmpty(TargetZone))
                return base.HandleEvent(E);

            // Start failsafe timer
            if (!failSafeTickStarted)
            {
                startTick = The.Game.TimeTicks;
                failSafeTickStarted = true;
            }

            // Force placement if movement takes too long
            if (failSafeTicks > 0
                && The.Game.TimeTicks - startTick >= failSafeTicks
                && !failsafeTriggered)
            {
                failSafeTriggeredBuffer = true;

                Zone zone = The.ZoneManager.GetZone(TargetZone);

                if (zone != null)
                {
                    var currentCell = ParentObject.Physics?.CurrentCell;
                    if (currentCell != null)
                    {
                        currentCell.RemoveObject(ParentObject);
                    }

                    zone.GetCell(TargetX, TargetY)?.AddObject(ParentObject);
                }
            }


            var cell = ParentObject.Physics?.CurrentCell;

            // Check arrival
            if (cell != null
                && cell.ParentZone != null
                && cell.ParentZone.ZoneID == TargetZone
                && cell.X == TargetX
                && cell.Y == TargetY)
            {
                ParentObject.SetIntProperty("AllowGlobalTraversal", 0);

                if (removeOnArrival)
                {
                    ParentObject.RemovePart(this);
                }

                if (removeAfterFailsafe && failsafeTriggered)
                {
                    ParentObject.RemovePart(this);
                }

                if (failSafeTriggeredBuffer)
                {
                    failsafeTriggered = true;
                    failSafeTriggeredBuffer = false;
                }

                if (!string.IsNullOrEmpty(setStateOnArrival))
                {
                    if (!The.Game.HasStringGameState(setStateOnArrival) ||
                        The.Game.GetStringGameState(setStateOnArrival) != stateValueOnArrival)
                    {
                        The.Game.SetStringGameState(setStateOnArrival, stateValueOnArrival);
                    }
                }
            }

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetZoneSuspendabilityEvent E)
        {
            E.Suspendability = Suspendability.Pinned;
            return false;
        }
    }
}