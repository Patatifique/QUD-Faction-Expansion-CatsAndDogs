#nullable disable
namespace XRL
{
  [HasModSensitiveStaticCache]
  public static class Sibs_CatsDogs_Achievement
  {
    public static readonly AchievementInfo SIBS_CATSDOGS_NEUTRAL_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_NEUTRAL_ENDING", 
      "{{G|Status Quo}}", 
      "UI/Achievements/catsdogs/statusquo.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"Maintain a fragile peace between the villages of Shik and Spar.");
 
    public static readonly AchievementInfo SIBS_CATSDOGS_SHIK_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_SHIK_ENDING", 
      "{{G|A Dog's Bite}}", 
      "UI/Achievements/catsdogs/dogsbite.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"Settle the dispute in Shik and Spar in Shik's favor.");

    public static readonly AchievementInfo SIBS_CATSDOGS_SPAR_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_SPAR_ENDING", 
      "{{G|A Cat's Claw}}", 
      "UI/Achievements/catsdogs/catsclaw.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"Settle the dispute in Shik and Spar in Spar's favor.");

    public static readonly AchievementInfo SIBS_CATSDOGS_PERFECT_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_PERFECT_ENDING", 
      "{{G|Family}}", 
      "UI/Achievements/catsdogs/family.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"End the feud and reunite Shikspar.");


    public static readonly AchievementInfo SIBS_CATSDOGS_FERAL_DINNER = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_FERAL_DINNER", 
      "{{G|Gone Feral}}", 
      "UI/Achievements/catsdogs/feraldinner.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"Eat the Feral Dinner.");

   public static readonly AchievementInfo SIBS_CATSDOGS_HYDRA_DOG = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_HYDRA_DOG", 
      "{{G|The Pack Within}}", 
      "UI/Achievements/catsdogs/hydradog.png", 
      "{{K|Faction Expansion: Cats and Dogs}}\n"+"Witness a hydric hound reaching 10 heads.");


    [ModSensitiveCacheInit]
    public static void Init()
    {
      // had to reload the achievements json because it reads it too soon otherwise
      AchievementManager.Load();
    }
  }
}