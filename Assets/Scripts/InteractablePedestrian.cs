using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Conversation))]
public class InteractablePedestrian : Character, IInteractable
{
    [SerializeField] private LinePath _path;
    [SerializeField] private SpawnPoint _interactionPoint;
    [SerializeField] private SpawnPoint _interactionPointForPlayer;
    // [SerializeField] private bool shouldDestroyAfterFinish;
    protected Conversation _conversation;
    protected PlayerController _player;
    private bool _isInteracting, _isPlayer, _playerShouldLookAtPed;
    private Coroutine _patrolCoroutine;

    private void Awake()
    {
        _conversation = GetComponent<Conversation>();
        _player = Camera.main.GetComponent<PlayerController>();
        _isInteracting = _playerShouldLookAtPed = false;

        _isPlayer = transform.parent == _player.transform;
        if (!_isPlayer && _path.IsValid()) {
            _patrolCoroutine = StartCoroutine(Move(_path.startPoint.point.transform.position, _path.endPoint.point.transform.position, 1.3f, ResetPatrol));
        }
    }

    private void ResetPatrol()
    {
        if (_isPlayer) return;

        transform.position = _path.startPoint.point.transform.position;
        _patrolCoroutine = StartCoroutine(Move(_path.startPoint.point.transform.position, _path.endPoint.point.transform.position, 1.3f, ResetPatrol));
    }

    public void Begin()
    {
        if (_player.isInteracting || _isInteracting) return;

        _isInteracting = true;
        _playerShouldLookAtPed = true;
        _player.isInteracting = true;
        if (_patrolCoroutine != null) StopAllCoroutines();
        // animator.SetBool("Walk", false);
        // animator.SetBool("Run", false);
        InputManager.Instance.EnablePlayerInput(false);

        if (!_isPlayer && _interactionPoint.IsValid()) {
            StartCoroutine(Move(transform.position, _interactionPoint.point.transform.position, 1.3f, PlayerWalk));
            // Vector3 target = transform.position + (_player.transform.position - transform.position).normalized * 2;
            // _player.WalkTo(new Vector3(target.x, _player.transform.position.y, target.z), 2.75f, StartConversation);
        } else {
            StartConversation();
        }
    }

    private void PlayerWalk()
    {
        _playerShouldLookAtPed = false;
        StartCoroutine(_LookAt(_player.transform.position, 0.4f));
        if (_interactionPointForPlayer.IsValid()) {
            _player.WalkTo(new Vector3(_interactionPointForPlayer.point.transform.position.x, _player.transform.position.y, _interactionPointForPlayer.point.transform.position.z), 2.75f, PlayerArrive);
        }
    }

    private void PlayerArrive()
    {
        StartCoroutine(_LookAt(_player.transform.position, 0.4f));
        _player.SetPlayerLookAt(new Vector3(transform.position.x, _player.transform.position.y, transform.position.z), 0.4f);
        StartConversation();
    }

    private void StartConversation()
    {
        StartCoroutine(_conversation.StartConversation());
        if (!_isPlayer) {
            StartCoroutine(_LookAt(_player.transform.position, 0.4f));
            _player.SetPlayerLookAt(new Vector3(transform.position.x, _player.transform.position.y, transform.position.z), 0.4f);
        }

        _conversation.afterAllDialogues += ContinueMove;
    }

    private void Update()
    {
        if (_playerShouldLookAtPed) {
            _player.transform.LookAt(new Vector3(transform.position.x, _player.transform.position.y, transform.position.z));
        }
    }

    private void ContinueMove()
    {
        InputManager.Instance.EnablePlayerInput(true);
        _isInteracting = false;
        _player.isInteracting = false;

        if (!_isPlayer && _path.IsValid()) {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            _patrolCoroutine = StartCoroutine(Move(transform.position, _path.endPoint.point.transform.position, 1.3f, ResetPatrol));
        }

        if (_isPlayer) {
            Destroy(this);
        }
    }

    protected IEnumerator _LookAt(Vector3 target, float speed)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target - new Vector3(transform.position.x, target.y, transform.position.z));

        float t = 0f;
        while (t < 1f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
    }
}
