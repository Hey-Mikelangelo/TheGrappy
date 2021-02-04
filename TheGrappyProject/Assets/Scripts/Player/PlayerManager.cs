using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public enum Ability
{
    none,
    sideBoost,
    oneShot
}


[RequireComponent (typeof (AimPhase))]
[RequireComponent (typeof (GrapPhase))]

public class PlayerManager : MonoBehaviour {
    public bool runGame;

    [Header("Scriptable Objects")]
    public Collision2DEventChannelSO playerCollision;
    public DataManagerSO dataManager;
    public LinkerSO linker;

    [Header("Debug data")]
    public int coinsCount;
    public string playerName;

    [Header("Game Objects")]
    public Tilemap collectiblesTilemap;
    public MapGenerator mapGenerator;
    public TextMeshProUGUI coinsCountDisplay;
    private GrapPhase grapPhase;
    private AimPhase aimPhase;

    [Header("Scripts")]
    public SideBoost sideBoost;
    public OneShot oneShot;

    [Header ("Aim phase variables")]
    public float speed = 1;
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

    public Ability currentAbility;

    private bool _doSideBoost, _doOneShot;
    public float _force, _boostTime;
    [Space(10)]
    public Vector2Int playerChunk;
    private Vector2Int prevPlayerChunk;
    public enum Phase {
        aim,
        grap
    }
    public void GenerateChunks()
    {
        mapGenerator.PlayerChangedChunk(playerChunk);

    }
    Vector2Int GetCurrentChunk()
    {
        return mapGenerator.mapData.WorldToChunk(transform.position);
    }
    private void Start () {
       
        CoinsPool = new List<GameObject>();
        aimPhase = GetComponent<AimPhase> ();
        grapPhase = GetComponent<GrapPhase> ();
        Application.targetFrameRate = 30;

        oneShot.Shoot(transform, _aimDelta);
        playerChunk = GetCurrentChunk();
        prevPlayerChunk = playerChunk;
        GenerateChunks();

        startPos = transform.position;
        aimPhase.Setup(grapLength, grapPointsMask, speed);
        grapPhase.Setup(speed, maxRotSpeed, rotToAlignTime, pullSpeed, minOrbitRadius, pullDelay);

        StartAimPhase();
    }

    private void OnEnable () {
        linker.inputProxy.aimEvent += OnAim;
        linker.inputProxy.actionStartEvent += OnActionStart;
        linker.inputProxy.actionEndEvent += OnActionEnd;
        linker.inputProxy.abilityStartEvent += OnAbilityStart;
        linker.inputProxy.abilityEndEvent += OnAbilityEnd;

        playerCollision.collision2DEvent += OnPlayerCollision;

        coinsCountDisplay.text = "Coins: " + dataManager.playerData.coinsCount;
    }
    private void OnDisable () {
        linker.inputProxy.aimEvent -= OnAim;
        linker.inputProxy.actionStartEvent -= OnActionStart;
        linker.inputProxy.actionEndEvent -= OnActionEnd;
        linker.inputProxy.abilityStartEvent -= OnAbilityStart;
        linker.inputProxy.abilityEndEvent -= OnAbilityEnd;

        playerCollision.collision2DEvent -= OnPlayerCollision;
    }
   
    void Update () {

        playerChunk = GetCurrentChunk();
        if(prevPlayerChunk != playerChunk)
        {
            GenerateChunks();
        }
        prevPlayerChunk = playerChunk;

        if (!runGame)
        {
            return;
        }
        switch (currentAbility)
        {
            case Ability.oneShot:
                if (_doOneShot)
                {
                    _doOneShot = false;
                    currentAbility = Ability.none;
                    oneShot.Shoot(transform, _aimDelta);
                }
                break;
            case Ability.sideBoost:
                if (_doSideBoost)
                {
                    transform.position += sideBoost.SideBoostStep(transform);
                }
                break;
        }
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
            Vector3Int tilePos = collectiblesTilemap.WorldToCell(collision.GetContact(0).point);
            CollectibleTile tile = collectiblesTilemap.GetTile(
                tilePos) as CollectibleTile;
            mapGenerator.ChunksCollectiblesRemovedTiles[GetCurrentChunk()].Add(tilePos);

            Collectible collectible = tile.type;
            switch (collectible)
            {
                case Collectible.coin:
                    GameObject coin = collision.gameObject;
                    dataManager.playerData.coinsCount += 1;
                    coinsCountDisplay.text = "Coins: " + dataManager.playerData.coinsCount;
                    break;
                case Collectible.oneShot:
                    currentAbility = Ability.oneShot;
                    break;
                case Collectible.sideBoost:
                    currentAbility = Ability.sideBoost;
                    break;
            }
            collectiblesTilemap.SetTile(tilePos, null);


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
    void OnAim (Vector2 delta) {
        if (delta != zero)
            _aimDelta = delta;
    }
    void OnActionStart () {
        if (_phase == Phase.aim) {
            if (_grapPoint == Vector3.zero) {
                return;
            }
            StartGrapPhase ();
        } else if (_phase == Phase.grap) {
            StartAimPhase ();
        }
    }
    void OnActionEnd () {
        grapPhase.StopPull ();
    }

    void OnAbilityStart()
    {
        switch (currentAbility)
        {
            case Ability.oneShot:
                _doOneShot = true;
                break;
            case Ability.sideBoost:
                _doSideBoost = true;
                sideBoost.ResetSideBoost();
                sideBoost.SetSideBoost(_boostTime, _force, (_aimDelta.x > 0 ? true : false));
                break;
        }
    }
    void OnAbilityEnd()
    {
        _doSideBoost = false;
        currentAbility = Ability.none;

    }
}