using System.Collections;
using UnityEngine;

public class WarningOverlayController : MonoBehaviour
{
    [SerializeField] private CanvasGroup overlayGroup;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine currentRoutine;

    public void FadeIn()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeCanvasGroup(overlayGroup, overlayGroup.alpha, 1f));
    }

    public void FadeOut()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeCanvasGroup(overlayGroup, overlayGroup.alpha, 0f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            yield return null;
        }
        cg.alpha = end;
    }
}
