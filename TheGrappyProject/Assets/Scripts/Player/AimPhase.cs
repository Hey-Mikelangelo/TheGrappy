using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class AimPhase : MonoBehaviour
{
    public Transform aimArrowHolder;
    //public LineRenderer lineRenderer;
    public Light2D laternLight;
    //public Color aimMatchColor;
    public Transform aimPoint;
    public Gradient aimMatchGradient;
    //private Gradient _initialGradient;
    //private Color _initialColor;
    private float _grapLength;
    private LayerMask _grapPointsMask;
    private float _speed;
    private RaycastHit2D[] hit = new RaycastHit2D[1];
    private Vector3 _grapPos;
    private Vector3Int _grapTilePos;
    private float _aimAngle;
    private PlayerBehaviour playerBehaviour;
    private PlayerVarsSO playerVars;
    private Transform _playerTransform;
    public MapGenerator mapGenerator;

    private Tilemap _wallTilemap;

    //should be called once to setup variables 
    public void Setup(Transform playerTransform, float grapLength, LayerMask grapPointsMask, float speed)
    {
        _playerTransform = playerTransform;
        _grapLength = grapLength;
        _grapPointsMask = grapPointsMask;
        _speed = speed;
        _grapPos = Vector3.zero;
        playerBehaviour = GetComponent<PlayerBehaviour>();
        playerVars = playerBehaviour.linker.playerVars;
        _wallTilemap = playerBehaviour.mapGenerator.wallTilemap;
        playerVars.hasGrapPoint = false;
        aimPoint.GetComponent<SpriteRenderer>().enabled = false;

    }
    void ResetGrap()
    {
        playerVars.hasGrapPoint = false;
        aimPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
    //should be called every time when switching to Aim phase
    public void Switch()
    {
        ResetGrap();
        if (aimArrowHolder.GetChild(0).GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
        }
        else
            aimArrowHolder.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;


        laternLight.enabled = true;

    }
    public void End()
    {
        if (aimArrowHolder.GetChild(0).GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("AimArrowHolder on AimPhase Script has no attatched SpriteRenderer on child");
        }
        else
            aimArrowHolder.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        if (aimPoint != null)
        {
            aimPoint.GetComponent<SpriteRenderer>().enabled = false;

        }

        laternLight.enabled = false;

    }
    private void OnDrawGizmos()
    {
        //Debug.DrawLine(_playerTransform.position, hit[0].point);

    }
    public void RotateAimToGrap()
    {
        Vector3 dirToHitPoint = new Vector3(hit[0].point.x - _playerTransform.position.x,
           hit[0].point.y - _playerTransform.position.y, 0);
        float angleToHit = Mathf.Atan2(dirToHitPoint.y, dirToHitPoint.x) * Mathf.Rad2Deg;

        Quaternion newRot = Quaternion.Euler(0, 0, angleToHit);
        float angleDelta = Mathf.Abs(Quaternion.Angle(aimArrowHolder.rotation, newRot));
        aimArrowHolder.rotation = Quaternion.RotateTowards(aimArrowHolder.rotation, newRot, angleDelta / 5);
    }

    public void RotateJoystickPointer(Vector2 aimDelta)
    {
        float playerAngle = Mathf.Atan2(_playerTransform.right.y, _playerTransform.right.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = deltaAngle + playerAngle;
        Quaternion newRot = Quaternion.Euler(0, 0, angle);
        float angleDelta = Mathf.Abs(Quaternion.Angle(aimArrowHolder.rotation, newRot));
        laternLight.transform.rotation = Quaternion.RotateTowards(laternLight.transform.rotation, newRot, angleDelta / 2);
    }

    //returns position offset
    public Vector3 Run(Vector2 aimDelta, out Vector3 grapPos, out Vector3Int grapTilePos)
    {
        float playerAngle = Mathf.Atan2(_playerTransform.right.y, _playerTransform.right.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = deltaAngle + playerAngle;
        //shoot rayCast in direction of joystick
        Vector2 dir = new Vector2(math.cos(math.radians(angle)), math.sin(math.radians(angle)));
        //Debug.DrawLine(_playerTransform.position, dir);
        int hitCount = Physics2D.CircleCastNonAlloc(
            _playerTransform.position, 1f, dir, hit, _grapLength, _grapPointsMask);

        //change grap posiotion only if found new target
        if (hitCount != 0)
        {

            _grapPos = hit[0].point;
            Vector3 grapTileWorldPos = _grapPos - new Vector3(hit[0].normal.x, hit[0].normal.y, 0);

            Debug.DrawLine(hit[0].point, _grapPos + new Vector3(hit[0].normal.x, hit[0].normal.y, 0));

            _grapTilePos = _wallTilemap.WorldToCell(grapTileWorldPos);

        }
        if (MapGenerator.CheckForTile(_wallTilemap, _grapTilePos))
        {
            playerVars.hasGrapPoint = true;
            aimPoint.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            playerVars.hasGrapPoint = false;
            aimPoint.GetComponent<SpriteRenderer>().enabled = false;
        }
        SetAimPoint(_grapPos);

        grapPos = _grapPos;
        grapTilePos = _grapTilePos;

        return (_playerTransform.up * _speed * Time.fixedDeltaTime);
    }
    void CheckForAdjacent(Tilemap wallTilemap)
    {
        Vector3Int tilePos;
        Vector3[] deltaSide = new Vector3[] {
            new Vector3(0.5f, 0, 0),
            new Vector3(0, 0.5f, 0),
            new Vector3(-0.5f, 0, 0),
            new Vector3(0, -0.5f, 0),
            };
        for (int i = 0; i < 4; i++)
        {
            tilePos = wallTilemap.WorldToCell(_grapPos + deltaSide[i]);
            if (MapGenerator.CheckForTile(wallTilemap, tilePos))
            {
                return;
            }
        }
        playerVars.hasGrapPoint = false;
        //aimPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void SetAimPoint(Vector3 point)
    {
        float delta = Vector3.Distance(aimPoint.position, point);
        aimPoint.position = Vector3.MoveTowards(aimPoint.position, point, delta / 5);
    }

}