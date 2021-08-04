using UnityEngine;

public class OrbitingMovementController : MovementController
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float maxRotationSpeed = 150;
    [SerializeField, Range(0, 5)]
    private float minRopeLength = 1;
    [SerializeField, Range(1, 10)]
    private float pullSpeedMultiplier = 2;

    private float pullSpeed => pullSpeedMultiplier * MovementSpeed;
    private float ropeLenght;

    public override void Enable()
    {
        base.Enable();
        Vector2 playerPosition = GetPlayerPosition();
        Vector2 pivotPosition = GetRotationPivotPosition();
        ropeLenght = Vector3.Distance(playerPosition, pivotPosition);
    }

    public override void Disable()
    {
        base.Disable();
        SetRopeEnabled(false);
    }

    public override void RunFixedUpdate()
    {
        Vector2 playerPosition = GetPlayerPosition();
        Vector2 pivotPosition = GetRotationPivotPosition();
        Vector2 playerForwardDirection = GetPlayerForwardDirection();

        //shorten the rope 
        if (player.InputValues.MainAction.IsHeld)
        {
            ropeLenght -= pullSpeed * Time.deltaTime;

            if (ropeLenght < minRopeLength)
            {
                ropeLenght = minRopeLength;
            }
        }

        //calculate forward velocity
        Vector2 forwardVelocity = playerForwardDirection.normalized * MovementSpeed;

        //calculate rope constraint force(velocity)
        Vector2 ropeConstraintVelocity = new Vector2(0, 0);
        Vector2 playerNextPositionOnlyForward = GetPredictedPosition(forwardVelocity);
        float nextDistanceToPivot = Vector3.Distance(playerNextPositionOnlyForward, pivotPosition);

        if (nextDistanceToPivot > ropeLenght)
        {
            float exceededDistanceToRope = nextDistanceToPivot - ropeLenght;
            Vector2 nextDirectionToPivot = MathUtilities2D.GetDirectionFromTo(
                playerNextPositionOnlyForward, pivotPosition);
            ropeConstraintVelocity = nextDirectionToPivot.normalized * exceededDistanceToRope / Time.deltaTime;
        }

        Vector2 finalNewVelocity = forwardVelocity + ropeConstraintVelocity;

        //calculate player rotation
        float newPlayerRotationAngle;
        Vector2 directionToPivot = MathUtilities2D.GetDirectionFromTo(playerPosition, pivotPosition);
        if (Vector3.Dot(playerForwardDirection, directionToPivot) <= 0)
        {
            bool rotateClockwise = IsRotateClockwise(playerPosition, pivotPosition, GetPlayerRightDirection());
            newPlayerRotationAngle = GetAngleToAlignGrap(playerPosition, pivotPosition, rotateClockwise);
        }
        else
        {
            newPlayerRotationAngle = GetPlayerRotation();
        }

        //assing values
        RotatePlayer(newPlayerRotationAngle, maxRotationSpeed);
        AddPlayerVelocity(finalNewVelocity);

        //draw rope
        SetRopeEnabled(true);
        Vector2 playerNextPosition = GetPredictedPosition(finalNewVelocity);
        DrawRope(playerNextPosition, pivotPosition);
    }

    private bool IsRotateClockwise(Vector2 playerPosition, Vector2 rotationPivotPosition, Vector2 playerRightDirection)
    {
        Vector2 directionToPivot = MathUtilities2D.GetDirectionFromTo(playerPosition, rotationPivotPosition);
        bool rotateClockwise = Vector3.Dot(playerRightDirection, directionToPivot) < 0;

        return rotateClockwise;
    }

    private float GetAngleToAlignGrap(
        Vector2 playerPosition, Vector2 rotationPivotPosition, bool rotateClockwise)
    {
        Vector2 directionToRotationPivot = MathUtilities2D.GetDirectionFromTo(playerPosition, rotationPivotPosition);
        Vector2 normalDirectionToRotationPivot = MathUtilities2D.GetNormal(directionToRotationPivot);

        if (rotateClockwise)
        {
            normalDirectionToRotationPivot = -normalDirectionToRotationPivot;
        }

        float angleToAlignMovementDirection = MathUtilities2D.GetAngleToAlignDirection(normalDirectionToRotationPivot);
        angleToAlignMovementDirection += 90;
        angleToAlignMovementDirection = MathUtilities2D.NormalizeAngleTo360(angleToAlignMovementDirection);
        return angleToAlignMovementDirection;
    }

    private Vector2 GetRotationPivotPosition()
    {
        return player.AimControllersManager.ActiveController.AimPointPosition;
    }

    private void SetRopeEnabled(bool value)
    {
        if (lineRenderer.enabled != value)
        {
            lineRenderer.enabled = value;
        }
    }

    private void DrawRope(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}


