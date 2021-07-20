using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MovementController : PlayerModuleController
{
    [SerializeField] protected float movementSpeed;

    public override void RunUpdate(){}

    public override void Disable()
    {
        base.Disable();
        player.PlayerRigidbody2D.velocity = Vector2.zero;
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

    public void SetPlayerVelocity(Vector2 velocity)
    {
        Vector2 newVelocity = velocity;
        player.PlayerRigidbody2D.velocity = newVelocity;
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



}


