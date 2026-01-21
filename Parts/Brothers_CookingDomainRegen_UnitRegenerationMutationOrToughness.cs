using System;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;

#nullable disable
namespace XRL.World.Effects
{
    // Most of this code is adapted from ProceduralCookingEffectUnitMutation<T> where T is Regeneration
    // The changes were made to account for true kin characters

    [Serializable]
    public class Brothers_CookingDomainRegen_UnitRegenerationMutationOrToughness : ProceduralCookingEffectUnit
    {
        // True kin stuff
        public int Bonus = 1;

        // Mutant stuff
        public bool wasApplied;
        public string AddedTier = "1-2";
        public string BonusTier = "2-3";
        public int Tier = 1;
        public int moddedAmount;
        public string _MutationDisplayName;
        public string ClassName;
        public string TrackingProperty;
        public bool mutationWasAdded;
        public Guid modTracker;

        private bool targetIsTrueKin;

        public string MutationDisplayName
        {
            get
            {
            if (this._MutationDisplayName == null)
                this._MutationDisplayName = new Regeneration().DisplayName;
            return this._MutationDisplayName;
            }
            set => this._MutationDisplayName = value;
        }

        public override void Init(GameObject target)
        {
            Regeneration obj = new Regeneration();
            this.wasApplied = false;
            this._MutationDisplayName = obj.DisplayName;
            this.ClassName = obj.Name;
            this.TrackingProperty = "Equipped" + this.ClassName;
            if (!target.HasPart(typeof (Regeneration)))
            {
            this.mutationWasAdded = true;
            this.Tier = this.AddedTier.RollCached();
            }
            else
            this.Tier = this.BonusTier.RollCached();

            if (!target.IsTrueKin())
            {
                this.targetIsTrueKin = false;
            }
            else
            {
                this.targetIsTrueKin = true;
            }

        }

        public virtual string GetBonusZeroMessage() => "No effect.";

        public override string GetDescription()
        {
            if (this.targetIsTrueKin)
            {
                return "Gain +1 Toughness permanently.";
            }
            else
            {
                if (this.mutationWasAdded)
                    return $"Gain {this.MutationDisplayName} at level {this.Tier.ToString()} permanently.";
                if (this.Tier == 1)
                    return $"{this.Tier.Signed()} level to {this.MutationDisplayName} permanently.";
                return this.Tier == 0 ? this.GetBonusZeroMessage() : $"{this.Tier.Signed()} levels to {this.MutationDisplayName} permanently.";
            }
        }

        public override string GetTemplatedDescription()
        {
            if (this.targetIsTrueKin)
            {
                string templatedDescription = "+1 Toughness permanently";
                return templatedDescription;
            }
            else
            {
                string templatedDescription = $"Gain {this.MutationDisplayName} at level {this.AddedTier} permanently.";
                if (this.BonusTier != "0")
                templatedDescription = $"{templatedDescription} If @they already have {this.MutationDisplayName}, it's enhanced by {this.BonusTier} levels.";
                return templatedDescription;
            }
        }

        public override void Apply(GameObject go, Effect parent)
        {
            this.wasApplied = true;

            if (this.targetIsTrueKin)
            {
                go.GetStat("Toughness").BaseValue += this.Bonus;
                return;
            }

            string name = typeof (Regeneration).Name;
            if (!go.HasPart(name))
            go.SetIntProperty(this.TrackingProperty, 1);
            this.modTracker = go.RequirePart<Mutations>().AddMutationMod(name, Level: this.Tier, SourceType: Mutations.MutationModifierTracker.SourceType.Cooking, SourceName: parent.DisplayName);
            go.RegisterEffectEvent(parent, "BeforeMutationAdded");
            go.Body?.RegenerateDefaultEquipment();
            go.SyncMutationLevelAndGlimmer();
        }

        // remove was here but I
        // get this...
        // REMOVED IT 

        public override void FireEvent(Event E)
        {
            if (E.ID == "BeforeMutationAdded")
            {
                GameObject gameObjectParameter = E.GetGameObjectParameter("Object");
                if (E.GetStringParameter("Mutation") == this.ClassName)
                {
                    if (gameObjectParameter.HasIntProperty(this.TrackingProperty))
                    {
                    try
                    {
                        BaseMutation part = (BaseMutation) gameObjectParameter.GetPart<Regeneration>();
                        if (part != null)
                        {
                        part.Unmutate(gameObjectParameter);
                        gameObjectParameter.RemovePart((IPart) part);
                        }
                        gameObjectParameter.RemoveIntProperty(this.TrackingProperty);
                    }
                    catch (Exception ex)
                    {
                        MetricsManager.LogError("Exception on ProceduralCookingEffectUnitMutation during BeforeMutationAdded", ex);
                    }
                    }
                }
            }
            base.FireEvent(E);
        }

    }
}