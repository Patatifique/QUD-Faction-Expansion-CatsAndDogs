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
        public string TargetZone;
        public int TargetX;
        public int TargetY;
        public bool removeOnArrival = false;

        public string setStateOnArrival = null;
        public string stateValueOnArrival = "1";

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == PooledEvent<AIBoredEvent>.ID
                || ID == GetZoneSuspendabilityEvent.ID;
        }

        public override bool HandleEvent(AIBoredEvent E)
        {
            if (!string.IsNullOrEmpty(TargetZone))
            {
                ParentObject.SetIntProperty("AllowGlobalTraversal", 1);
                ParentObject.Brain.MoveToGlobal(TargetZone, TargetX, TargetY);
            }


                var cell = ParentObject.Physics.CurrentCell;
                if (cell != null
                    && cell.ParentZone != null
                    && cell.ParentZone.ZoneID == TargetZone
                    && cell.X == TargetX
                    && cell.Y == TargetY)
                {
                    if (removeOnArrival)
                    {
                        //Popup.Show("Arrived at destination.");
                        ParentObject.SetIntProperty("AllowGlobalTraversal", 0);
                        ParentObject.RemovePart(this);
                    }
                    if (!string.IsNullOrEmpty(setStateOnArrival))
                    {
                        if (The.Game.HasStringGameState(setStateOnArrival) && The.Game.GetStringGameState(setStateOnArrival) == stateValueOnArrival)
                        {
                            return false;
                        }
                        //Popup.Show("State has been set");
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