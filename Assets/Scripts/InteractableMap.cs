using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Conversation))]
public class InteractableMap : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _lookAt;
    private Image _image;
    private PlayerController _player;
    private Conversation _conversation;
    private bool _isPlaying;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _conversation = GetComponent<Conversation>();
        _player = Camera.main.GetComponent<PlayerController>();

        _image.rectTransform.localScale = Vector3.zero;
        _isPlaying = false;
    }

    public void Begin()
    {
        if (_player.isInteracting || _isPlaying) return;

        InputManager.Instance.EnablePlayerInput(false);
        _player.SetPlayerLookAt(_lookAt.position, 0.5f);
        StartCoroutine(_FadeImage());
    }

    private IEnumerator _FadeImage()
    {
        _player.isInteracting = true;
        _isPlaying = true;

        yield return new WaitForSeconds(1f);

        for (float t = 0f; t < 1f; t += Time.deltaTime / 1f) {
            _image.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        _image.rectTransform.localScale = Vector3.one;

        yield return new WaitForSeconds(5);

        for (float t = 0f; t < 1f; t += Time.deltaTime / 1f) {
            _image.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }

        _image.rectTransform.localScale = Vector3.zero;

        InputManager.Instance.EnablePlayerInput(true);
        // _conversation.afterAllDialogues?.Invoke();
        _player.isInteracting = false;
        _isPlaying = false;
    }
}
