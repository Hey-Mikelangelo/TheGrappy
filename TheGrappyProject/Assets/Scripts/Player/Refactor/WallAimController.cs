using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class WallAimController : AimController
{
    [SerializeField] private float maxGrapLength;
    [SerializeField] private LayerMask wallLayerMask;

    [Space(8)]
    [SerializeField] private Transform aimPointVisualsTransform;
    [SerializeField] private SpriteRenderer aimPointRenderer;

    [Space(8)]
    [SerializeField] private Transform aimDirectionCircleHolderTransform;
    [SerializeField] private SpriteRenderer aimDirectionCircleRenderer;

    [Space(8)]
    [SerializeField] private Transform laternLightTransform;
    [SerializeField] private Light2D laternLight;

    [Space(8)]
    [SerializeField, Range(0, 1)] private float aimPointSpeed;
    [SerializeField, Range(0, 1)] private float aimDirectionCircleSpeed;
    [SerializeField, Range(0, 1)] private float laternLightSpeed;
    [SerializeField] private float circleCastSize = 1;

    private RaycastHit2D[] hits = new RaycastHit2D[1];
    private Vector2 aimDelta = new Vector2(0, 0);
    public override void Enable()
    {
        base.Enable();
        aimPointRenderer.enabled = true;
        aimDirectionCircleRenderer.enabled = true;
        laternLight.enabled = true;
    }

    public override void Disable()
    {
        base.Disable();
        aimPointRenderer.enabled = false;
        aimDirectionCircleRenderer.enabled = false;
        laternLight.enabled = false;
    }

    private Vector2 GetWorldAimDirection(Transform playerTransform, Vector2 aimDelta)
    {
        float playerAngle = Mathf.Atan2(playerTransform.right.y, playerTransform.right.x) * Mathf.Rad2Deg;
        float aimAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = aimAngle + playerAngle;
        Vector2 aimDeltaWorldDirection = new Vector2(math.cos(math.radians(angle)), math.sin(math.radians(angle)));
        return aimDeltaWorldDirection;
    }

    public override void RunUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        Transform playerTransform = player.PlayerTransform;
        if (player.InputValues.AimDelta != Vector2.zero)
        {
            aimDelta = player.InputValues.AimDelta;
        }
        Tilemap wallTilemap = player.MapGenerator.wallTilemap;

        Vector2 worldAimDeltaDirection = GetWorldAimDirection(playerTransform, aimDelta);
        //Debug.DrawLine(_playerTransform.position, dir);
        int hitCount = Physics2D.CircleCastNonAlloc(
            playerTransform.position, circleCastSize, worldAimDeltaDirection, hits, maxGrapLength, wallLayerMask);

        if (hitCount != 0)
        {
            Vector2 aimPointPosition = hits[0].point;
            Vector3 grapTileWorldPos = aimPointPosition - new Vector2(hits[0].normal.x, hits[0].normal.y);
            Vector3Int aimPointCellPosition = wallTilemap.WorldToCell(grapTileWorldPos);
            //Debug.DrawLine(hits[0].point, aimPointPosition + new Vector2(hits[0].normal.x, hits[0].normal.y));

            if (MapGenerator.CheckForTile(wallTilemap, aimPointCellPosition))
            {
                AimPointPosition = aimPointPosition;
                HasTarget = true;
            }
            else
            {
                HasTarget = false;
            }
        }
        else
        {
            if(IsAimPointTooFar())
            {
                HasTarget = false;
            }
        }

        HandleVisuals();
    }

    private bool IsAimPointTooFar() => Vector3.Distance(AimPointPosition, player.PlayerTransform.position) > maxGrapLength;

    private void HandleVisuals()
    {
        RotateAimVisualsToAimPoint();
        RotateLaternToJoystick();

        if (HasTarget)
        {
            SetAimPointVisualsEnabled(true);
            SetAimPointVisualsPosition();
        }
        else
        {
            SetAimPointVisualsEnabled(false);
        }
    }

    private void RotateAimVisualsToAimPoint()
    {
        MathUtilities2D.RotateTransformToPointWithSpeed(
            aimDirectionCircleHolderTransform, player.PlayerTransform.position, AimPointPosition, aimDirectionCircleSpeed);
    }

    private void RotateLaternToJoystick()
    {
        float playerAngle = Mathf.Atan2(player.PlayerTransform.right.y, player.PlayerTransform.right.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = deltaAngle + playerAngle;
        MathUtilities2D.RotateTransformToAngleWithSpeed(laternLightTransform, angle, laternLightSpeed);
    }

    private void SetAimPointVisualsPosition()
    {
        Vector3 targetPosition = AimPointPosition;
        float distanceToTargetPosition = Vector3.Distance(aimPointVisualsTransform.position, targetPosition);
        aimPointVisualsTransform.position = Vector3.MoveTowards(
            aimPointVisualsTransform.position, targetPosition, distanceToTargetPosition * aimPointSpeed);
    }

    private void SetAimPointVisualsEnabled(bool value)
    {
        aimPointRenderer.enabled = value;
    }
}
