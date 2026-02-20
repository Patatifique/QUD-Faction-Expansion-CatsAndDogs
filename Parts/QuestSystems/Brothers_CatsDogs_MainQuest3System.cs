using System;
using XRL.World;
using XRL.World.Quests;
using XRL.World.Parts;
using XRL.UI;

namespace XRL.World.Quests
{
    [Serializable]
    public class Brothers_CatsDogs_MainQuest3System : IQuestSystem
    {

        static string meetingZone = "JoppaWorld.38.23.1.0.10";
        
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(AfterConversationEvent.ID);
        }

        public override bool HandleEvent(AfterConversationEvent E)
        {
            // Move the dog mayor to the meeting
            if (The.Game.HasFinishedQuestStep("Brothers_CatsDogs_MainQuest3", "DogMayor"))
            {
                GameObject dogmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Brothers_CatsDogs_DogMayor");
                if (dogmayor != null)
                {
                    var move = dogmayor.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = meetingZone;
                    move.TargetX = 33;
                    move.TargetY = 12;
                    move.setStateOnArrival = "Brothers_CatsDogs_DogMayorMoved";
                }
            }

             // Move the cat mayor to the meeting
             if (The.Game.HasFinishedQuestStep("Brothers_CatsDogs_MainQuest3", "CatMayor"))
             {
                GameObject catmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Brothers_CatsDogs_CatMayor");
                if (catmayor != null)
                {
                    var move = catmayor.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = meetingZone;
                    move.TargetX = 35;
                    move.TargetY = 12;
                    move.setStateOnArrival = "Brothers_CatsDogs_CatMayorMoved";
                }
            }

            return base.HandleEvent(E);
        }

        public override void Finish()
        {
            // Liberate the mayors from the meeting
            GameObject dogmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Brothers_CatsDogs_DogMayor");
            if (dogmayor != null)
            {
                dogmayor.RemovePart<Brothers_GlobalMove>();
            }

            GameObject catmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Brothers_CatsDogs_CatMayor");
            if (catmayor != null)
            {
                catmayor.RemovePart<Brothers_GlobalMove>();
            }
        }
    }
}