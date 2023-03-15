using System;
using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private CanvasGroup _fader;

    private void Awake()
    {
        _fader = GetComponent<CanvasGroup>();
    }

    public void FadeInAnOut(float fadeDuration, Action inBetweenCallback = null, Action fadedCallback = null)
    {
        StartCoroutine(_FadeInAnOut(fadeDuration, inBetweenCallback, fadedCallback));
    }

    private IEnumerator _FadeInAnOut(float fadeDuration, Action inBetweenCallback, Action fadedCallback)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeDuration) {
            _fader.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        _fader.alpha = 1f;

        inBetweenCallback?.Invoke();

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeDuration) {
            _fader.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        _fader.alpha = 0f;

        fadedCallback?.Invoke();
    }
}
