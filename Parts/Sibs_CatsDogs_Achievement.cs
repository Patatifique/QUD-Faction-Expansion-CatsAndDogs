#nullable disable
namespace XRL
{
  [HasModSensitiveStaticCache]
  public static class Sibs_CatsDogs_Achievement
  {
    public static readonly AchievementInfo SIBS_CATSDOGS_NEUTRAL_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_NEUTRAL_ENDING", 
      "FE - Cats and Dogs: Status Quo", 
      "UI/Achievements/catsdogs/statusquo.png", 
      "Maintain the fragile peace between the villages of Shik and Spar.");
 
    public static readonly AchievementInfo SIBS_CATSDOGS_SHIK_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_SHIK_ENDING", 
      "FE - Cats and Dogs: The Dog's Bite", 
      "UI/Achievements/catsdogs/dogsbite.png", 
      "Settle the dispute in Shik and Spar in Shik's favor.");

    public static readonly AchievementInfo SIBS_CATSDOGS_SPAR_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_SPAR_ENDING", 
      "FE - Cats and Dogs: The Cat's Claw", 
      "UI/Achievements/catsdogs/catsclaw.png", 
      "Settle the dispute in Shik and Spar in Spar's favor.");

    public static readonly AchievementInfo SIBS_CATSDOGS_PERFECT_ENDING = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_PERFECT_ENDING", 
      "FE - Cats and Dogs: Family", 
      "welcome.png", 
      "End the feud and reunite Shikspar.");


    public static readonly AchievementInfo SIBS_CATSDOGS_FERAL_DINNER = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_FERAL_DINNER", 
      "FE - Cats and Dogs: Gone Feral", 
      "UI/Achievements/catsdogs/feraldinner.png", 
      "Eat the Feral Dinner.");

   public static readonly AchievementInfo SIBS_CATSDOGS_HYDRA_DOG = new AchievementInfo(
      "ACH_SIBS_CATSDOGS_HYDRA_DOG", 
      "FE - Cats and Dogs: The Pack Within", 
      "UI/Achievements/catsdogs/hydradog.png", 
      "Witness a hydric hound reaching 10 heads.");


    [ModSensitiveCacheInit]
    public static void Init()
    {
      // had to reload the achievements json because it reads it too soon otherwise
      AchievementManager.Load();
    }
  }
}