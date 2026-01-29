using XRL;
using XRL.Core;
using XRL.World;
using XRL.World.WorldBuilders;
using Qud.API;
using HistoryKit;

namespace Brothers.CatsAndDogs
{
    [JoppaWorldBuilderExtension]
    public class Brothers_CatsDogs_JournalAndHistoryWorldBuilderExtension : IJoppaWorldBuilderExtension
    {
        public override void OnAfterBuild(JoppaWorldBuilder builder)
        {
            //  Gossips to be given by the Gods
            JournalAPI.AddObservation(
                "Bubumube had round ears and nonretractile claws.", 
                "$Brothers_CatsDogs_ShiksparFounder_Cat", 
                "Gossip", 
                "$Brothers_CatsDogs_ShiksparFounder_Cat", 
                new string[1]
            );

            JournalAPI.AddObservation(
                "Bubumube had solitary tendecies and fished in Qud's rivers.",
                "$Brothers_CatsDogs_ShiksparFounder_Dog",
                "Gossip",
                "$Brothers_CatsDogs_ShiksparFounder_Dog",
                new string[1]
            );



            // Coordinates for Shikspar in JoppaWorld
            var shiksparLocation = builder.ZoneIDFromXY("JoppaWorld", 38*3, 23*3);

            var history = XRLCore.Core.Game.sultanHistory;

            // Add Shikspar village history
            if (!history.TryGetEntity("Shikspar", out HistoricEntity village))
            {
                long year = 857;
                village = history.CreateEntity("Shikspar", year);

                HistoricEvent foundingEvent = village.events[0];
                foundingEvent.SetEntityProperty("type", "village");
                foundingEvent.SetEntityProperty("name", "Shikspar");
            }

            // Add map note
            JournalAPI.AddMapNote(
                shiksparLocation,
                "Shikspar",
                "Settlements"
            );

            // Add journal notes for history
            JournalAPI.AddVillageNote(
                "Brothers_ShiksparSecret1",
                "In 857, Bubumube decided to quit traveling Qud. They left Ezra and settled with their followers beyond the southern jungles. There, they named the village Shikspar, and sowed starapple seeds in the furrowed dirt.",
                "Shikspar",
                Attributes: "village"
            );

            JournalAPI.AddVillageNote(
                "Brothers_ShiksparSecret2",
                "In 859, Bubumube left the direction of Shikspar to their loyal canine companion as their final will. They passed away peacefully, and from this day forth, the village was known as Shik.",
                "Shikspar",
                Attributes: "village"
            );

            JournalAPI.AddVillageNote(
                "Brothers_ShiksparSecret3",
                "In 859, Bubumube disappeared from Shikspar, never to be seen again. Come Harvest Dawn, a single feline babe was found swaddled in their bed and, from this day forth, the village was known as Spar.",
                "Shikspar",
                Attributes: "village"
            );
        }
    }
}
