using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class inputTest : MonoBehaviour
{
    [SerializeField] private InputAction fireAction;

    [Button]
    void Register()
    {
        InputSystem.RegisterInteraction<MyWiggleInteraction>();
    }
    private void OnEnable()
    {

        fireAction.Enable();
        fireAction.started +=
    context =>
    {
        //Debug.Log("Started");
        if (context.interaction is SlowTapInteraction)
            Debug.Log("Started SlowTapInteraction");
        else
        {
            Debug.Log("Started tapInteraction");
        }
    };

        fireAction.performed +=
            context =>
            {
                if (context.interaction is SlowTapInteraction)
                    Debug.Log("ChargedFire();");
                else
                    Debug.Log("Fire();");
            };

        fireAction.canceled +=
            _ => Debug.Log("HideChargingUI();");

    }

    private void Update()
    {
        if (fireAction.triggered)
        {

        }
    }
}


public class MyWiggleInteraction : IInputInteraction
{
    public float duration = 0.2f;
    void IInputInteraction.Process(ref InputInteractionContext context)
    {
        if (context.timerHasExpired)
        {
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if (context.ReadValue<float>() == 1)
                {
                    context.Started();
                    context.SetTimeout(duration);
                }
                break;

            case InputActionPhase.Started:
                if (context.ReadValue<float>() == -1)
                    context.Performed();
                break;
        }
    }

    // Unlike processors, Interactions can be stateful, meaning that you can keep a
    // local state that mutates over time as input is received. The system might
    // invoke the Reset() method to ask Interactions to reset to the local state
    // at certain points.
    void IInputInteraction.Reset()
    {
    }
}
