﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu (fileName = "InputProxySO", menuName = "Game/Input Proxy")]
public class InputProxy : ScriptableObject, InputActions.IBaseGameplayActions {
    public event UnityAction<Vector2> aimEvent;
    public event UnityAction actionStartEvent;
    public event UnityAction actionEndEvent;

    private InputActions _inputActions;
    public void OnAction (InputAction.CallbackContext context) {
       // Debug.Log (context.phase);
        if (context.started)
            actionStartEvent?.Invoke ();
        else if (context.canceled)
            actionEndEvent?.Invoke ();
    }

    public void OnAim (InputAction.CallbackContext context) {
            aimEvent?.Invoke (context.ReadValue<Vector2> ());
    }

    private void OnEnable () {
        if (_inputActions == null) {
            _inputActions = new InputActions ();
            _inputActions.BaseGameplay.SetCallbacks (this);
        }
        _inputActions.BaseGameplay.Enable ();
    }
    private void OnDisable () {
        _inputActions.BaseGameplay.Disable ();

    }
}