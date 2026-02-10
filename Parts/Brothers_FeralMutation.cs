using System;
using XRL.World.Anatomy;
using XRL.World.Parts;

#nullable disable
namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class Brothers_FeralMutation : BaseMutation
    {
        public const string EQUIPMENT_BLUEPRINT = "Brothers_CatsDogs_MutationJaws";
        public const string MANAGER_ID = "Mutation::Brothers_FeralMutation";

        public Brothers_FeralMutation()
        {
            DisplayName = "Feral Jaws";
            Type = "Physical";
        }

        public override bool CanLevel() => false;

        public override bool GeneratesEquipment() => true;

        public override string GetDescription()
        {
            return "You bear long, fur-covered arms and powerful jaws.";
        }

        public override string GetLevelText(int Level)
        {
            return "" + "+4 Strength\n" + "+4 Toughness\n" + "+15 Cold Resistance\n" + "+15 Fire Resistance\n" + "+100 reputation with apes, baboons, bears, cats, dogs and grazing hedonists\n" + "Feral jaws act as a melee weapon with a chance to latch onto your target.";
        }

        private GameObject FindFeralJaws()
        {
            return this.ParentObject.Body?.FindEquipmentOrDefaultByBlueprint("Brothers_CatsDogs_MutationJaws");
        }

        private void AddJawsTo(BodyPart part)
        {
            GameObject jaws = part.FindEquipmentOrDefaultByBlueprint(EQUIPMENT_BLUEPRINT);
            if (jaws == null)
            {
                jaws = GameObject.Create(EQUIPMENT_BLUEPRINT);
                jaws.GetPart<Armor>().WornOn = part.Type;
                jaws.RequirePart<Brothers_FeralJawsItem>();
            }

            bool equippedAsDefault = jaws.EquipAsDefaultBehavior();

            if (equippedAsDefault && part.DefaultBehavior != null)
            {
                if (part.DefaultBehavior == jaws)
                    return;

                part.DefaultBehavior = (GameObject) null;
            }

            if (!equippedAsDefault && part.Equipped != null)
            {
                if (part.Equipped == jaws)
                    return;

                if (part.Equipped.CanBeUnequipped(SemiForced: true))
                    part.ForceUnequip(true);
            }

            if (!part.Equip(jaws, 0, true, SemiForced: true))
            {
                CleanUpMutationEquipment(ParentObject, ref jaws);
            }
        }

        public override bool Mutate(GameObject GO, int Level)
        {
            if (ParentObject.Body != null)
            {
                BodyPart part = ParentObject.Body.GetFirstPart("Face");

                if (part != null)
                {
                    AddJawsTo(part);
                }
            }

            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            CleanUpMutationEquipment(GO, FindFeralJaws());
            return base.Unmutate(GO);
        }
    }
}
