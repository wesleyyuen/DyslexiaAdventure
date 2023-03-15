using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Text _name;
    [SerializeField] private Text _text;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvas.worldCamera = _cam;
        _canvasGroup.alpha = 0f;
    }

    public void FadeIn(float duration)
    {
        _canvas.enabled = true;
        FadeCanvas(0f, 1f, duration);
    }

    public void FadeCanvas(float from, float to, float duration)
    {
        if (_canvasGroup.alpha == to) return;
        StartCoroutine(_FadeCanvas(from, to, duration));
    }

    private IEnumerator _FadeCanvas(float from, float to, float duration)
    {
        if (duration == 0f) {
            _canvasGroup.alpha = to;
            yield break;
        }

        _canvasGroup.alpha = from;

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            _canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        _canvasGroup.alpha = to;
    }

    public void SetText(string name, string text)
    {
        if (_name != null) _name.text = name + ":";
        if (_text != null) _text.text = text;
    }
}
