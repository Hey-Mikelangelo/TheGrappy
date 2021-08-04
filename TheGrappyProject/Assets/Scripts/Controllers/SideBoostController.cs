using UnityEngine;

public class SideBoostController : AbilityController
{
    [SerializeField] private float maxBoostTime;
    [SerializeField] private float timeToFullForce = 1;
    [SerializeField] private float force;
    
    private bool toRight;
    [SerializeField] private float timeElapsed = 0;
    [SerializeField] private float accelerationProgress = 0;
    public override void Enable()
    {
        base.Enable();
        timeElapsed = 0;
        accelerationProgress = 0;
        bool isDirectionRight = player.AimControllersManager.ActiveController.AimUIDirection.x > 0;
        SetDirection(isDirectionRight);
    }
    private void SetDirection(bool right)
    {
        this.toRight = right;
    }

    public bool HasLeftTime()
    {
        return (timeElapsed < maxBoostTime);
    }
   
    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        if (timeElapsed < maxBoostTime)
        {
            timeElapsed += Time.deltaTime;
            accelerationProgress += Time.deltaTime * 1 / timeToFullForce;
            accelerationProgress = Mathf.Clamp01(accelerationProgress);

            MovementController movementController = player.MovementControllersManager.ActiveController;
            float forceMagnitude = GetSideForceMagnitude();
            Vector2 directionRight = movementController.GetPlayerRightDirection();
            Vector2 sideBoostVelocity = ((toRight ? forceMagnitude : -forceMagnitude) * directionRight);
            movementController.AddPlayerVelocity(sideBoostVelocity);
        }
        else
        {
            DisableThisController();
        }
    }

    private float GetSideForceMagnitude()
    {
        return Mathf.Lerp(0, force, accelerationProgress);
    }
}

