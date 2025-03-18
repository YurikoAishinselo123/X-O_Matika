using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public float loadingTime = 2f;

    private void Start()
    {
        StartCoroutine(LoadSceneWithProgress());
    }

    private IEnumerator LoadSceneWithProgress()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        SceneLoader.Instance.LoadSelectLevel();
    }
}
