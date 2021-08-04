using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "InputEventsToValuesLinker", menuName = "Game/Input/InputEventsToValuesLinker")]
public class InputEventsToValuesLinkerSO : ScriptableObject
{
    [SerializeField] private InputEventsSO inputEventsSO;
    [SerializeField] private InputValuesSO inputValuesSO;

    private void OnEnable()
    {
        if(inputEventsSO != null)
        {
            inputEventsSO.onAim += InputEventsSO_onAim;
            inputEventsSO.onAimEnd += InputEventsSO_onAimEnd;
            inputEventsSO.onMainAction += InputEventsSO_onMainAction;
            inputEventsSO.onMainActionEnd += InputEventsSO_onMainActionEnd;
            inputEventsSO.onMainActionHold += InputEventsSO_onMainActionHold;
            inputEventsSO.onMainActionHoldEnd += InputEventsSO_onMainActionHoldEnd;
        }
      
        InputSystem.onBeforeUpdate += OnBeforeUpdate;
    }

    private void OnDisable()
    {
        if (inputEventsSO != null)
        {
            inputEventsSO.onAim -= InputEventsSO_onAim;
            inputEventsSO.onAimEnd -= InputEventsSO_onAimEnd;
            inputEventsSO.onMainAction -= InputEventsSO_onMainAction;
            inputEventsSO.onMainActionEnd -= InputEventsSO_onMainActionEnd;
            inputEventsSO.onMainActionHold -= InputEventsSO_onMainActionHold;
            inputEventsSO.onMainActionHoldEnd -= InputEventsSO_onMainActionHoldEnd;
        }
     
        InputSystem.onBeforeUpdate -= OnBeforeUpdate;
    }

    private void OnBeforeUpdate()
    {
        if(InputState.currentUpdateType == InputUpdateType.Editor)
        {
            return;
        }
        ResetOneFrameValues();
    }

    private void ResetOneFrameValues()
    {
        inputValuesSO.MainAction.WasPressed = false;
        inputValuesSO.MainAction.WasReleased = false;
        inputValuesSO.AimDelta.WasReleased = false;
    }

    private void InputEventsSO_onMainAction()
    {
        inputValuesSO.MainAction.Value = true;
        inputValuesSO.MainAction.WasPressed = true;
        inputValuesSO.MainAction.IsHeld = true;
    }

    private void InputEventsSO_onMainActionEnd()
    {
        inputValuesSO.MainAction.Value = false;
        inputValuesSO.MainAction.WasReleased = true;
        inputValuesSO.MainAction.IsHeld = false;
    }
    private void InputEventsSO_onAim(Vector2 value)
    {
        inputValuesSO.AimDelta.Value = value;
    }

    private void InputEventsSO_onAimEnd()
    {
        inputValuesSO.AimDelta.Value = new Vector2(0, 0);
        inputValuesSO.AimDelta.WasReleased = true;
    }
    private void InputEventsSO_onMainActionHold()
    {
        //inputValuesSO.MainAction.IsHeld = true;
    }

    private void InputEventsSO_onMainActionHoldEnd()
    {
        //inputValuesSO.MainAction.IsHeld = false;
    }
}


