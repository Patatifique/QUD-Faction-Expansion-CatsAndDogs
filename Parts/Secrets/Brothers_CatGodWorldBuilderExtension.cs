using XRL;
using XRL.World;
using XRL.World.WorldBuilders;
using XRL.World.ZoneBuilders;

namespace Brothers.CatsAndDogs
{
    [JoppaWorldBuilderExtension]
    public class Brothers_CatGodWorldBuilderExtension  : IJoppaWorldBuilderExtension
    {
        public override void OnAfterBuild(JoppaWorldBuilder builder)
        {
            var location = builder.popMutableLocationOfTerrain("DesertCanyon", centerOnly: false);
            var zoneID = builder.ZoneIDFromXY("JoppaWorld", location.X, location.Y);

            var secret = builder.AddSecret(
                zoneID,
                "the napping spot of {{M|Miramihamia, Feline God}}",
                new string[2] { "lair", "cats" },
                "Lairs",
                "$brothers_catgod_lair"
            );

            var zoneManager = The.ZoneManager;


            zoneManager.SetZoneProperty(zoneID, "SkipTerrainBuilders", true);
            zoneManager.AddZonePostBuilder(zoneID, "MapBuilder", "FileName", "Brothers_ShrineCat.rpm");


            // I might add a music later
            //zoneManager.AddZonePostBuilder(zoneID, "Music", "Track", "Music/Bey Lah Heritage II");




            // You can also set various properties on the zone, if you wish.
            zoneManager.SetZoneName(zoneID, "napping spot of Miramihamia, Feline God", Article: "the", Proper: true);
            zoneManager.SetZoneIncludeStratumInZoneDisplay(zoneID, false);
            zoneManager.SetZoneProperty(zoneID, "NoBiomes", "Yes");
        }
    }
}