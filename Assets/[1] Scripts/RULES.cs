//Used to store GameRules referenced by multiple scripts which do not change during runtime.
//Gamerules specific to a single script/Object should use a scriptable object.
public class RULES
{
    public const int totalFlowers = 12;
    public const int totalMenInRed = 3;
    public const int totalNights = 3;
    public const int totalLovePoints = 9;
    public const float flowerCompletionWeight = 0.1f/totalFlowers;
    public const float menInRedCompletionWeight = 0.3f/totalMenInRed;
    public const float survivedNightsCompletionWeight = 0.3f/totalNights;
    public const float lovePointCompletionWeight = 0.4f/totalLovePoints;

    public const float nightLength = 300;
}