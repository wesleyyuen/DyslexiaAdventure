using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, InputMaster.IPlayerActions
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get  { return _instance; }
    }
    private InputMaster _input;
    private bool isPlayerControlEnabled;
    public bool IsPlayerControlEnabled { get {return isPlayerControlEnabled;} }

    private void Awake()
    {
        // Singleton Pattern
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        // Initialize InputActionAsset
        if (_input == null) {
            _input = new InputMaster();

            _input.Player.SetCallbacks(this);
        }

        // Enable Action maps
        EnablePlayerInput(true);
    }

    public void EnablePlayerInput(bool enable)
    {
        if (enable) {
            _input.Player.Enable();
        }
        else {
            _input.Player.Disable();
        }
        isPlayerControlEnabled = enable;
    }


    public static event Action<Vector2> Event_PlayerInput_FreeLook;
    public void OnFreeLook(InputAction.CallbackContext context)
    {
        Event_PlayerInput_FreeLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
    }
    public Vector2 GetDirectionalInputVector()
    {
        return _input.Player.Movement.ReadValue<Vector2>();
    }
}
