using System;
using System.Collections.Generic;
using XRL.World;
using XRL.World.Quests;
using XRL.World.Parts;
using XRL.UI;
using XRL.World.ZoneParts;
using System.Globalization;

namespace XRL.World.Quests
{
    [Serializable]
    public class Sibs_CatsDogs_MainQuest3System : IQuestSystem
    {
        // Zones dictionary
        static Dictionary<string, string> Zones = new Dictionary<string, string>()
        {
            { "NorthWest", "JoppaWorld.38.23.0.0.10" },
            { "West", "JoppaWorld.38.23.0.1.10" },
            { "SouthWest", "JoppaWorld.38.23.0.2.10" },

            { "North", "JoppaWorld.38.23.1.0.10" },
            { "Center", "JoppaWorld.38.23.1.1.10" },
            { "South", "JoppaWorld.38.23.1.2.10" },

            { "NorthEast", "JoppaWorld.38.23.2.0.10" },
            { "East", "JoppaWorld.38.23.2.1.10" },
            { "SouthEast", "JoppaWorld.38.23.2.2.10" }
        };

        // Prevent giving move part multiple times
        private bool dogMayorMoveGiven = false;
        private bool catMayorMoveGiven = false;
        
        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Registrar.Register(AfterConversationEvent.ID);
        }

        public override bool HandleEvent(AfterConversationEvent E)
        {
            // Move the dog mayor to the meeting
            if (!dogMayorMoveGiven && The.Game.HasFinishedQuestStep("Sibs_CatsDogs_MainQuest3", "DogMayor"))
            {
                GameObject dogmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Sibs_CatsDogs_DogMayor");
                if (dogmayor != null)
                {
                    Sibs_GlobalMove move;
                    if (!dogmayor.TryGetPart<Sibs_GlobalMove>(out move))
                    {
                        move = dogmayor.AddPart<Sibs_GlobalMove>();
                    }

                    move.TargetZone = Zones["North"];
                    move.TargetX = 32;
                    move.TargetY = 13;
                    move.failSafeTicks = 250L;
                    move.setStateOnArrival = "Sibs_CatsDogs_DogMayorMoved";

                    dogMayorMoveGiven = true;
                }
            }

            // Move the cat mayor to the meeting
            if (!catMayorMoveGiven && The.Game.HasFinishedQuestStep("Sibs_CatsDogs_MainQuest3", "CatMayor"))
            {
                GameObject catmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Sibs_CatsDogs_CatMayor");
                if (catmayor != null)
                {
                    Sibs_GlobalMove move;
                    if (!catmayor.TryGetPart<Sibs_GlobalMove>(out move))
                    {
                        move = catmayor.AddPart<Sibs_GlobalMove>();
                    }

                    move.TargetZone = Zones["North"];
                    move.TargetX = 36;
                    move.TargetY = 13;
                    move.failSafeTicks = 250L;
                    move.setStateOnArrival = "Sibs_CatsDogs_CatMayorMoved";

                    catMayorMoveGiven = true;
                }
            }

            return base.HandleEvent(E);
        }

        public override void Finish()
        {
            // bring the mayors back to their places
            GameObject dogmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Sibs_CatsDogs_DogMayor");
            if (dogmayor != null)
            {
                dogmayor.RemovePart<Sibs_GlobalMove>();

                var move = dogmayor.AddPart<Sibs_GlobalMove>();
                move.TargetZone = Zones["West"];
                move.TargetX = 26;
                move.TargetY = 18;
                move.failSafeTicks = 250L;
                move.removeAfterFailsafe = true;
            }

            GameObject catmayor = The.Player.Physics.CurrentCell.ParentZone.FindObject("Sibs_CatsDogs_CatMayor");
            if (catmayor != null)
            {
                catmayor.RemovePart<Sibs_GlobalMove>();
                
                var move = catmayor.AddPart<Sibs_GlobalMove>();
                move.TargetZone = Zones["East"];
                move.TargetX = 39;
                move.TargetY = 17;
                move.failSafeTicks = 250L;
                move.removeAfterFailsafe = true;
            }

            // Endings are handled in the outcome part of each zone, and the timer is added to the player
            foreach (var zone in Zones.Values)
            {
                The.ZoneManager.GetZone(zone).AddPart(new Sibs_CatsDogs_MainQuestOutcome());
            }
            
            var part = new Sibs_BoolStateTimer();
            part.startTurn = Calendar.TotalTimeTicks;
            part.state = "Sibs_CatsDogs_AnyEnding_Occured";
            part.targetTurns = 6000L;
            The.Player.AddPart(part);
        
        }
        public override GameObject GetInfluencer()
        {           
            return GameObject.FindByBlueprint("Sibs_CatsDogs_Warden Arendubark");
        }
    }
}