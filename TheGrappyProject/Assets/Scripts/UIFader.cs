using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public UnityAction onUnfadeCompleted;
    public UnityAction onFadeCompleted;

    public void Unfade(float time)
    {
        StartCoroutine(UnfadeCanvasGroup(time));
    }
    public void Fade(float time)
    {
        StartCoroutine(FadeCanvasGroup(time));
    }
    IEnumerator FadeCanvasGroup(float timeToUnfade)
    {
        if (timeToUnfade <= 0)
        {
            canvasGroup.alpha = 1;
        }
        float alpha = canvasGroup.alpha;
        float step = (1 / timeToUnfade);

        while (alpha < 1)
        {
            alpha += step * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        onFadeCompleted?.Invoke();
    }
    IEnumerator UnfadeCanvasGroup(float timeToUnfade)
    {
        if (timeToUnfade <= 0)
        {
            canvasGroup.alpha = 0;
        }
        float alpha = canvasGroup.alpha;
        float step = (1 / timeToUnfade);

        while (alpha > 0)
        {
            alpha -= step * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        onUnfadeCompleted?.Invoke();

    }
}
