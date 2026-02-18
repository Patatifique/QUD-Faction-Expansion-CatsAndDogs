using System;
using System.Linq;
using Qud.API;
using XRL.World;
using XRL.World.Quests;

namespace XRL.World.Quests
{
    [Serializable]
    public class Brothers_CatsDogs_MainQuest1System : IQuestSystem
    {
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(SecretVisibilityChangedEvent.ID);
            Registrar.Register(EndTurnEvent.ID);
        }

        private int GetKnownShiksparNotes()
        {
            return JournalAPI.VillageNotes.Count(
                o => o.Revealed
                     && o.VillageID == "Shikspar"
            );
        }

        private void CheckQuestProgress()
        {
            int shiksparNoteCount = GetKnownShiksparNotes();
            if (shiksparNoteCount > 2)
            {
                The.Game.FinishQuestStep(Quest, "Find");
            }
        }

        public override bool HandleEvent(SecretVisibilityChangedEvent E)
        {
            CheckQuestProgress();
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            CheckQuestProgress();
            return base.HandleEvent(E);
        }
    }
}
