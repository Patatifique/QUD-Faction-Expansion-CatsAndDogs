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
        private long startTick;
        private bool failSafeTickStarted = false;
        private bool failsafeTriggered = false;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == PooledEvent<AIBoredEvent>.ID
                || ID == GetZoneSuspendabilityEvent.ID;
        }

        public override bool HandleEvent(AIBoredEvent E)
        {
            // Attempt global movement toward the target
            if (!string.IsNullOrEmpty(TargetZone))
            {
                ParentObject.SetIntProperty("AllowGlobalTraversal", 1);
                ParentObject.Brain.MoveToGlobal(TargetZone, TargetX, TargetY);
            }

            // Start failsafe timer
            if (The.Game != null && !failSafeTickStarted)
            {
                startTick = The.Game.TimeTicks;
                failSafeTickStarted = true;
            }

            // Force placement if movement takes too long
            if (failSafeTicks > 0
                && The.Game.TimeTicks - this.startTick >= this.failSafeTicks
                && !failsafeTriggered)
                {
                    failsafeTriggered = true;

                    Zone zone = The.ZoneManager.GetZone(TargetZone);

                    if (zone != null)
                    {
                        var currentCell = ParentObject.Physics?.CurrentCell;
                        if (currentCell != null)
                        {
                            currentCell.RemoveObject(ParentObject);
                        }

                        zone.GetCell(TargetX, TargetY).AddObject(ParentObject);
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
                if (removeOnArrival)
                {
                    ParentObject.SetIntProperty("AllowGlobalTraversal", 0);
                    ParentObject.RemovePart(this);
                }

                // Optionally update a game state when arriving
                if (!string.IsNullOrEmpty(setStateOnArrival))
                {
                    if (The.Game.HasStringGameState(setStateOnArrival)
                        && The.Game.GetStringGameState(setStateOnArrival) == stateValueOnArrival)
                    {
                        return false;
                    }

                    The.Game.SetStringGameState(setStateOnArrival, stateValueOnArrival);
                }
            }

            return false;
        }

        public override bool HandleEvent(GetZoneSuspendabilityEvent E)
        {
            E.Suspendability = Suspendability.Pinned;
            return false;
        }
    }
}