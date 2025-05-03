using UnityEngine;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        SceneLoader.Instance.LoadLoadingScreen();
    }

    private void QuitGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        SceneLoader.Instance.QuitGame();
    }
}