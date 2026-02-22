using System;
using XRL.World;
using XRL.UI;
using XRL.World.Parts;

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

        public void DestroyPeopleFromFaction(string faction)
        {
            Zone activeZone = The.ZoneManager.ActiveZone;

            foreach (GameObject gameObject in activeZone.GetObjects(
                o => o.HasPart<Brain>() &&
                o.GetPart<Brain>().GetPrimaryFaction() == faction
            ))
            {
                gameObject.Destroy();
            }
        }
        public void ApplyOutcome()
        {
            // Neutral ending
            if(The.Game.GetBooleanGameState("Brothers_CatsDogs_NeutralEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_NeutralEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_NeutralEnding_Occured", true);
                    Popup.Show("Neutral Ending");
                }
            }
            
            // Shik ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_ShikEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_ShikEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_ShikEnding_Occured", true);
                    Popup.Show("Shik Ending");
                }
                this.DestroyPeopleFromFaction("Spar");
            }
            
            // Spar ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_SparEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_SparEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_SparEnding_Occured", true);
                    Popup.Show("Spar Ending");
                }
                
                this.DestroyPeopleFromFaction("Shik");
            }
            
            // Perfect ending
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_PerfectEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_PerfectEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_PerfectEnding_Occured", true);
                    Popup.Show("Perfect Ending");
                }
            }
            
            // Debug
            else
            {
                Popup.Show("No ending");
            }
        }
    }
}