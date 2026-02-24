using System;
using XRL.World;
using XRL.UI;
using XRL.World.Parts;
using System.Collections.Generic;
using System.Linq;
using XRL.Rules;

namespace XRL.World.ZoneParts
{
    [Serializable]
    public class Brothers_CatsDogs_MainQuestOutcome : IZonePart
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


        public override bool SameAs(IZonePart p) => false;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == ZoneActivatedEvent.ID;
        }


        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            if (The.Game.GetBooleanGameState("Brothers_CatsDogs_AnyEnding_Occured"))
            {       
                this.ApplyOutcome();
            }
            return base.HandleEvent(E);
        }

        public void DestroyPeopleFromFaction(string faction)
        {
            foreach (GameObject gameObject in this.ParentZone.GetObjects(
                o => o.HasPart<Brain>() &&
                o.GetPart<Brain>().GetPrimaryFaction() == faction
            ))
            {
                gameObject.Obliterate();
            }
        }

        public void PopulateRandomly(int min, int max, string blueprint)
        {
                int num1 = 0;
                for (int index = Stat.Random(min, max); num1 < index; ++num1)
                    ((IEnumerable<Cell>) this.ParentZone.GetEmptyCellsShuffled()).First<Cell>().AddObject(blueprint);
        }

        public void ReplaceObject(string oldObjectBlueprint, string newObjectBlueprint)
        {

            foreach (GameObject gameObject in this.ParentZone.GetObjects(o => o.Blueprint == oldObjectBlueprint))
            {
                var cell = gameObject.Physics?.CurrentCell;
                if (cell != null)
                {
                    cell.RemoveObject(gameObject);
                    cell.AddObject(newObjectBlueprint);
                }
            }
        }

        public void DestroyWalls()
        {

            foreach (GameObject gameObject in this.ParentZone.GetObjects(o =>
                o.HasTag("Brothers_CatsDogs_DestroyedOnEnding") ||
                o.HasTag("Brothers_CatsDogs_RubbleOnEnding")))
            {
                if (gameObject.HasPart<Brain>())
                    continue;

                if (gameObject.HasTag("Brothers_CatsDogs_DestroyedOnEnding"))
                {
                    gameObject.Obliterate();
                }
                else if (gameObject.HasTag("Brothers_CatsDogs_RubbleOnEnding"))
                {
                    var cell = gameObject.Physics?.CurrentCell;
                    if (cell != null)
                    {
                        cell.RemoveObject(gameObject);

                        // Roll For Random Rubble
                        string rubbleToAdd = null; 
                        int roll = Stat.Random(1, 11);

                        switch (roll)
                        {
                            case 1:
                                rubbleToAdd = "Rubble";
                                break;
                            case 2:
                                rubbleToAdd = "LargeBoulder";
                                break;
                            case 3:
                                rubbleToAdd = "MediumBoulder";
                                break;
                            case 4:
                                rubbleToAdd = "SmallBoulder";
                                break;
                        }

                        if (!string.IsNullOrEmpty(rubbleToAdd))
                            cell.AddObject(rubbleToAdd);
                    }
                }
            }
        }

        public void ReplaceTerrainData(string newDetail, string newName, string newDescription)
        {
            Cell cell = The.ZoneManager.GetZone("JoppaWorld").GetCell(38, 23);
            var terrain = cell.GetFirstObjectWithPart("TerrainTravel");
            if (terrain != null)
            {
                terrain.Render.DetailColor = newDetail;
                terrain.Render.DisplayName = newName;
                terrain.GetPart<Description>().Short = newDescription;}
        }

        public void ApplyOutcome()
        {
            // Ultimate Mayor failsafe, bringing them back to their place if they somehow didn't come back from the meeting
            foreach (var zoneID in Zones.Values)
            {
                var zone = The.ZoneManager.GetZone(zoneID);
                The.ZoneManager.SetCachedZone(zone); // Caching will also be usefull later when grabbing the lovers
                {
                    var dogMayor = zone.FindObject("Brothers_CatsDogs_DogMayor");
                    if (dogMayor != null)
                    {
                        var move = dogMayor.AddPart<Brothers_GlobalMove>();
                        move.TargetZone = Zones["West"];
                        move.TargetX = 26;
                        move.TargetY = 18;
                        move.failSafeTicks = 1L;
                        move.removeAfterFailsafe = true;
                    }

                    var catMayor = zone.FindObject("Brothers_CatsDogs_CatMayor");
                    if (catMayor != null)
                    {
                        var move = catMayor.AddPart<Brothers_GlobalMove>();
                        move.TargetZone = Zones["East"];
                        move.TargetX = 39;
                        move.TargetY = 17;
                        move.failSafeTicks = 1L;
                        move.removeAfterFailsafe = true;
                    }
                }
            }
            /////////////////////////////////////////
            // Neutral ending ///////////////////////
            /////////////////////////////////////////

            if(The.Game.GetBooleanGameState("Brothers_CatsDogs_NeutralEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_NeutralEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_NeutralEnding_Occured", true);
                    Popup.Show("Neutral Ending");
                }
            }
            
            /////////////////////////////////////////
            // Shik ending //////////////////////////
            /////////////////////////////////////////

            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_ShikEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_ShikEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_ShikEnding_Occured", true);
                    Popup.Show("Shik Ending");
                    
                    // Change Terrain Data
                    ReplaceTerrainData(
                        "k", 
                        "Shik", 
                        "Twin huts stand in memory of a community bisected by self-imposed exile.");
                }


                // Remove Spar people
                this.DestroyPeopleFromFaction("Spar");

                
                // Move Dog Lover
                var loverZone = The.ZoneManager.GetZone(Zones["West"]);
                var lover = loverZone.FindObject("Brothers_CatsDogs_DogLover");
                if (lover != null)
                {
                    var move = lover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["East"];
                    move.TargetX = 33;
                    move.TargetY = 18;
                    move.failSafeTicks = 1L;
                }
                
                // Zone specifc changes
                if (
                    this.ParentZone == The.ZoneManager.GetZone(Zones["East"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["NorthEast"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["SouthEast"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["Center"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["North"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["South"])
                    )
                {
                    // Populate everywhere except Shik
                    this.PopulateRandomly(2, 4, "Brothers_CatsDogs_ShikCitizen");
                    
                    // Change zone names
                    if (this.ParentZone == The.ZoneManager.GetZone(Zones["NorthEast"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["SouthEast"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["North"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["South"]))
                    {
                        The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "outskirts, Shik");
                    }
                    else if (this.ParentZone == The.ZoneManager.GetZone(Zones["Center"]) ||
                             this.ParentZone == The.ZoneManager.GetZone(Zones["East"]))
                    {
                        The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "Shik");
                    }

                }

                // Replace Monument
                this.ReplaceObject("Brothers_CatsDogs_SparMonument", "Brothers_CatsDogs_SparMonumentRuined");

                // Destroy Walls
                this.DestroyWalls();
            }

            /////////////////////////////////////////
            // Spar ending //////////////////////////
            /////////////////////////////////////////

            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_SparEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_SparEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_SparEnding_Occured", true);
                    Popup.Show("Spar Ending");

                    // Change Terrain Data
                    ReplaceTerrainData(
                        "k", 
                        "Spar", 
                        "Twin huts stand in memory of a community bisected by self-imposed exile.");
                }
                
                this.DestroyPeopleFromFaction("Shik");

                // Move Cat Lover
                var loverZone = The.ZoneManager.GetZone(Zones["East"]);
                var lover = loverZone.FindObject("Brothers_CatsDogs_CatLover");
                if (lover != null)
                {
                    var move = lover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["West"];
                    move.TargetX = 11;
                    move.TargetY = 15;
                    move.failSafeTicks = 1L;
                }
                
                // Zone specifc changes
                if (
                    this.ParentZone == The.ZoneManager.GetZone(Zones["West"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["NorthWest"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["SouthWest"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["Center"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["North"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["South"])
                    )
                {
                    // Populate everywhere except Spar
                    this.PopulateRandomly(2, 4, "Brothers_CatsDogs_SparCitizen");
                    // Change zone names
                    if (this.ParentZone == The.ZoneManager.GetZone(Zones["NorthWest"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["SouthWest"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["North"]) ||
                        this.ParentZone == The.ZoneManager.GetZone(Zones["South"]))
                    {
                        The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "outskirts, Spar");
                    }
                    else if (this.ParentZone == The.ZoneManager.GetZone(Zones["Center"]) ||
                             this.ParentZone == The.ZoneManager.GetZone(Zones["West"]))
                    {
                        The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "Spar");
                    }
                }

                // Replace Monument
                this.ReplaceObject("Brothers_CatsDogs_ShikMonument", "Brothers_CatsDogs_ShikMonumentRuined");

                // Destroy Walls
                this.DestroyWalls();               
            }
            
            /////////////////////////////////////////
            // Perfect ending////////////////////////
            /////////////////////////////////////////
            
            else if (The.Game.GetBooleanGameState("Brothers_CatsDogs_PerfectEnding"))
            {
                if(!The.Game.GetBooleanGameState("Brothers_CatsDogs_PerfectEnding_Occured"))
                {
                    The.Game.SetBooleanGameState("Brothers_CatsDogs_PerfectEnding_Occured", true);
                    Popup.Show("Perfect Ending");

                    // Change Terrain Data
                    ReplaceTerrainData(
                        "k", 
                        "Shikspar", 
                        "A newfound sense of kinship tears down misbegotten feuds and reunites a mending fraternity.");
                }

                //
                // Move BOTH lovers to Center
                //

                // Dog Lover
                var westZone = The.ZoneManager.GetZone(Zones["West"]);
                var dogLover = westZone.FindObject("Brothers_CatsDogs_DogLover");
                if (dogLover != null)
                {
                    var move = dogLover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["Center"];
                    move.TargetX = 26;
                    move.TargetY = 7;
                    move.failSafeTicks = 1L;
                }

                // Cat Lover
                var eastZone = The.ZoneManager.GetZone(Zones["East"]);
                var catLover = eastZone.FindObject("Brothers_CatsDogs_CatLover");
                if (catLover != null)
                {
                    var move = catLover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["Center"];
                    move.TargetX = 28;
                    move.TargetY = 6;
                    move.failSafeTicks = 1L;
                }

                //
                // Population logic
                //

                // Center, 2–3 of EACH
                if (
                    (this.ParentZone.ZoneID == Zones["Center"]) ||
                    (this.ParentZone.ZoneID == Zones["North"]) ||
                    (this.ParentZone.ZoneID == Zones["South"])
                    )
                {
                    this.PopulateRandomly(2, 3, "Brothers_CatsDogs_ShikCitizen");
                    this.PopulateRandomly(2, 3, "Brothers_CatsDogs_SparCitizen");
                }

                // West side, 1–2 Spar
                else if (
                    this.ParentZone.ZoneID == Zones["West"] ||
                    this.ParentZone.ZoneID == Zones["NorthWest"] ||
                    this.ParentZone.ZoneID == Zones["SouthWest"]
                )
                {
                    this.PopulateRandomly(1, 2, "Brothers_CatsDogs_SparCitizen");
                }

                // East side, 1–2 Shik
                else if (
                    this.ParentZone.ZoneID == Zones["East"] ||
                    this.ParentZone.ZoneID == Zones["NorthEast"] ||
                    this.ParentZone.ZoneID == Zones["SouthEast"]
                )
                {
                    this.PopulateRandomly(1, 2, "Brothers_CatsDogs_ShikCitizen");
                }

                //
                // Rename zones
                //

                if (
                    this.ParentZone.ZoneID == Zones["West"] ||
                    this.ParentZone.ZoneID == Zones["Center"] ||
                    this.ParentZone.ZoneID == Zones["East"]
                )
                {
                    The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "Shikspar");
                }
                else
                {
                    The.ZoneManager.SetZoneName(this.ParentZone.ZoneID, "outskirts, Shikspar");
                }

                // Destroy walls
                this.DestroyWalls();
            }
            
            // Debug
            else
            {
                Popup.Show("No ending");
            }

            // Remove part after applying the outcome
            this.ParentZone.RemovePart(this);
        }
    }
}