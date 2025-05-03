using UnityEngine;
using UnityEngine.UI;


public class InformationUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    void Start()
    {
        continueButton.onClick.AddListener(Continue);
    }

    public void Continue()
    {
        // AudioManager.Instance.PlayClickButtonSFX();
        SceneLoader.Instance.LoadSelectLevel();
    }
}