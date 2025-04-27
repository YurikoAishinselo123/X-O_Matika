using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public static string SelectedDifficulty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu() => LoadScene("MainMenu");
    public void LoadLoadingScreen() => LoadScene("LoadingScreen");
    public void LoadInformation() => LoadScene("Information");
    public void LoadSelectLevel() => LoadScene("SelectLevel");
    public void LoadHowToPlay1() => LoadScene("HowToPlay1");
    public void LoadHowToPlay2() => LoadScene("HowToPlay2");
    public void LoadGameplay() => LoadScene("TicTaoToe");


    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No previous scene stored!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
