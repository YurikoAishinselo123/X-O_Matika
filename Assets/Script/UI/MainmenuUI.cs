using UnityEngine;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    private bool backsoundIsPlaying = true;


    void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // void Update()
    // {
    //     if (!backsoundIsPlaying)
    //     {
    //         Debug.Log("stop");

    //     }
    // }

    private void PlayGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        backsoundIsPlaying = false;
        SceneLoader.Instance.LoadLoadingScreen();
    }

    private void QuitGame()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        SceneLoader.Instance.QuitGame();
    }
}