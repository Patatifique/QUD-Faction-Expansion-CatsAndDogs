using System;
using XRL.Rules;
using XRL.Messages;

#nullable disable
namespace XRL.World.Parts
{
  [Serializable]
  public class Brothers_ElementalDamageOnHit : IActivePart
  {
    // This part applies elemental damage on hit. 
    // It can be configured to work on the wielder's hits or the weapon's hits as opposed to the regular ElementalDamage part.
    // It was made mainly for unarmed attacks, but can be used for any weapon type by adjusting the RequireDamageAttribute field.
    // AttackMessage can also be customized :>
    // Example usage: <part Name="Brothers_ElementalDamageOnHit" WorksOnEquipper= "true" Type="Cold" RequireDamageAttribute="Unarmed" Amount="3d2"

    
    public string Amount = "1d3";
    public string Type = "Acid";
    public string RequireDamageAttribute;
    public string AttackMessage= "from %t attack.";

    public Brothers_ElementalDamageOnHit() => this.WorksOnSelf = true;
    
    public override bool WantEvent(int ID, int cascade)
    {
      return base.WantEvent(ID, cascade)
        || ID == EquippedEvent.ID
        || ID == UnequippedEvent.ID;
    }

    public override bool HandleEvent(EquippedEvent E)
    {
        E.Actor.RegisterPartEvent(this, "WieldedWeaponHit");
        return base.HandleEvent(E);
    }

    public override bool HandleEvent(UnequippedEvent E)
    {
        E.Actor.UnregisterPartEvent(this, "WieldedWeaponHit");
        return base.HandleEvent(E);
    }

    public override bool AllowStaticRegistration() => false;

    public override void Register(GameObject Object, IEventRegistrar Registrar)
    {
        Registrar.Register("WeaponHit");
        base.Register(Object, Registrar);
    }

    private bool CheckApply(Event E)
    {
        if (!this.RequireDamageAttribute.IsNullOrEmpty() && (!(E.GetParameter("Damage") is Damage parameter) || !parameter.HasAttribute(this.RequireDamageAttribute)) || !this.IsReady(true))
        {
            return false;
        }

        GameObject attacker = E.GetGameObjectParameter("Attacker");
        GameObject defender = E.GetGameObjectParameter("Defender");

        int amount = this.Amount.RollCached();
        
        string damageTypeMessage = "damage";

        if (this.Type == "Acid")
            damageTypeMessage = "{{g|acid damage}}";
        
        else if (this.Type == "Heat")
            damageTypeMessage = "{{fiery|heat damage}}";
        
        else if (this.Type == "Cold")
            damageTypeMessage = "{{icy|cold damage}}";

        else if (this.Type == "Electrical")
            damageTypeMessage = "{{W|electrical damage}}";
        
        defender.TakeDamage(
            amount,
            this.AttackMessage,
            this.Type,
            Attacker: attacker,
            SilentIfNoDamage: true,
            ShowDamageType: damageTypeMessage 
        );

        return true;
    }

    public override bool FireEvent(Event E)
    {
        if (E.ID == "WieldedWeaponHit")
        {
            if (this.IsObjectActivePartSubject(E.GetGameObjectParameter("Attacker")))
                this.CheckApply(E);
        }
        else if (E.ID == "WeaponHit")
        {
            if (this.IsObjectActivePartSubject(E.GetGameObjectParameter("Weapon")))
                this.CheckApply(E);
        }
      return base.FireEvent(E);
    }
  }
}
