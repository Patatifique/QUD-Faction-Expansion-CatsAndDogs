using System;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_CatsDogs_EndingTimer : IPart
    {
        public long startTurn;
        public long targetTurns = 200L;

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("BeginTakeAction");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "BeginTakeAction" &&
                The.Game.Turns - this.startTurn > this.targetTurns)
            {
                The.Game.SetBooleanGameState("Brothers_CatsDogs_AnyEnding_Occured", true);
                this.ParentObject.RemovePart(this);
            }

            return true;
        }
    }
}