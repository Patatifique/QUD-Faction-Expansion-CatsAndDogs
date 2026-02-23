using XRL.Wish;
using Genkit;
using XRL.World.Capabilities;
using XRL.World.Skills.Cooking;
using XRL.Core;
using XRL.UI;
using Qud.API;
using System;

namespace XRL.World.Capabilities
{
[HasWishCommand]

public class Brothers_DebugWishes
{


  [WishCommand(Command = "feraldinnerTestOwO")]
  public static void Brothers_FeralDinnerWishHandler()
  {
    CookingGameState.LearnRecipe((CookingRecipe) new Brothers_FeralDinner());
    The.Player.ReceiveObject("Bones", 1);
    The.Player.ReceiveObject("Furs", 1);
    The.Player.ReceiveObject("BloodCanteen", 1);
  }

  [WishCommand(Command = "TimeToChooseASide")]
  public static void Brothers_TimeToChooseASideWishHandler()
  {
    
    // Goto Shikspar
    Zone zone = The.ZoneManager.GetZone("JoppaWorld.38.23.1.0.10");
    Point2D pos2D = The.Player.Physics.CurrentCell.Pos2D;
    The.Player.Physics.CurrentCell.RemoveObject(The.Player.Physics.ParentObject);
    zone.GetCell(pos2D).AddObject(The.Player);
    The.ZoneManager.SetActiveZone(zone);
    The.ZoneManager.ProcessGoToPartyLeader();


    // Start the quest
    The.Game.StartQuest("Brothers_CatsDogs_MainQuest3");

    // Reveal All Observations
    Popup.Suppress = true;
    JournalAPI.Observations.ForEach((Action<JournalObservation>) (o => JournalAPI.RevealObservation(o)));
    Popup.Suppress = false;
  }

  


}
}