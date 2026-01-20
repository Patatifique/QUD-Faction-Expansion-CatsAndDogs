using System;
using System.Text;
using XRL.Core;
using XRL.Rules;
using XRL.Messages;
using XRL.World.Capabilities;

#nullable disable
namespace XRL.World.Parts
{

    [Serializable]
    public class Brothers_VomitOnEat : IPart
    {

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("OnEat");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "OnEat")
            {
                GameObject eater = E.GetGameObjectParameter("Eater");
                
                // kinda hacky way to induce vomiting
                bool exitInterface = false;
                InduceVomitingEvent.Send(eater, ref exitInterface, null);   
 
            }

            return base.FireEvent(E);
        }
    }
}
