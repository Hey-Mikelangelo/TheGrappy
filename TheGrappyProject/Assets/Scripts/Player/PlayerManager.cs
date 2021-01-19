using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent (typeof (AimPhase))]
[RequireComponent (typeof (GrapPhase))]
public class PlayerManager : MonoBehaviour {
    public Linker linker;
    private GrapPhase grapPhase;
    private AimPhase aimPhase;

    [Header ("Aim phase variables")]
    public float speed = 1;
    public float grapLength;
    public float circleCastRadius;
    public LayerMask grapPointsMask;

    [Header ("Grap phase variables")]
    public float rotToAlignSpeed = 0.5f;
    public float minOrbitRadius = 1f;
    public float pullSpeed = 1;
    public float pullDelay = 0.05f;
    private Phase _phase;
    private Vector2 _aimDelta;
    private bool _onActionEnd;
    private Vector3 _grapPoint;
    public enum Phase {
        aim,
        grap
    }
    private void Awake () {
        aimPhase = GetComponent<AimPhase> ();
        grapPhase = GetComponent<GrapPhase> ();
    }
    private void OnEnable () {
        linker.inputProxy.aimEvent += onAim;
        linker.inputProxy.actionStartEvent += onActionStart;
        linker.inputProxy.actionEndEvent += onActionEnd;

    }
    private void OnDisable () {
        linker.inputProxy.aimEvent -= onAim;
        linker.inputProxy.actionStartEvent -= onActionStart;
        linker.inputProxy.actionEndEvent -= onActionEnd;
    }
    void Start () {
        aimPhase.Setup (grapLength, circleCastRadius, grapPointsMask, speed);
        grapPhase.Setup (speed, rotToAlignSpeed, pullSpeed, minOrbitRadius, pullDelay);
        StartAimPhase ();
    }
    void Update () {
        switch (_phase) {
            case Phase.aim:
                _grapPoint = aimPhase.Run (_aimDelta, new Vector2 (0, 0));
                break;
            case Phase.grap:
                grapPhase.Run ();
                break;
        }

    }
    private void MoveForward () {
        transform.position += transform.up * speed * Time.deltaTime;

    }
    public void StartAimPhase () {
        grapPhase.End ();
        aimPhase.Switch ();
        _phase = Phase.aim;

    }
    public void StartGrapPhase () {
        aimPhase.End ();
        grapPhase.Switch (_grapPoint);
        _phase = Phase.grap;
    }
    Vector2 zero = new Vector2 (0, 0);
    void onAim (Vector2 delta) {
        if (delta != zero)
            _aimDelta = delta;
    }
    void onActionStart () {
        if (_phase == Phase.aim) {
            if (_grapPoint == null) {
                return;
            }
            StartGrapPhase ();
        } else if (_phase == Phase.grap) {
            StartAimPhase ();
        }
    }
    void onActionEnd () {
        grapPhase.StopPull ();
    }
}