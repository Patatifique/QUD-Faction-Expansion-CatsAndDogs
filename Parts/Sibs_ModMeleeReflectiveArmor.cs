// This is almost a 1 to 1 copy of the vanilla ModGlassArmor but with the added fact that it only reflects melee damage!

using System;
using System.Collections.Generic;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class Sibs_ModMeleeReflectiveArmor : IModification
    {
        public string Type = "glass";

        public Sibs_ModMeleeReflectiveArmor()
        {
        }

        public Sibs_ModMeleeReflectiveArmor(int Tier)
            : base(Tier)
        {
        }

        public override void Configure() => this.WorksOnWearer = true;

        public override int GetModificationSlotUsage() => 0;

        public override bool ModificationApplicable(GameObject Object) => Object.HasPart<Armor>();

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == BeforeApplyDamageEvent.ID
                || ID == PooledEvent<GetItemElementsEvent>.ID
                || ID == GetShortDescriptionEvent.ID;
        }

        public override bool HandleEvent(BeforeApplyDamageEvent E)
        {
            if (E.Damage.Amount > 0
                && E.Damage.HasAttribute("Melee") // ONLY CHANGE
                && !E.Damage.HasAttribute("reflected")
                && !E.Damage.HasAttribute("Unavoidable")
                && this.IsReady(true))
            {
                GameObject equipped = this.ParentObject.Equipped;
                int num = (int)Math.Ceiling((double)E.Damage.Amount * (double)this.Tier / 100.0);
                if (num > 0 && E.Actor != null && E.Actor != this.ParentObject && E.Actor != equipped)
                {
                    List<string> stringList = new List<string>((IEnumerable<string>)E.Damage.Attributes);
                    if (!stringList.Contains("reflected"))
                        stringList.Add("reflected");

                    if (equipped != null && equipped.IsPlayer())
                    {
                        string[] strArray = new string[6];
                        GameObject parentObject = this.ParentObject;
                        GameObject gameObject = equipped;
                        bool? IncludeAdjunctNoun = new bool?();
                        GameObject AsPossessedBy = gameObject;
                        strArray[0] = parentObject.Does("reflect", IncludeAdjunctNoun: IncludeAdjunctNoun, AsPossessedBy: AsPossessedBy);
                        strArray[1] = " ";
                        strArray[2] = num.ToString();
                        strArray[3] = " damage back at ";
                        strArray[4] = E.Actor.t();
                        strArray[5] = ".";
                        IComponent<GameObject>.AddPlayerMessage(string.Concat(strArray));
                    }

                    GameObject actor = E.Actor;
                    int Amount = num;
                    string str = string.Join(" ", stringList.ToArray());
                    GameObject gameObject1 = equipped ?? this.ParentObject;
                    GameObject parentObject1 = this.ParentObject;
                    string Message = $"from %t {this.Type} armor!";
                    string Attributes = str;
                    GameObject Attacker = gameObject1;
                    GameObject Source = parentObject1;

                    actor.TakeDamage(Amount, Message, Attributes, Attacker: Attacker, Source: Source);
                    this.ParentObject.Equipped?.FireEvent("ReflectedDamage");
                }
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules($"Reflects {this.Tier.ToString()}% damage back at your attackers, rounded up.");
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetItemElementsEvent E)
        {
            if (E.IsRelevantObject(this.ParentObject))
                E.Add("glass", 10);
            return base.HandleEvent(E);
        }

        public override bool AllowStaticRegistration() => true;
    }
}