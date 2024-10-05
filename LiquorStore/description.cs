using Random = UnityEngine.Random;

namespace LiquorStore;

internal class description
{
    private static string[] strs = new string[3]
    {
        "THE STORE WHERE ONLY THE HARD GUYS GET THEIR ALCOHOL",
        "EARLY ACCESS TO ALCOHOLISM MEANS CONSEQUENCES",
        "FLORIDA MAN DRINKS 3 BOTTLES OF SPIRIT AND PASSES OUT OVER THE PERIOD OF 2 WEEKS"
    };

    public static string random() => description.strs[Random.Range(0, description.strs.Length - 1)];
}