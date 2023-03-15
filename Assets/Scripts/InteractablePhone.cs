using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractablePhone : MonoBehaviour, IInteractable
{
    public enum OpenedApp
    {
        None,
        Map,
        Calculator   
    }
    private OpenedApp _currentApp;
    [SerializeField] private Transform _phone;
    [SerializeField] private Transform _map;
    [SerializeField] private Transform _calculator;
    private TextMesh _calculatorText;
    private Vector3 _originalPhonePos, _originalMapSize;
    private PlayerController _player;
    private bool _isPlaying;

    private void Awake()
    {
        _player = Camera.main.GetComponent<PlayerController>();
        _calculatorText = _calculator.GetComponentInChildren<TextMesh>();

        _originalPhonePos = _phone.localPosition;
        _originalMapSize = _map.localScale;
        _currentApp = OpenedApp.None;
        _isPlaying = false;
    }

    public void Begin()
    {
        if (_player.isInteracting || _isPlaying) return;

        InputManager.Instance.EnablePlayerInput(false);
        _map.gameObject.SetActive(true);
        StartCoroutine(_Begin());
    }

    private IEnumerator _Begin()
    {
        Vector3 zoomScale = Vector3.one * 12f;
        _player.isInteracting = true;
        _isPlaying = true;

        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _map.localScale = Vector3.Lerp(_originalMapSize, zoomScale, t);
            yield return null;
        }

        _map.localScale = zoomScale;

        yield return new WaitForSeconds(2f);

        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.75f) {
            _map.localScale = Vector3.Lerp(zoomScale, _originalMapSize, t);
            yield return null;
        }

        _map.localScale = _originalMapSize;

        InputManager.Instance.EnablePlayerInput(true);
        // _conversation.afterAllDialogues?.Invoke();
        _player.isInteracting = false;
        _isPlaying = false;
    }

    public void RaisePhone(int app)
    {
        SetApp((OpenedApp)app);
        Vector3 raisedPos = _originalPhonePos;
        raisedPos.y = -0.05f;
        StartCoroutine(_MovePhone(_originalPhonePos, raisedPos, 0.75f));
    }

    private IEnumerator _MovePhone(Vector3 from, Vector3 to, float duration)
    {
        _player.isInteracting = true;
        _isPlaying = true;
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            _phone.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }
        _phone.localPosition = to;
        _isPlaying = false;
        _player.isInteracting = false;
    }

    public void HidePhone()
    {
        StartCoroutine(_MovePhone(_phone.localPosition, _originalPhonePos, 0.75f));
    }

    private void SetApp(OpenedApp app)
    {
        _currentApp = app;
        switch (app)
        {
            case OpenedApp.None: {
                _map.gameObject.SetActive(false);
                _calculator.gameObject.SetActive(false);
                break;
            }
            case OpenedApp.Map: {
                _map.gameObject.SetActive(true);
                _calculator.gameObject.SetActive(false);
                break;
            }
            case OpenedApp.Calculator: {
                _map.gameObject.SetActive(false);
                _calculator.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void SetCalculatorText(string text)
    {
        if (_currentApp == OpenedApp.Calculator) {
            _calculatorText.text = text;
        }
    }
}
