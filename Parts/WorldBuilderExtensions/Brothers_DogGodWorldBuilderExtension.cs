using XRL;
using XRL.World;
using XRL.World.WorldBuilders;
using XRL.World.ZoneBuilders;
using XRL.World.Parts;

namespace Brothers.CatsAndDogs
{
    [JoppaWorldBuilderExtension]
    public class Brothers_DogGodWorldBuilderExtension  : IJoppaWorldBuilderExtension
    {
        public override void OnAfterBuild(JoppaWorldBuilder builder)
        {
            var location = builder.popMutableLocationOfTerrain("Hills", centerOnly: false);
            var zoneID = builder.ZoneIDFromXY("JoppaWorld", location.X, location.Y);

            var secret = builder.AddSecret(
                zoneID,
                "the mourning cave of {{M|Boroworof, Canine God}}",
                new string[2] { "lair", "dogs" },
                "Lairs",
                "$brothers_doggod_lair"
            );

            var zoneManager = The.ZoneManager;
            
            Faction faction = Factions.Get("Dogs");
            faction.HolyPlaces.Add(zoneID);


            zoneManager.SetZoneProperty(zoneID, "SkipTerrainBuilders", true);
            zoneManager.AddZonePostBuilder(zoneID, "MapBuilder", "FileName", "Brothers_ShrineDog.rpm");


            // I might add a music later
            //zoneManager.AddZonePostBuilder(zoneID, "Music", "Track", "Music/Bey Lah Heritage II");




            zoneManager.SetZoneName(zoneID, "mourning cave of Boroworof, Canine God", Article: "the", Proper: true);
            zoneManager.SetZoneIncludeStratumInZoneDisplay(zoneID, false);
            zoneManager.SetZoneProperty(zoneID, "NoBiomes", "Yes");

            zoneManager.GetZone("JoppaWorld").GetCell(location.X / 3, location.Y / 3).GetFirstObjectWithPart("TerrainTravel")?.GetPart<TerrainTravel>().AddEncounter(new EncounterEntry("Dug in a hillside is the lair of a godly canine. Would you like to investigate?", zoneID, "", secret, true));
        }
    }
}