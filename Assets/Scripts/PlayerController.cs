using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _anim;
    private Rigidbody _rb;
    [Header("Settings")]
    [SerializeField] private float _freeLookSpeed;
    [SerializeField] private float _movementSpeed;
    private Coroutine _LookAtCoroutine, _WalkToCoroutine;
    [HideInInspector] public bool isInteracting;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.Event_PlayerInput_FreeLook += OnFreeLook;
    }

    private void OnDisable()
    {
        InputManager.Event_PlayerInput_FreeLook -= OnFreeLook;
    }

    public void SetPlayerLookAt(Vector3 target, float duration)
    {
        if (_LookAtCoroutine != null) {
            StopCoroutine(_LookAtCoroutine);
        }
        _LookAtCoroutine = StartCoroutine(_LookAt(target, duration));
    }

    private IEnumerator _LookAt(Vector3 target, float duration)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);

        float t = 0f;
        while (t < duration) {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, t);
            t += Time.deltaTime;
            yield return null;
        }

        transform.LookAt(target);
    }

    public void Area7_3_LookAt()
    {
        SetPlayerLookAt(transform.position + new Vector3(0f, 0f, 1f), 1f);
    }

    public void WalkTo(Vector3 target, float speed, Action callback = null)
    {
        if (_WalkToCoroutine != null) {
            StopCoroutine(_WalkToCoroutine);
        }
        _WalkToCoroutine = StartCoroutine(_WalkTo(target, speed, callback));
    }

    private IEnumerator _WalkTo(Vector3 target, float speed, Action callback)
    {
        Vector3 start = transform.position;
        float duration = Vector3.Distance(start, target) / speed;
        transform.rotation = Quaternion.LookRotation(target - start, Vector3.up);

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.position = target;

        callback?.Invoke();
    }

    private void OnFreeLook(Vector2 val)
    {
        transform.eulerAngles += _freeLookSpeed * new Vector3(-val.y, val.x, 0f);
    }

    private void Update()
    {
        Vector2 movementInput = InputManager.Instance.GetDirectionalInputVector();
        if (movementInput == Vector2.zero) {
            _rb.velocity = Vector3.zero;
            return;
        }

        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir.y = 0f; // No vertical
        _rb.velocity = dir * _movementSpeed;
    }
}
