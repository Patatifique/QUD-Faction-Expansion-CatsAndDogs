using System;
using XRL.World.Parts.Mutation;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_FeralJawsItem : IPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("Unequipped");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "Unequipped")
            {
                GameObject unequippingObject = E.GetGameObjectParameter("UnequippingObject");
                unequippingObject?.GetPart<Brothers_FeralMutation>()?.Unmutate(unequippingObject);
            }

            return base.FireEvent(E);
        }
    }
}
