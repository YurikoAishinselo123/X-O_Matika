using UnityEngine;
using UnityEngine.UI;


public class HowToPlayUI2 : MonoBehaviour
{
    [SerializeField] private Button startGamePlayButton;

    void Start()
    {
        startGamePlayButton.onClick.AddListener(StartGamePlay);
    }

    public void StartGamePlay()
    {
        SceneLoader.Instance.LoadGameplay();
    }
}