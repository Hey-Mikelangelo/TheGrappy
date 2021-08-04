using UnityEngine;

public abstract class MovementController : PlayerModuleController
{
    public float MovementSpeed { get; set; }

    private Vector2 Vector2Zero = new Vector2(0, 0);

    public override void Disable()
    {
        base.Disable();
        player.PlayerRigidbody2D.velocity = Vector2.zero;
    }

    public override void RunLateFixedUpdate()
    {
        base.RunLateFixedUpdate();
        SetPlayerVelocity(Vector2Zero);
    }

    public float GetPlayerRotation(bool normalizedTo360 = true)
    {
        float angle = player.PlayerRigidbody2D.rotation;
        if (normalizedTo360)
        {
            angle = MathUtilities2D.NormalizeAngleTo360(angle);
        }
        return angle;
    }

    public Vector2 GetPlayerPosition()
    {
        return player.PlayerRigidbody2D.position;
    }

    public Vector3 GetPlayerForwardDirection()
    {
        return player.PlayerTransform.up;
    }

    public Vector3 GetPlayerRightDirection()
    {
        return player.PlayerTransform.right;
    }

    public Vector2 GetPlayerVelocity()
    {
        return player.PlayerRigidbody2D.velocity;
    }

    public Vector2 GetPredictedPosition(Vector2 velocity)
    {
        return GetPlayerPosition() + (velocity * Time.deltaTime);
    }

    public void AddPlayerVelocity(Vector2 velocity)
    {
        Vector2 resultingVecocity = GetPlayerVelocity() + velocity;
        player.PlayerRigidbody2D.velocity = resultingVecocity;
    }

    public void SetPlayerRotation(float angle)
    {
        player.PlayerRigidbody2D.SetRotation(angle);
    }
    
    public float RotatePlayer(float targetAngle)
    {
        SetPlayerRotation(targetAngle);
        return targetAngle;
    }

    public float RotatePlayer(float targetAngle, float anglesPerSec)
    {
        float currentAngle = GetPlayerRotation();
        float newRotation = MathUtilities2D.GetRotationTowards(
            currentAngle, targetAngle, anglesPerSec * 0.02f);

        SetPlayerRotation(newRotation);

        return newRotation;
    }

    private void SetPlayerVelocity(Vector2 velocity)
    {
        player.PlayerRigidbody2D.velocity = velocity;
    }


}


