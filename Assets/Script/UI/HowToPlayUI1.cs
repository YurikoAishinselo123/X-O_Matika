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
        SceneLoader.Instance.LoadHowToPlay2();
    }
}