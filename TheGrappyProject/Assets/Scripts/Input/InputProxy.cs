using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputProxySO", menuName = "Game/Input Proxy")]
public class InputProxy : ScriptableObject, InputActions.IBaseGameplayActions
{
    public event UnityAction<Vector2> aimEvent;
    public event UnityAction aimEndEvent;
    public event UnityAction actionStartEvent;
    public event UnityAction actionEndEvent;
    public event UnityAction abilityStartEvent;
    public event UnityAction abilityEndEvent;

    private InputActions _inputActions;
    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new InputActions();
            _inputActions.BaseGameplay.SetCallbacks(this);
        }
        _inputActions.BaseGameplay.Enable();
    }
    private void OnDisable()
    {
        _inputActions.BaseGameplay.Disable();

    }
    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.started)
            actionStartEvent?.Invoke();
        else if (context.canceled)
            actionEndEvent?.Invoke();
    }

    Vector2 prevAim;
    Vector2 zero = new Vector2(0, 0);
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if(value == zero && prevAim != zero)
        {
            aimEndEvent?.Invoke();
        }
        else
        {
            aimEvent?.Invoke(value);
        }
        prevAim = value;
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.started)
            abilityStartEvent?.Invoke();
        else if (context.canceled)
            abilityEndEvent?.Invoke();
    }
}