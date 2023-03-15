using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeIntro : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private SceneManager _scene;
    [SerializeField] private AudioClip _introAudio;
    private AudioSource _player;
    private Image _bg;
    private Text _text;
    private bool _introed, _isPlaying;

    private void Awake()
    {
        _player = Camera.main.GetComponentInChildren<AudioSource>();
        _bg = _canvas.GetComponentInChildren<Image>();
        _text = _canvas.GetComponentInChildren<Text>();

        _bg.rectTransform.localScale = Vector3.zero;
        _text.rectTransform.localScale = Vector3.zero;
        _introed = false;
        _isPlaying = false;
    }
    
    public void StartIntroSequence(Transform area)
    {
        if (_isPlaying) return;

        if (_introed) {
            _scene.ChangeSphere(area);
        } else {
            InputManager.Instance.EnablePlayerInput(false);
            StartCoroutine(Sequence(area));
        }
    }

    private IEnumerator Sequence(Transform area)
    {
        _isPlaying = true;
        _introed = true;

        for (float t = 0f; t < 1f; t += Time.deltaTime / 1f) {
            _bg.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        _bg.rectTransform.localScale = Vector3.one;

        _player.clip = _introAudio;
        _player.Play();

        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _text.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        _text.rectTransform.localScale = Vector3.one;

        yield return new WaitUntil(() => !_player.isPlaying);

        for (float t = 0f; t < 1f; t += Time.deltaTime / 1f) {
            _bg.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }

        _bg.rectTransform.localScale = Vector3.zero;

        InputManager.Instance.EnablePlayerInput(true);
        _scene.ChangeSphere(area);
        _isPlaying = false;
    }
}
