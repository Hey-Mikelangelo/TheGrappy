using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputEventsSO", menuName = "Game/Input/Input Events")]
public class InputEventsSO : ScriptableObject, InputActions.IBaseGameplayActions
{
    public event UnityAction<Vector2> onAim;
    public event UnityAction onAimEnd;
    public event UnityAction onMainAction;
    public event UnityAction onMainActionEnd;
    public event UnityAction onMainActionHold;
    public event UnityAction onMainActionHoldEnd;

    private InputActions inputActions;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputActions();
            inputActions.BaseGameplay.SetCallbacks(this);
        }
        inputActions.BaseGameplay.Enable();
    }

    private void OnDisable()
    {
        inputActions.BaseGameplay.Disable();
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.started)
            onMainAction?.Invoke();
        else if (context.canceled)
            onMainActionEnd?.Invoke();
    }


    Vector2 prevAim;
    Vector2 zero = new Vector2(0, 0);
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (value == zero && prevAim != zero)
        {
            onAimEnd?.Invoke();
        }
        else
        {
            onAim?.Invoke(value);
        }
        prevAim = value;
    }

    public void OnActionHold(InputAction.CallbackContext context)
    {
        if (context.started)
            onMainActionHold?.Invoke();
        else if (context.canceled)
            onMainActionHoldEnd?.Invoke();
    }
}