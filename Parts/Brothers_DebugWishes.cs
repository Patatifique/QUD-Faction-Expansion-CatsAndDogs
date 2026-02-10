using XRL.Wish;
using XRL.World.Capabilities;
using XRL.World.Skills.Cooking;
using XRL.Core;

namespace XRL.World.Capabilities
{
[HasWishCommand]

public class Brothers_DebugWishes
{


  [WishCommand(Command = "feraldinner")]
  public static void Brothers_FeralDinnerWishHandler()
  {
    CookingGameState.LearnRecipe((CookingRecipe) new Brothers_FeralDinner());
    The.Player.ReceiveObject("Bones", 1);
    The.Player.ReceiveObject("Furs", 1);
    The.Player.ReceiveObject("BloodCanteen", 1);
  }


}
}