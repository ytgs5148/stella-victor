using System.Collections;
using UnityEngine;

public class CanvasFadeTransition : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float transitionTime = 1f;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / transitionTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / transitionTime);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
