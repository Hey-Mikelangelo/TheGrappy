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
    private GameplayManager _gameplayManager;
    private Transform _playerTransform;
    private Tilemap _wallTilemap;

    //should be called once to setup variables 
    public void Setup(Transform playerTransform, float grapLength, LayerMask grapPointsMask, float speed)
    {
        _playerTransform = playerTransform;
        _grapLength = grapLength;
        _grapPointsMask = grapPointsMask;
        _speed = speed;
        _grapPos = Vector3.zero;
        _gameplayManager = GetComponent<GameplayManager>();
        _gameplayManager.linker.playerVars.hasGrapPoint = false;
        aimPoint.GetComponent<SpriteRenderer>().enabled = false;
        _wallTilemap = _gameplayManager.mapGenerator.wallTilemap;

    }
    void ResetGrap()
    {
        _gameplayManager.linker.playerVars.hasGrapPoint = false;
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

        if(aimPoint!= null)
        {
            aimPoint.GetComponent<SpriteRenderer>().enabled = false;

        }

        laternLight.enabled = false;
       
    }
    private void OnDrawGizmos()
    {
        //Debug.DrawLine(_playerTransform.position, hit[0].point);

    }
    private void DrawAim(Vector3 dir)
    {
        Vector3 dirToHitPoint = new Vector3(hit[0].point.x - _playerTransform.position.x,
           hit[0].point.y - _playerTransform.position.y, 0);
        float angleToHit = Mathf.Atan2(dirToHitPoint.y, dirToHitPoint.x) * Mathf.Rad2Deg;

        Quaternion newRot = Quaternion.Euler(0, 0, angleToHit);
        Quaternion prevRot = Quaternion.Euler(0, 0, _aimAngle);
        aimArrowHolder.rotation = Quaternion.RotateTowards(aimArrowHolder.rotation, newRot, 20);
      
    }
   
    void RotateLight(Vector2 aimDelta)
    {
        float playerAngle = Mathf.Atan2(_playerTransform.right.y, _playerTransform.right.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.Atan2(aimDelta.y, aimDelta.x) * Mathf.Rad2Deg;
        float angle = deltaAngle + playerAngle;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        laternLight.transform.rotation = rotation;
    }
    public void Run(Vector2 aimDelta, out Vector3 grapPos, out Vector3Int grapTilePos)
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
            Debug.Log("Set grapPos");
            
            _grapPos = hit[0].point;
            Vector3 grapTileWorldPos = _grapPos - new Vector3(hit[0].normal.x, hit[0].normal.y, 0);

            Debug.DrawLine(hit[0].point, _grapPos +new Vector3(hit[0].normal.x, hit[0].normal.y, 0));

            _grapTilePos = _wallTilemap.WorldToCell(grapTileWorldPos);
            
        }
        if (MapGenerator.CheckForTile(_wallTilemap, _grapTilePos))
        {
            Debug.Log("Check for tile");
            _gameplayManager.linker.playerVars.hasGrapPoint = true;
            aimPoint.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            _gameplayManager.linker.playerVars.hasGrapPoint = false;
            aimPoint.GetComponent<SpriteRenderer>().enabled = false;
        }
        _playerTransform.position += _playerTransform.up * _speed * Time.deltaTime;
        RotateLight(aimDelta);
        DrawAim(dir);
        SetAimPoint(_grapPos);

        grapPos = _grapPos;
        grapTilePos = _grapTilePos;
       // SetAimPoint(_grapPos - (new Vector3(dir.x, dir.y, 0) * 0.5f));
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
        _gameplayManager.linker.playerVars.hasGrapPoint = false;
        //aimPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void SetAimPoint(Vector3 point)
    {
        aimPoint.position = point;
    }

}