using UnityEngine;
using UnityEngine.UI;


public class HowToPlayUI1 : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    void Start()
    {
        continueButton.onClick.AddListener(Continue);
    }

    public void Continue()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        SceneLoader.Instance.LoadHowToPlay2();
    }

    //Temp
    void Update()
    {
        skipScene();
    }

    private void skipScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneLoader.Instance.LoadGameplay();
        }
    }
}