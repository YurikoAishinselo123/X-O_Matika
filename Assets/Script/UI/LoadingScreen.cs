using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public float loadingTime = 2f;
    [SerializeField] float startValue = 0f;


    private void Start()
    {
        StartCoroutine(LoadSceneWithProgress());
    }

    private IEnumerator LoadSceneWithProgress()
    {
        float elapsedTime = 0f;
        startValue = 0.2f;
        float endValue = 1f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Lerp(startValue, endValue, elapsedTime / loadingTime);
            loadingSlider.value = progress;
            yield return null;
        }

        loadingSlider.value = 1f;
        SceneLoader.Instance.LoadInformation();
    }
}
