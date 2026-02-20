using System;
using System.Linq;
using Qud.API;
using XRL.World;
using XRL.World.Quests;

namespace XRL.World.Quests
{
    [Serializable]
    public class Brothers_CatsDogs_MainQuest2System : IQuestSystem
    {
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(SecretVisibilityChangedEvent.ID);
        }

        private int GetKnownGodClueCount()
        {
            return JournalAPI.Observations.Count(
                o => o.Revealed
                && o.Has("brothers_godclue")
            );
        }

        private void CheckQuestProgress()
        {
            int godClueCount = GetKnownGodClueCount();

            if (godClueCount > 0)
            {
                The.Game.FinishQuestStep(Quest, "Clue");
            }
            
            if (godClueCount > 1)
            {
                The.Game.FinishQuestStep(Quest, "Investigate");
            }  
        }

        public override bool HandleEvent(SecretVisibilityChangedEvent E)
        {
            CheckQuestProgress();
            return base.HandleEvent(E);
        }

        public override void Start()
        {
            CheckQuestProgress();
        }
    }
}
