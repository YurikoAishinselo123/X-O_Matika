using UnityEngine;

public class HowToPlayUI : MonoBehaviour
{
    public void startPlay()
    {
        SceneLoader.Instance.LoadGameplay();
    }
}