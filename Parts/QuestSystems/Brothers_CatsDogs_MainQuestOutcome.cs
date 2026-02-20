using System;
using XRL.World;
using XRL.UI;

namespace XRL.World.ZoneParts
{
    [Serializable]
    public class Brothers_CatsDogs_MainQuestOutcome : IZonePart
    {
        public long startTick;

        public long ticksToApply = 50;

        public bool outcomeApplied;

        public override bool SameAs(IZonePart p) => false;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == ObjectCreatedEvent.ID
                || ID == ZoneActivatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            if (The.Game != null)
                this.startTick = The.Game.TimeTicks;
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            if (!this.outcomeApplied && The.Game.TimeTicks - this.startTick >= this.ticksToApply)
            {
                this.outcomeApplied = true;               
                this.ApplyOutcome();
            }
            return base.HandleEvent(E);
        }

        public override void AddedAfterCreation()
        {
            base.AddedAfterCreation();
            if (The.Game != null)
                this.startTick = The.Game.TimeTicks;
        }

        public void ApplyOutcome()
        {
            Popup.Show("Time has passed and the quest outcome has been applied!");
        }
    }
}