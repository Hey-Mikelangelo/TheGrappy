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

    public GameEventsSO gameEvents;
    public bool isStartGameFader = false;
    public float fadeTime;
    public void OnEnable()
    {
        gameEvents.onPlayerDeath += Fade;
    }
    public void OnDisable()
    {
        gameEvents.onPlayerDeath -= Fade;
    }
    public void Unfade(float time)
    {
        StartCoroutine(UnfadeCanvasGroup(time));
    }
    public void Fade()
    {
        StartCoroutine(FadeCanvasGroup(fadeTime));
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
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        onFadeCompleted?.Invoke();
        if (isStartGameFader)
        {
            //gameEvents.OnFadedToMenu();
        }
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
        if (isStartGameFader)
        {
            //gameEvents.OnUnfadedToPlay();
        }
    }
}
