using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SimpleAimController : AimController
{
    [SerializeField] private Transform laternLightTransform;
    [SerializeField] private Light2D laternLight;
    [SerializeField, Range(0, 1)] private float laternLightSpeed;

    public override void Enable()
    {
        base.Enable();
        laternLight.enabled = true;
    }

    public override void Disable()
    {
        base.Disable();
        laternLight.enabled = false;
    }

    public override void RunUpdate()
    {
        base.RunUpdate();
        if (player.InputValues.AimDelta != Vector2.zero)
        {
            AimUIDirection = player.InputValues.AimDelta;
        }
        RotateLaternToJoystick();
    }

    private void RotateLaternToJoystick()
    {
        float playerAngle = Mathf.Atan2(player.PlayerTransform.right.y, player.PlayerTransform.right.x) * Mathf.Rad2Deg;
        float aimUIAngle = Mathf.Atan2(AimUIDirection.y, AimUIDirection.x) * Mathf.Rad2Deg;
        float angle = aimUIAngle + playerAngle;
        MathUtilities2D.RotateTransformToAngleWithSpeed(laternLightTransform, angle, laternLightSpeed);
    }
}
