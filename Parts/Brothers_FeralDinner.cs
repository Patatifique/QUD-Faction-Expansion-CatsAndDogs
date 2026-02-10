using System;
using System.Collections.Generic;
using XRL.World.Effects;

#nullable disable
namespace XRL.World.Skills.Cooking
{

[Serializable]
public class Brothers_FeralDinner : CookingRecipe
{
    public Brothers_FeralDinner()
    {
      this.Components.Add((ICookingRecipeComponent) new PreparedCookingRecipieComponentLiquid("blood"));
      this.Components.Add((ICookingRecipeComponent) new PreparedCookingRecipeUnusualComponentBlueprint("Bones"));
      this.Components.Add((ICookingRecipeComponent) new PreparedCookingRecipeUnusualComponentBlueprint("Furs"));
      this.Effects.Add((ICookingRecipeResult) new CookingRecipeResultProceduralEffect(ProceduralCookingEffect.CreateSpecific(new List<string>()
      {
        "Brothers_CookingDomainSpecial_UnitFeralTransform"
      })));
    }

    public override string GetDescription() => "?????";

    public override string GetApplyMessage() => "";

    public override string GetDisplayName() => "{{W|Feral Dinner}}";

    public override bool ApplyEffectsTo(GameObject target, bool showMessage = true)
    {
      string str = "";
      if (showMessage)
        str = this.GetApplyMessage();
      foreach (ICookingRecipeResult effect in this.Effects)
        str = str + effect.apply(target) + "\n";
      return true;
    }
  }
}
