using UnityEngine;

public class SelectLevelUI : MonoBehaviour
{
    public void SelectEasy()
    {
        SetDifficultyAndLoad(DifficultyLevel.Easy);
    }

    public void SelectMedium()
    {
        SetDifficultyAndLoad(DifficultyLevel.Medium);
    }

    public void SelectHard()
    {
        SetDifficultyAndLoad(DifficultyLevel.Hard);
    }

    private void SetDifficultyAndLoad(DifficultyLevel difficulty)
    {
        // DifficultyManager.Instance.SetDifficulty(difficulty);
        SceneLoader.Instance.LoadHowToPlay();
        // Debug.Log($"Selected Level: {difficulty}");
    }
}
