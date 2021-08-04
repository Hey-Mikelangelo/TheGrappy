using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionValue<TValue> where TValue : struct
{
    [SerializeField] private InputActionReference inputActionReference;

    public InputAction inputAction => inputActionReference.action;
    public bool started { get; private set; }
    public bool performed { get; private set; }
    public bool canceled { get; private set; }
    public TValue value { get; private set; }

    internal InputActionValue<TValue> Enable()
    {
        inputAction.Enable();
        return this;
    }

    internal InputActionValue<TValue> Disable()
    {
        inputAction.Disable();
        return this;
    }

    internal void SubscribeToContextChanges()
    {
        inputAction.performed += OnContextChanged;
        inputAction.started += OnContextChanged;
        inputAction.canceled += OnContextChanged;
    }

    internal void UnsubscribeToContextChanges()
    {
        inputAction.performed -= OnContextChanged;
        inputAction.started -= OnContextChanged;
        inputAction.canceled -= OnContextChanged;
    }

    internal void ResetOneFrameValues()
    {
        started = false;
        performed = false;
        canceled = false;
    }

    internal void OnContextChanged(InputAction.CallbackContext context)
    {
        started = context.started;
        performed = context.performed;
        canceled = context.canceled;
        value = context.ReadValue<TValue>(); 
    }

    public static implicit operator InputAction(InputActionValue<TValue> inputActionValue)
    {
        return inputActionValue.inputAction;
    }

    public static implicit operator TValue(InputActionValue<TValue> inputActionValue)
    {
        return inputActionValue.value;
    }
}
