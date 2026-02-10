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
    public override string GetDescription() => "@they became a feral beast permanently.";

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
        if (Object.IsPlayer())
        {
          Popup.Show("...");
          Popup.Show("You feel an uncomfortable pressure across the length of your body.");
          Popup.Show("Your back bend with a sickening crunch as you grow fur all over.");
          Popup.Show("Your arms thicken and elongate, rippling with muscle.");
          Popup.Show("Jagged fangs jut out of your gums, tasting the air in search of blood.");
		  Popup.Show("You gained the mutation {{C|Feral Jaws}}!");
          JournalAPI.AddAccomplishment("You ate the Cloaca Surprise.", $"Houndform! Houndform! On the {Calendar.GetDay()} of {Calendar.GetMonth()}, in the year {Calendar.GetYear().ToString()} AR, =name= underwent the divine transformation and became a great hound.", $"To defend the city of Shikspar against invading goatfolk, =name= howled by the Beetle Moon until {The.Player.GetPronounProvider().Subjective} assumed the form of a great hound.", muralCategory: MuralCategory.BodyExperienceNeutral, muralWeight: MuralWeight.VeryHigh);
        }


        Object.Body.Rebuild("Brothers_CatsDogs_FeralTransformation");
        Object.RequirePart<Mutations>().AddMutation((BaseMutation) new Brothers_FeralMutation());
        
        Object.Render.RenderString = "Q";

        Object.Render.Tile = "Creatures/catsdogs/feraltransformation.png";
        Object.SetStringProperty("Brothers_AteFeralDinner", "true");

        Object.SetStringProperty("Species", "dog");
      }
    }
  }

}
