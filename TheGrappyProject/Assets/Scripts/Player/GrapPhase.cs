using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class GrapPhase : MonoBehaviour
{

    public Transform grapPoint;
    private float _speed;
    private float _maxRotSpeed;
    private float _rotToAlignTime;
    private float _pullSpeed;
    private float _minOrbitRadius;

    private bool _attatched;
    private float _orbitRadius, _distToGrap, _rotAngle, _rotProgress;
    private bool _doPull, _rotateClockwise;
    private float _pullDelay;
    private Quaternion _newRot, _prevRot;
    private Vector3 _vectorToGrap;
    private Vector3Int _grapTilePos;
    private Tilemap _wallTilemap;
    private LineRenderer _lineRenderer;
    private Transform _prevParent;
    private Transform _playerTransform;
    private Coroutine _DoPullCoroutine;
    private PlayerBehaviour playerBehaviour;
    private PlayerVarsSO playerVars;
    public void Setup(Transform playerTransform, float speed, float maxRotSpeed, float rotToAlignTime, float pullSpeed,
       float minOrbitRadius, float pullDelay)
    {
        _playerTransform = playerTransform;
        _speed = speed;
        _maxRotSpeed = maxRotSpeed;
        _rotToAlignTime = rotToAlignTime;
        _pullSpeed = pullSpeed;
        _minOrbitRadius = minOrbitRadius;
        _pullDelay = pullDelay;

        _prevParent = _playerTransform.parent;
        _lineRenderer = GetComponent<LineRenderer>();
        grapPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        playerBehaviour = GetComponent<PlayerBehaviour>();
        _wallTilemap = playerBehaviour.mapGenerator.wallTilemap;
        playerVars = playerBehaviour.linker.playerVars;
    }
    public void SetSpeed(float speed, float pullSpeed)
    {
        _speed = speed;
        _pullSpeed = pullSpeed;
    }
    public void Switch(Vector3 grapPointPos, Vector3Int grapTilePos)
    {
        grapPoint.position = grapPointPos;
        _grapTilePos = grapTilePos;

        grapPoint.GetComponent<SpriteRenderer>().enabled = true;
        _orbitRadius = Vector3.Distance(_playerTransform.position, grapPoint.position);
        _DoPullCoroutine = StartCoroutine(CanPull(_pullDelay));
        _attatched = false;
    }
    public void End()
    {
        if (_lineRenderer)
            _lineRenderer.enabled = false;

        StopPull();
        DetatchFromGrap();
        if (grapPoint != null)
        {
            grapPoint.GetComponent<SpriteRenderer>().enabled = false;
            grapPoint.rotation = quaternion.identity;
            grapPoint.position = _playerTransform.position;
        }

    }
    public void StopPull()
    {
        if (_DoPullCoroutine != null)
        {
            StopCoroutine(_DoPullCoroutine);
            _doPull = false;
        }

    }
    public void Run()
    { 
        if (_attatched)
        {
            AttatchedMovement();
            _lineRenderer.SetPosition(0, _playerTransform.position);
            _lineRenderer.SetPosition(1, grapPoint.position);
        }
        else
        {
            _distToGrap = Vector3.Distance(_playerTransform.position, grapPoint.position);
            //this allows orbiting around grap realistically 
            if (_distToGrap > _orbitRadius)
            {
                CatchAngle();
                SetRotDirection();
                SetRotation();
                AttatchToGrap();
                AttatchedMovement();
                _lineRenderer.SetPosition(0, _playerTransform.position);
                _lineRenderer.SetPosition(1, grapPoint.position);
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }
                _attatched = true;
            }
            else
            {
                if (_doPull && _orbitRadius > _minOrbitRadius)
                {
                    Vector3 vectorToGrap = grapPoint.position - _playerTransform.position;
                    vectorToGrap.Normalize();
                    _playerTransform.position += vectorToGrap * _pullSpeed * Time.fixedDeltaTime;
                    _orbitRadius = Vector3.Distance(_playerTransform.position, grapPoint.position);
                }
                _playerTransform.position += _playerTransform.up * _speed * Time.fixedDeltaTime;
                _lineRenderer.SetPosition(0, _playerTransform.position);
                _lineRenderer.SetPosition(1, grapPoint.position);
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }
            }
        }
        if (!MapGenerator.CheckForTile(_wallTilemap, _wallTilemap.WorldToCell(_grapTilePos)))
        {
            playerVars.hasGrapPoint = false;
            playerBehaviour.StartAimPhase();
        }
    }
    void SmoothRotationToAlignGrap()
    {
        _playerTransform.localRotation = Quaternion.Slerp(_prevRot, _newRot, _rotProgress);
        _rotProgress += 1 / _rotToAlignTime * Time.fixedDeltaTime;
    }
    void AttatchedMovement()
    {

        if (Quaternion.Angle(_playerTransform.localRotation, _newRot) > 0.1f)
        {
            SmoothRotationToAlignGrap();
        }
        float rotSpeed = (_speed / _orbitRadius);
        rotSpeed = math.clamp(rotSpeed, 0, _maxRotSpeed);
        rotSpeed = _rotateClockwise ? -rotSpeed : rotSpeed;
        grapPoint.Rotate(_playerTransform.forward, rotSpeed * Time.fixedDeltaTime * Mathf.Rad2Deg, Space.World);
        if (_doPull && _orbitRadius > _minOrbitRadius)
        {
            //when parent is scaled child's position is also scaled
            Vector2 adjustToGrapScale = new Vector2(1 / grapPoint.localScale.x, 1 / grapPoint.localScale.y);
            Vector3 dirToCenter = _playerTransform.localPosition.normalized;
            Vector3 newLocalPos = new Vector2(dirToCenter.x * _pullSpeed * adjustToGrapScale.x,
               dirToCenter.y * _pullSpeed * adjustToGrapScale.y);
            _playerTransform.localPosition -= newLocalPos * Time.fixedDeltaTime;
        }
        _distToGrap = Vector3.Distance(_playerTransform.position, grapPoint.position);
        _orbitRadius = _distToGrap;
    }
    void SetRotation()
    {
        float rotAngle = _rotateClockwise ? _rotAngle + 180 : _rotAngle;
        _prevRot = _playerTransform.rotation;
        _newRot = Quaternion.Euler(0, 0, rotAngle);
        _rotProgress = 0;
    }
    void AttatchToGrap()
    {
        _playerTransform.parent = grapPoint;
    }
    void CatchAngle()
    {
        _vectorToGrap = _playerTransform.position - grapPoint.position;
        float angle = Mathf.Atan2(_vectorToGrap.y, _vectorToGrap.x) * Mathf.Rad2Deg;
        _rotAngle = angle - grapPoint.rotation.eulerAngles.z;
    }
    void SetRotDirection()
    {

        if (math.dot(_vectorToGrap, _playerTransform.right) > 0)
        {
            _rotateClockwise = false;
        }
        else
        {
            _rotateClockwise = true;
        }
    }
    void DetatchFromGrap()
    {
        _playerTransform.parent = _prevParent;
    }
    IEnumerator CanPull(float time)
    {
        yield return new WaitForSeconds(time);
        _doPull = true;
    }
}