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
            // Neutral ending
            if(The.Game.GetBooleanGameState("Brothers_CatsDogs_NeutralEnding"))
            {
                Popup.Show("Neutral Ending");
            }
            
            // Shik ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_ShikEnding"))
            {
                Popup.Show("Shik Ending");
            }
            
            // Spar ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_SparEnding"))
            {
                Popup.Show("Spar Ending");
            }
            
            // Perfect ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_PerfectEnding"))
            {
                Popup.Show("Perfect Ending");
            }
            
            // Debug
            else
            {
                Popup.Show("No ending");
            }
        }
    }
}