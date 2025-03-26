using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    public DifficultyLevel SelectedDifficulty { get; private set; } = DifficultyLevel.Easy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        SelectedDifficulty = difficulty;
    }
}
