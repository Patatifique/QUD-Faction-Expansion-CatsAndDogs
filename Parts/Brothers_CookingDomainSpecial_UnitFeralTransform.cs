using Qud.API;
using System;
using XRL.Language;
using XRL.UI;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;

#nullable disable
namespace XRL.World.Effects
{

  [Serializable]
  public class Brothers_CookingDomainSpecial_UnitFeralTransform : ProceduralCookingEffectUnit
  {
    public override string GetDescription() => "@they became (a feral thinggy idk please write here) permanently.";

    public override string GetTemplatedDescription() => "?????";

    public override void Init(GameObject target)
    {
    }

    public override void Apply(GameObject Object, Effect parent)
    {
      Brothers_CookingDomainSpecial_UnitFeralTransform.ApplyTo(Object);
    }

    public override void Remove(GameObject Object, Effect parent)
    {
    }

    public static void ApplyTo(GameObject Object)
    {
      if (Object.GetPropertyOrTag("Brothers_AteFeralDinner") == "true")
      {
        Object.ShowFailure("Your genome has already undergone this transformation.");
      }
      else
      {
        // Please balth write the description here (for now i kept the slug stuff)
        if (Object.IsPlayer())
        {
          Popup.Show("...");
          Popup.Show("You feel an uncomfortable pressure across the length of your body.");
          Popup.Show("Feelers rip through your scalp and shudder with curiosity.");
          Popup.Show("Your arms shrink into your torso.");
          Popup.Show("A bilge hose painted with mucus undulates out of your lower body. It spews the amniotic broth of its birth from its sputtering mouth.");
          JournalAPI.AddAccomplishment("You ate the Cloaca Surprise.", $"Slugform! Slugform! On the {Calendar.GetDay()} of {Calendar.GetMonth()}, in the year {Calendar.GetYear().ToString()} AR, =name= underwent the divine transformation and assumed the Slugform.", $"Deep in the bowels of Golgotha, =name= came upon a giant slug performing a secret ritual. Because of {Grammar.MakePossessive(The.Player.BaseDisplayNameStripped)} <spice.elements.{The.Player.GetMythicDomain()}.quality.!random>, the slug taught {The.Player.BaseDisplayNameStripped} the secret to being a slug.", muralCategory: MuralCategory.BodyExperienceNeutral, muralWeight: MuralWeight.VeryHigh);
          Achievement.ATE_SURPRISE.Unlock();
        }


        Object.Body.Rebuild("Brothers_CatsDogs_FeralTransformation");
        Object.RequirePart<Mutations>().AddMutation((BaseMutation) new Brothers_FeralMutation());
        
        // what color do you want the player to be bro
        Object.Render.RenderString = "Q";

        Object.Render.Tile = "Creatures/catsdogs/feraltransformation.png";
        Object.SetStringProperty("Brothers_AteFeralDinner", "true");

        // do we wanna change the species ?
        //Object.SetStringProperty("Species", "slug");
      }
    }
  }

}
