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
        SceneLoader.Instance.LoadLoadingScreen();
    }

    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}