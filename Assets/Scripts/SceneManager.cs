using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider), typeof(EventTrigger))]
public class SceneManager : MonoBehaviour
{
    [SerializeField] private bool shouldHide;
    [SerializeField] private Vector3 facingAfterTransit;
    [SerializeField] private Vector3 offsetAfterTransit = Vector3.zero;

    private Transform _player;
    private MeshRenderer _renderer;
    private Fader _fader;
    private bool _isChanging;

    private void Awake()
    {
        _isChanging = false;
        _player = Camera.main.transform;
        _renderer = GetComponentInChildren<MeshRenderer>();
        _fader = GameObject.FindGameObjectWithTag("Fader")?.GetComponent<Fader>();

        _renderer.enabled = !shouldHide;
    }

    public void ChangeSphere(Transform nextSphere)
    {
        if (InputManager.Instance.IsPlayerControlEnabled && !_isChanging) {
            _isChanging = true;

            StopAllCoroutines();
            _renderer.enabled = !shouldHide;

            _fader.FadeInAnOut(0.15f, () => {
                _player.SetParent(nextSphere, false);
                _player.localPosition = offsetAfterTransit;
                _player.localEulerAngles = facingAfterTransit;

                // Show arrows in next area after delay
                SceneManager[] arrows = nextSphere.GetComponentsInChildren<SceneManager>();
                foreach (SceneManager arrow in arrows) {
                    arrow.ShowArrowAfterDelay(10f);
                }
            }, () => {
                if (nextSphere.TryGetComponent<ConversationSequence>(out ConversationSequence sequence)) {
                    sequence.StartSequence();
                }

                _isChanging = false;
            });
        }
    }

    public void Replay()
    {
        _fader.FadeInAnOut(0.15f, () => {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        });
    }

    public void ShowArrowAfterDelay(float delay)
    {
        if (!shouldHide) {
            _renderer.enabled = true;
            return;
        }
        
        StopAllCoroutines();
        StartCoroutine(_ShowArrowAfterDelay(delay));
    }

    private IEnumerator _ShowArrowAfterDelay(float delay)
    {
        _renderer.enabled = false;

        yield return new WaitForSeconds(delay);

        _renderer.enabled = true;
    }
}
