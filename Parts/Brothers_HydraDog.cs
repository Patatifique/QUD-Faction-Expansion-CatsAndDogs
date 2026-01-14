using System;
using System.Collections.Generic;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Anatomy;

using XRL.Messages;


namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_HydraDog : IPart
    {
        public string AdditionsManagerID => this.ParentObject.ID + "::HydraDog::Add";
        public string ChangesManagerID => this.ParentObject.ID + "::HydraDog::Change";

        public int MaxHeads = 10;


        public void AddHead()
        {
            // coppied from the TwoHeaded Mutation
            Body body1 = this.ParentObject.Body;
            if (body1 != null)
            {
                BodyPart body2 = body1.GetBody();
                BodyPart firstAttachedPart1 = body2.GetFirstAttachedPart("Head", 0, body1, true);
                BodyPart firstAttachedPart2 = firstAttachedPart1 == null ? (BodyPart) null : firstAttachedPart1.GetFirstAttachedPart("Face", 0, body1, true);
                if (firstAttachedPart1 != null && firstAttachedPart1.Manager == null && firstAttachedPart2 != null && firstAttachedPart2.Manager == null && firstAttachedPart1.IsLateralitySafeToChange(BackupParentBody: body1, ChildPart: firstAttachedPart2))
                {
                    BodyPart bodyPart1 = body2;
                    BodyPart InsertAfter = firstAttachedPart1;
                    int? nullable = new int?(body2.Category);
                    string additionsManagerId1 = this.AdditionsManagerID;
                    int? Category1 = nullable;
                    int? RequiresLaterality1 = new int?();
                    int? Mobility1 = new int?();
                    bool? Appendage1 = new bool?();
                    bool? Integral1 = new bool?();
                    bool? Mortal1 = new bool?();
                    bool? Abstract1 = new bool?();
                    bool? Extrinsic1 = new bool?();
                    bool? Dynamic1 = new bool?();
                    bool? Plural1 = new bool?();
                    bool? Mass1 = new bool?();
                    bool? Contact1 = new bool?();
                    bool? IgnorePosition1 = new bool?();
                    BodyPart bodyPart2 = bodyPart1.AddPartAt(InsertAfter, "Head", 1, Manager: additionsManagerId1, Category: Category1, RequiresLaterality: RequiresLaterality1, Mobility: Mobility1, Appendage: Appendage1, Integral: Integral1, Mortal: Mortal1, Abstract: Abstract1, Extrinsic: Extrinsic1, Dynamic: Dynamic1, Plural: Plural1, Mass: Mass1, Contact: Contact1, IgnorePosition: IgnorePosition1);
                    nullable = new int?(body2.Category);
                    string additionsManagerId2 = this.AdditionsManagerID;
                    int? Category2 = nullable;
                    int? RequiresLaterality2 = new int?();
                    int? Mobility2 = new int?();
                    bool? Appendage2 = new bool?();
                    bool? Integral2 = new bool?();
                    bool? Mortal2 = new bool?();
                    bool? Abstract2 = new bool?();
                    bool? Extrinsic2 = new bool?();
                    bool? Dynamic2 = new bool?();
                    bool? Plural2 = new bool?();
                    bool? Mass2 = new bool?();
                    bool? Contact2 = new bool?();
                    bool? IgnorePosition2 = new bool?();
                    bodyPart2.AddPart("Face", 1, Manager: additionsManagerId2, Category: Category2, RequiresLaterality: RequiresLaterality2, Mobility: Mobility2, Appendage: Appendage2, Integral: Integral2, Mortal: Mortal2, Abstract: Abstract2, Extrinsic: Extrinsic2, Dynamic: Dynamic2, Plural: Plural2, Mass: Mass2, Contact: Contact2, IgnorePosition: IgnorePosition2);
                    firstAttachedPart1.ChangeLaterality(2);
                    firstAttachedPart2.ChangeLaterality(2);
                    firstAttachedPart1.Manager = this.ChangesManagerID;
                    firstAttachedPart2.Manager = this.ChangesManagerID;
                }
                else
                {
                    int? nullable;
                    BodyPart bodyPart3;
                    if (firstAttachedPart1 != null)
                    {
                        BodyPart bodyPart4 = body2;
                        BodyPart InsertAfter = firstAttachedPart1;
                        nullable = new int?(body2.Category);
                        string additionsManagerId = this.AdditionsManagerID;
                        int? Category = nullable;
                        int? RequiresLaterality = new int?();
                        int? Mobility = new int?();
                        bool? Appendage = new bool?();
                        bool? Integral = new bool?();
                        bool? Mortal = new bool?();
                        bool? Abstract = new bool?();
                        bool? Extrinsic = new bool?();
                        bool? Dynamic = new bool?();
                        bool? Plural = new bool?();
                        bool? Mass = new bool?();
                        bool? Contact = new bool?();
                        bool? IgnorePosition = new bool?();
                        bodyPart3 = bodyPart4.AddPartAt(InsertAfter, "Head", Manager: additionsManagerId, Category: Category, RequiresLaterality: RequiresLaterality, Mobility: Mobility, Appendage: Appendage, Integral: Integral, Mortal: Mortal, Abstract: Abstract, Extrinsic: Extrinsic, Dynamic: Dynamic, Plural: Plural, Mass: Mass, Contact: Contact, IgnorePosition: IgnorePosition);
                    }
                    else
                    {
                        BodyPart bodyPart5 = body2;
                        nullable = new int?(body2.Category);
                        string additionsManagerId = this.AdditionsManagerID;
                        int? Category = nullable;
                        string[] strArray = new string[8]
                        {
                            "Back",
                            "Arm",
                            "Leg",
                            "Foot",
                            "Hands",
                            "Feet",
                            "Roots",
                            "Thrown Weapon"
                        };
                        int? RequiresLaterality = new int?();
                        int? Mobility = new int?();
                        bool? Appendage = new bool?();
                        bool? Integral = new bool?();
                        bool? Mortal = new bool?();
                        bool? Abstract = new bool?();
                        bool? Extrinsic = new bool?();
                        bool? Dynamic = new bool?();
                        bool? Plural = new bool?();
                        bool? Mass = new bool?();
                        bool? Contact = new bool?();
                        bool? IgnorePosition = new bool?();
                        string[] OrInsertBefore = strArray;
                        bodyPart3 = bodyPart5.AddPartAt("Head", Manager: additionsManagerId, Category: Category, RequiresLaterality: RequiresLaterality, Mobility: Mobility, Appendage: Appendage, Integral: Integral, Mortal: Mortal, Abstract: Abstract, Extrinsic: Extrinsic, Dynamic: Dynamic, Plural: Plural, Mass: Mass, Contact: Contact, IgnorePosition: IgnorePosition, InsertAfter: "Head", OrInsertBefore: OrInsertBefore);
                    }
                    BodyPart bodyPart6 = bodyPart3;
                    nullable = new int?(body2.Category);
                    string additionsManagerId3 = this.AdditionsManagerID;
                    int? Category3 = nullable;
                    int? RequiresLaterality3 = new int?();
                    int? Mobility3 = new int?();
                    bool? Appendage3 = new bool?();
                    bool? Integral3 = new bool?();
                    bool? Mortal3 = new bool?();
                    bool? Abstract3 = new bool?();
                    bool? Extrinsic3 = new bool?();
                    bool? Dynamic3 = new bool?();
                    bool? Plural3 = new bool?();
                    bool? Mass3 = new bool?();
                    bool? Contact3 = new bool?();
                    bool? IgnorePosition3 = new bool?();
                    bodyPart6.AddPart("Face", Manager: additionsManagerId3, Category: Category3, RequiresLaterality: RequiresLaterality3, Mobility: Mobility3, Appendage: Appendage3, Integral: Integral3, Mortal: Mortal3, Abstract: Abstract3, Extrinsic: Extrinsic3, Dynamic: Dynamic3, Plural: Plural3, Mass: Mass3, Contact: Contact3, IgnorePosition: IgnorePosition3);
                }
                // Add a bite to each face
                foreach (BodyPart Part in body1.GetPart("Face"))
                    {
                        if (Part.Equipped == null)
                            this.ParentObject.ForceEquipObject(GameObject.Create("Brothers_CatsDogs_Hydric Bite"), Part, true, new int?(0));
                    }
            }
        }


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
                
                // Limit number of heads with early exit
                if (this.ParentObject.Body.GetPart("Head").Count >= MaxHeads)
                    return base.FireEvent(E);
                
                // Trigger evolutive tile change
                this.ParentObject.FireEvent(Event.New("Brothers_ChangeEvolutiveState"));
                
                // Add Head
                AddHead();
                
                // debug stuff
                MessageQueue.AddPlayerMessage("it's evolving!");
            }
            return base.FireEvent(E);
        }


        public override bool AllowStaticRegistration() => true;
    }
}
