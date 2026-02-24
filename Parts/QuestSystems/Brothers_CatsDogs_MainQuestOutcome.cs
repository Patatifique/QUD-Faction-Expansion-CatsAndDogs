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
                    
                    // Change Terrain Data
                    ReplaceTerrainData(
                        "k", 
                        "Shik", 
                        "Shik Description");
                }
                // Remove Spar people
                this.DestroyPeopleFromFaction("Spar");

                
                // Move Dog Lover
                var loverZone = The.ZoneManager.GetZone(Zones["West"]);
                The.ZoneManager.SetCachedZone(loverZone);
                var lover = loverZone.FindObject("Brothers_CatsDogs_DogLover");
                if (lover != null)
                {
                    var move = lover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["East"];
                    move.TargetX = 33;
                    move.TargetY = 18;
                    move.failSafeTicks = 1L;
                }
                
                // Populating Shik people in the east zones
                if (
                    this.ParentZone == The.ZoneManager.GetZone(Zones["East"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["NorthEast"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["SouthEast"])
                )
                {
                    this.PopulateRandomly(3, 5, "Brothers_CatsDogs_ShikCitizen");
                }

                // Replace Monument
                this.ReplaceObject("Brothers_CatsDogs_SparMonument", "Brothers_CatsDogs_SparMonumentRuined");

                // Destroy Walls
                this.DestroyWalls();
            }
            
            // Spar ending

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
                        "Spar Description");
                }
                
                this.DestroyPeopleFromFaction("Shik");

                // Move Cat Lover
                var loverZone = The.ZoneManager.GetZone(Zones["East"]);
                The.ZoneManager.SetCachedZone(loverZone);
                var lover = loverZone.FindObject("Brothers_CatsDogs_CatLover");
                if (lover != null)
                {
                    var move = lover.AddPart<Brothers_GlobalMove>();
                    move.TargetZone = Zones["West"];
                    move.TargetX = 11;
                    move.TargetY = 15;
                    move.failSafeTicks = 1L;
                }
                
                // Populating Spar people in the west zones
                if (
                    this.ParentZone == The.ZoneManager.GetZone(Zones["West"]) || 
                    this.ParentZone == The.ZoneManager.GetZone(Zones["NorthWest"]) ||
                    this.ParentZone == The.ZoneManager.GetZone(Zones["SouthWest"])
                )
                {
                    this.PopulateRandomly(3, 5, "Brothers_CatsDogs_SparCitizen");
                }

                // Replace Monument
                this.ReplaceObject("Brothers_CatsDogs_ShikMonument", "Brothers_CatsDogs_ShikMonumentRuined");

                // Destroy Walls
                this.DestroyWalls();               
            }
            
            // Perfect ending

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
                        "Shikspar Description");
                }

                // Destroy Walls
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