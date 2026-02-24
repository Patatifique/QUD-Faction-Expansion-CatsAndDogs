// This is a simple reusable part that allows an object to have a tile that changes based on its "evolutive stage".
// The stage can be changed by firing the "Brothers_ChangeEvolutiveState" event on the object.

using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.World;
using XRL.Messages;

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_EvolutiveTile : IPart
    {
        public int Stage = 0;

        public string StageTiles;

        [NonSerialized]
        private List<string> ParsedStageTiles;

        // returns the tile for the current Stage, or clamp if out of range
        public string GetTileForStage()
        {
            EnsureParsed();

            if (ParsedStageTiles == null || ParsedStageTiles.Count == 0)
                return null;

            // clamp Stage to valid range
            if (Stage < 0)
                Stage = 0;
            else if (Stage >= ParsedStageTiles.Count)
                Stage = ParsedStageTiles.Count - 1;

            return ParsedStageTiles[Stage];
        }

        private void EnsureParsed()
        {
            if (ParsedStageTiles != null)
                return;

            ParsedStageTiles = new List<string>();

            if (string.IsNullOrEmpty(StageTiles))
                return;

            string[] entries = StageTiles.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in entries)
            {
                ParsedStageTiles.Add(entry.Trim());
            }
        }   

        // event stuff


        // Event for non 0 stage at start
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == AfterObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(AfterObjectCreatedEvent e)
        {
            // set initial tile
            string tile = GetTileForStage();
            ParentObject.Render.Tile = tile;
            return base.HandleEvent(e);
        }

        // Main stage change event
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("Brothers_ChangeEvolutiveState");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "Brothers_ChangeEvolutiveState")
            {
                // increase stage
                Stage++;
                
                // debug message
                //MessageQueue.AddPlayerMessage("it's evolving!");

                // change tile
                string tile = GetTileForStage();

                ParentObject.Render.Tile = tile;
            }

            return base.FireEvent(E);
        }


    }
}
