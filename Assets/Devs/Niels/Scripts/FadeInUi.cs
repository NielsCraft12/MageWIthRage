using System.Collections;
using UnityEngine;

public class FadeInUi : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 0.5f;

    [SerializeField]
    private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Start invisible for UI
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        canvasGroup.interactable = true;
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void FadeOutAndDisable()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, fadeCurve.Evaluate(progress));
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, fadeCurve.Evaluate(progress));
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut()
    {
        canvasGroup.blocksRaycasts = false;
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, fadeCurve.Evaluate(progress));
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
