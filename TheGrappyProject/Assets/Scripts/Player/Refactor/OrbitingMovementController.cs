using UnityEngine;

public class OrbitingMovementController : MovementController
{
    [SerializeField]
    private AimControllersManager aimControllersManager;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float maxRotationSpeed;
    [SerializeField, Range(0, 5)]
    private float minRopeLength;
    [SerializeField, Range(1, 10)]
    private float pullSpeedMultiplier;

    private float pullSpeed => pullSpeedMultiplier * movementSpeed;
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
        SetGrapLineEnabled(false);
    }

    public override void RunFixedUpdate()
    {
        Vector2 playerPosition = GetPlayerPosition();
        Vector2 pivotPosition = GetRotationPivotPosition();
        Vector2 playerForwardDirection = GetPlayerForwardDirection();

        if (player.InputValues.MainAction.IsHeld)
        {
            ropeLenght -= pullSpeed * Time.deltaTime;

            if (ropeLenght < minRopeLength)
            {
                ropeLenght = minRopeLength;
            }
        }

        Vector2 ropeConstraintVelocity = new Vector2(0, 0);

        Vector2 forwardVelocity = playerForwardDirection.normalized * movementSpeed;
        Vector2 playerNextPosition = GetPredictedPosition(forwardVelocity);

        float nextDistanceToPivot = Vector3.Distance(playerNextPosition, pivotPosition);
        if (nextDistanceToPivot > ropeLenght)
        {
            float exceededDistanceToRope = nextDistanceToPivot - ropeLenght;
            Vector2 nextDirectionToPivot = MathUtilities2D.GetDirectionFromTo(playerNextPosition, pivotPosition);
            ropeConstraintVelocity = nextDirectionToPivot.normalized * exceededDistanceToRope / Time.deltaTime;
        }

        Vector2 newPlayerVelocity = forwardVelocity + ropeConstraintVelocity;

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

        RotatePlayer(newPlayerRotationAngle, maxRotationSpeed);
        SetPlayerVelocity(newPlayerVelocity);

        SetGrapLineEnabled(true);
        DrawGrapLine(playerNextPosition, pivotPosition);
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
        return aimControllersManager.ActiveController.AimPointPosition;
    }

    private void SetGrapLineEnabled(bool value)
    {
        if (lineRenderer.enabled != value)
        {
            lineRenderer.enabled = value;
        }
    }

    private void DrawGrapLine(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}


