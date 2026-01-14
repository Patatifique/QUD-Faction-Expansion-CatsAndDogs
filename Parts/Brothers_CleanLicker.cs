using System;
using System.Collections.Generic;
using XRL.World;
using XRL.World.Parts;

using XRL.Messages;

// New part used for the dog pet, licks anyone that pets it, cleaning them of liquids.

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_CleanLicker : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == PooledEvent<AfterPetEvent>.ID;
        }

        public override bool HandleEvent(AfterPetEvent E)
        {
            if (E.Object == ParentObject && E.Actor != E.Object && GameObject.Validate(E.Actor))
            {
                // Add temporary liquid volume
                LiquidVolume volume = ParentObject.GetPart<LiquidVolume>();
                bool added = false;

                if (volume == null)
                {
                    volume = new LiquidVolume();
                    volume.InitialLiquid = "water-1000";
                    ParentObject.AddPart(volume);
                    added = true;
                }

                // perform cleaning
                List<GameObject> Objects;
                List<string> Types;

                CleanItemsEvent.PerformFor(
                    E.Actor,
                    E.Actor,
                    ParentObject,
                    out Objects,
                    out Types
                );
                
                // debug message
                //MessageQueue.AddPlayerMessage("it works");

                // Remove the temporary part
                if (added)
                {
                    ParentObject.RemovePart("LiquidVolume");
                }
            }

            return base.HandleEvent(E);
        }

        public override bool AllowStaticRegistration() => true;
    }
}
