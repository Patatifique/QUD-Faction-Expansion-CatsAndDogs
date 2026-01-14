using System;
using System.Collections.Generic;
using XRL.World;
using XRL.World.Parts;

using XRL.Messages;


namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_HydraDog : IPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("DefenderHit");
            Registrar.Register("DefenderMissileWeaponHit");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if ((E.ID == "DefenderHit" || E.ID == "DefenderMissileWeaponHit") && E.GetIntParameter("Penetrations") > 0)
            {
                //my logic here
                MessageQueue.AddPlayerMessage("it's evolving!");
            }
            return base.FireEvent(E);
        }


        public override bool AllowStaticRegistration() => true;
    }
}
