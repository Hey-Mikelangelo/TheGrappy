using UnityEngine;

public class ForwardMovementController : MovementController
{   
    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        AddPlayerVelocity(GetPlayerForwardDirection() * MovementSpeed);
    }
}


