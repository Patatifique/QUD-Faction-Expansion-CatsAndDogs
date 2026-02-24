using System;
using XRL.World;

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_BoolStateTimer : IPart
    {
        public long startTurn;
        public long targetTurns;

        public string state;

        public bool value = true;

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
                The.Game.SetBooleanGameState(state, value);
                this.ParentObject.RemovePart(this);
            }

            return true;
        }
    }
}