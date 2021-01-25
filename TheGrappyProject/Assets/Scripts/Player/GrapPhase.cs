using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapPhase : MonoBehaviour
{

    public Transform grapPoint;
    private float _speed;
    private float _maxRotSpeed;
    private float _rotToAlignTime;
    private float _pullSpeed;
    private float _minOrbitRadius;
    private float _pullDelay;

    private bool _attatched;
    private float _orbitRadius, _distToGrap, _rotAngle, _rotProgress;
    private bool _doPull, _rotateClockwise;
    private Quaternion _newRot, _prevRot;
    private Vector3 _vectorToGrap;
    private LineRenderer _lineRenderer;
    private Transform _prevParent;
    private Vector2 _delta;
    private Coroutine _DoPullCoroutine;
    public void Setup(float speed, float maxRotSpeed, float rotToAlignTime, float pullSpeed,
       float minOrbitRadius, float pullDelay)
    {
        _speed = speed;
        _maxRotSpeed = maxRotSpeed;
        _rotToAlignTime = rotToAlignTime;
        _pullSpeed = pullSpeed;
        _minOrbitRadius = minOrbitRadius;
        _pullDelay = pullDelay;

        _prevParent = transform.parent;
        _lineRenderer = GetComponent<LineRenderer>();
    }
    public void Switch(Vector3 grapPointPos)
    {
        if (grapPoint.position != grapPointPos)
        {
            grapPoint.position = grapPointPos;
        }
        grapPoint.GetComponent<SpriteRenderer>().enabled = true;
        _orbitRadius = Vector3.Distance(transform.position, grapPoint.position);
        _DoPullCoroutine = StartCoroutine(CanPull(0.05f));
        _attatched = false;
    }
    public void End()
    {
        _lineRenderer.enabled = false;
        DetatchFromGrap();
        grapPoint.GetComponent<SpriteRenderer>().enabled = false;
        grapPoint.rotation = quaternion.identity;
        grapPoint.position = transform.position;
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
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, grapPoint.position);
        }
        else
        {
            _distToGrap = Vector3.Distance(transform.position, grapPoint.position);
            //this allows orbiting around grap realistically 
            if (_distToGrap > _orbitRadius)
            {
                CatchAngle();
                SetRotDirection();
                SetRotation();
                AttatchToGrap();
                AttatchedMovement();
                _lineRenderer.SetPosition(0, transform.position);
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
                    Vector3 vectorToGrap = grapPoint.position - transform.position;
                    vectorToGrap.Normalize();
                    transform.position += vectorToGrap * _pullSpeed * Time.deltaTime;
                    _orbitRadius = Vector3.Distance(transform.position, grapPoint.position);
                }
                transform.position += transform.up * _speed * Time.deltaTime;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, grapPoint.position);
                if (!_lineRenderer.enabled)
                {
                    _lineRenderer.enabled = true;
                }
            }
        }
    }
    void SmoothRotationToAlignGrap()
    {
        transform.localRotation = Quaternion.Slerp(_prevRot, _newRot, _rotProgress);
        _rotProgress += 1 / _rotToAlignTime * Time.deltaTime;
    }
    void AttatchedMovement()
    {

        if (Quaternion.Angle(transform.localRotation, _newRot) > 0.1f)
        {
            SmoothRotationToAlignGrap();
        }
        float rotSpeed = (_speed / _orbitRadius);
        rotSpeed = math.clamp(rotSpeed, 0, _maxRotSpeed);
        rotSpeed = _rotateClockwise ? -rotSpeed : rotSpeed;
        grapPoint.Rotate(transform.forward, rotSpeed * Time.deltaTime * Mathf.Rad2Deg, Space.World);
        if (_doPull && _orbitRadius > _minOrbitRadius)
        {
            //when parent is scaled child's position is also scaled
            Vector2 adjustToGrapScale = new Vector2(1 / grapPoint.localScale.x, 1 / grapPoint.localScale.y);
            Vector3 dirToCenter = transform.localPosition.normalized;
            Vector3 newLocalPos = new Vector2(dirToCenter.x * _pullSpeed * adjustToGrapScale.x,
               dirToCenter.y * _pullSpeed * adjustToGrapScale.y);
            transform.localPosition -= newLocalPos * Time.deltaTime;
            _distToGrap = Vector3.Distance(transform.position, grapPoint.position);
            _orbitRadius = _distToGrap;
        }
    }
    void SetRotation()
    {
        float rotAngle = _rotateClockwise ? _rotAngle + 180 : _rotAngle;
        _prevRot = transform.rotation;
        _newRot = Quaternion.Euler(0, 0, rotAngle);
        _rotProgress = 0;
    }
    void AttatchToGrap()
    {
        transform.parent = grapPoint;
    }
    void CatchAngle()
    {
        _vectorToGrap = transform.position - grapPoint.position;
        float angle = Mathf.Atan2(_vectorToGrap.y, _vectorToGrap.x) * Mathf.Rad2Deg;
        _rotAngle = angle - grapPoint.rotation.eulerAngles.z;
    }
    void SetRotDirection()
    {

        if (math.dot(_vectorToGrap, transform.right) > 0)
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
        transform.parent = _prevParent;
    }
    IEnumerator CanPull(float time)
    {
        yield return new WaitForSeconds(time);
        _doPull = true;
    }
}