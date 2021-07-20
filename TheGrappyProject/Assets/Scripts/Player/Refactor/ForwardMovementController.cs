using UnityEngine;

public class ForwardMovementController : MovementController
{
    [SerializeField] private Vector3 offsetToMatchForward;
    public override void Enable()
    {
        base.Enable();
    }

    public override void RunUpdate()
    {
        base.RunUpdate();
        SetPlayerVelocity(GetPlayerForwardDirection() * movementSpeed);
    }

    private Vector3 GetPlayerForwardDirection()
    {
        return player.PlayerTransform.up + offsetToMatchForward;

    }
}


