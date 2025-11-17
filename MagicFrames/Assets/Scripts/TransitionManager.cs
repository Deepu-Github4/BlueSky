using UnityEngine;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private CanvasGroup cg;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cg = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeOut(float duration = 0.5f)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.fadeSound);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        cg.alpha = 1;
    }

    public IEnumerator FadeIn(float duration = 0.5f)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        cg.alpha = 0;
    }

    public IEnumerator PlayTransition(float duration = 0.5f)
    {
        yield return FadeOut(duration);

        yield return FadeIn(duration);
    }
}
