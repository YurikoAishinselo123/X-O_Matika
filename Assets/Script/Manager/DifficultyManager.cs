using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

public class DifficultyManager : MonoBehaviour
{
    private const string DifficultyKey = "SelectedDifficulty";

    public static void SetDifficulty(DifficultyLevel difficulty)
    {
        PlayerPrefs.SetInt(DifficultyKey, (int)difficulty);
        PlayerPrefs.Save();
    }

    public static DifficultyLevel GetDifficulty()
    {
        return (DifficultyLevel)PlayerPrefs.GetInt(DifficultyKey, (int)DifficultyLevel.Easy);
    }
}
