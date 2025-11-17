using UnityEngine;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 0.6f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToNextLevel(System.Action onFadeComplete)
    {
        StartCoroutine(FadeOutIn(onFadeComplete));
    }

    private IEnumerator FadeOutIn(System.Action onFadeComplete)
    {
        // Fade Out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadePanel.alpha = t / fadeDuration;
            yield return null;
        }

        fadePanel.alpha = 1;

        onFadeComplete?.Invoke();

        yield return new WaitForSeconds(0.15f);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadePanel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 0;
    }

    private IEnumerator FadeIn()
    {
        fadePanel.alpha = 1;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadePanel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 0;
    }
}
