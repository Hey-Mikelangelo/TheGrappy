using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent (typeof (AimPhase))]
[RequireComponent (typeof (GrapPhase))]
public class PlayerManager : MonoBehaviour {
    public Linker linker;
    public Collision2DEventChannelSO playerCollision;
    private GrapPhase grapPhase;
    private AimPhase aimPhase;

    public float speed = 1;
    [Header ("Aim phase variables")]
    public float grapLength = 20;
    public LayerMask grapPointsMask;

    [Header ("Grap phase variables")]
    [Range(0.01f, 1f)]
    public float rotToAlignTime = 0.5f;
    public float maxRotSpeed = 4;
    public float minOrbitRadius = 1f;
    public float pullSpeed = 5;
    public float pullDelay = 0.05f;
    private Phase _phase;
    private Vector2 _aimDelta;
    private Vector3 _grapPoint;

    private Vector3 startPos;
    private List<GameObject> CoinsPool;
    public enum Phase {
        aim,
        grap
    }
    private void Awake () {
        CoinsPool = new List<GameObject>();
        aimPhase = GetComponent<AimPhase> ();
        grapPhase = GetComponent<GrapPhase> ();
        
    }

    private void OnEnable () {
        linker.inputProxy.aimEvent += onAim;
        linker.inputProxy.actionStartEvent += onActionStart;
        linker.inputProxy.actionEndEvent += onActionEnd;
        playerCollision.collision2DEvent += OnPlayerCollision;
    }
    private void OnDisable () {
        linker.inputProxy.aimEvent -= onAim;
        linker.inputProxy.actionStartEvent -= onActionStart;
        linker.inputProxy.actionEndEvent -= onActionEnd;
        playerCollision.collision2DEvent -= OnPlayerCollision;
    }
    void Start () {
        startPos = transform.position;
        aimPhase.Setup (grapLength, grapPointsMask, speed);
        grapPhase.Setup (speed, maxRotSpeed, rotToAlignTime, pullSpeed, minOrbitRadius, pullDelay);
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
    private void OnPlayerCollision(GameObject player, Collision2D collision)
    {
        //if not a collectible
        if (collision.gameObject.layer != 9)
        {
            Die();
        }
        else
        {
            Collectible collectible = collision.gameObject.GetComponent<CollectibleItem>().type;
            GameObject coin = collision.gameObject;
            CoinsPool.Add(coin);
            coin.SetActive(false);
        }
    }
    private void Die()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        foreach (var coin in CoinsPool)
        {
            coin.SetActive(true);
        }
        StartAimPhase();
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