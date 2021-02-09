using NUnit.Framework.Constraints;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerPhase
{
    aim,
    grap
}

[RequireComponent(typeof(AimPhase), typeof(GrapPhase), typeof(GameProgressSaver))]
public class GameplayManager : MonoBehaviour
{
    public LinkerSO linker;
    public MapGenerator mapGenerator;
    public OneShot oneShotScript;
    public SideBoost sideBoostScript;
    [Header("Aim phase variables")]
    public float speed = 7;
    public float grapLength = 30;
    public int clearAreaRadius = 15;
    public LayerMask grapPointsMask;

    [Header("Grap phase variables")]
    [Range(0.01f, 1f)]
    public float rotToAlignTime = 0.5f;
    public float maxRotSpeed = 4;
    public float minOrbitRadius = 1f;
    public float pullSpeed = 5;
    public float pullDelay = 0.05f;


    public GameObject player;
    private AimPhase _aimPhase;
    private GrapPhase _grapPhase;
    private GameProgressSaver _progressSaver;
    private Vector2 _aimDelta;
    private Vector2Int _prevPlayerChunk;
    private Vector3 _startPos;
    private float timeSurvived = 0;
    private void Update()
    {
        linker.playerVars.currentChunk = GetCurrentChunk();
        if (_prevPlayerChunk != linker.playerVars.currentChunk)
        {
            mapGenerator.UpdateChunks(linker.playerVars.currentChunk);
        }
        _prevPlayerChunk = linker.playerVars.currentChunk;
        if (!linker.playerVars.isMoving)
        {
            return;
        }

        timeSurvived += Time.deltaTime;
        linker.playerData.lastScore = Mathf.FloorToInt(timeSurvived*10);

        switch (linker.playerVars.currentAbility)
        {
            case Ability.oneShot:
                if (linker.playerVars.oneShot.CheckUse())
                {
                    linker.playerVars.oneShot.SetUsed();

                    SetAbilityNoneIfNoLeft();

                    oneShotScript.Shoot(player.transform, _aimDelta);
                }
                break;
            case Ability.sideBoost:
                if (linker.playerVars.sideBoost.CheckUse())
                {
                    player.transform.position += sideBoostScript.SideBoostStep(player.transform);
                }
                break;
        }
        switch (linker.playerVars.currentPhase)
        {
            case PlayerPhase.aim:
                _aimPhase.Run(_aimDelta,
                    out linker.playerVars.grapPos,
                    out linker.playerVars.grapTilePos);
                break;
            case PlayerPhase.grap:
                _grapPhase.Run();
                break;
        }
    }

    private void Start()
    {
        SetupInitial();

    }
    private void OnEnable()
    {
        LinkScripts();

        LoadPlayerData();

    }
    private void OnDisable()
    {
        UnsubsribeEvents();
        SavePlayerData();
    }
    Vector2Int GetCurrentChunk()
    {
        return mapGenerator.mapData.WorldToChunk(player.transform.position);
    }
    void LinkScripts()
    {
        _aimPhase = GetComponent<AimPhase>();
        _grapPhase = GetComponent<GrapPhase>();
        _progressSaver = GetComponent<GameProgressSaver>();
    }
    void SetupInitial()
    {
        if (player == null)
        {
            player = gameObject;
        }
        SetupAimPhase();
        SetupGrapPhase();
        SubsribeEvents();

        Application.targetFrameRate = 30;

        _startPos = player.transform.position;

        linker.playerVars.isMoving = false;
        linker.playerVars.isMapGenerated = false;

        linker.playerData.lastScore = 0;
        timeSurvived = 0;

        linker.playerVars.currentChunk = GetCurrentChunk();
        _prevPlayerChunk = linker.playerVars.currentChunk;

        mapGenerator.UpdateChunks(linker.playerVars.currentChunk);
        StartAimPhase();
    }
    void OnMapgenerated()
    {
        if (!linker.playerVars.isMoving)
        {
            
            Vector3 bottomLeft = player.transform.position + new Vector3(-clearAreaRadius, -clearAreaRadius, 0);
            Vector3 topRight = player.transform.position + new Vector3(clearAreaRadius, clearAreaRadius, 0);
            mapGenerator.ClearAreaBox(bottomLeft, topRight);
            if (linker.playerVars.isWaitingForMapGenerated)
            {
                linker.playerVars.isMapGenerated = true;
                linker.playerVars.isWaitingForMapGenerated = false;
                StartGame();
            }
        }
        linker.playerVars.isMapGenerated = true;

    }
    public void StartAimPhase()
    {
        _grapPhase.End();
        _aimPhase.Switch();
        linker.playerVars.currentPhase = PlayerPhase.aim;

    }
    public void StartGrapPhase()
    {
        _aimPhase.End();
        _grapPhase.Switch(linker.playerVars.grapPos, linker.playerVars.grapTilePos);
        linker.playerVars.currentPhase = PlayerPhase.grap;
    }
    void SetupAimPhase()
    {
        _aimPhase.Setup(player.transform, grapLength, grapPointsMask, speed);
    }
    void SetupGrapPhase()
    {
        _grapPhase.Setup(player.transform, speed, maxRotSpeed, rotToAlignTime, pullSpeed, minOrbitRadius, pullDelay);
    }
    void LoadPlayerData()
    {
        _progressSaver.LoadProgress();
    }
    void SavePlayerData()
    {
        _progressSaver.SaveProgress();
    }
    void SubsribeEvents()
    {
        linker.inputProxy.aimEvent += OnAim;
        linker.inputProxy.aimEndEvent += OnAimEnd;
        linker.inputProxy.actionStartEvent += OnActionStart;
        linker.inputProxy.actionEndEvent += OnActionEnd;
        linker.inputProxy.abilityStartEvent += OnAbilityStart;
        linker.inputProxy.abilityEndEvent += OnAbilityEnd;
        linker.playerCollisionChannel.collision2DEvent += OnPlayerCollision;
        linker.gameEvents.onUnfadedToPlay += StartGame;
        linker.gameEvents.onMapGenerated += OnMapgenerated;
        Application.quitting += SavePlayerData;
        linker.gameEvents.onSetIsDestroyerFalse += SetIsDestroyerFalseDelayed;
    }
    
    void UnsubsribeEvents()
    {
        linker.inputProxy.aimEvent -= OnAim;
        linker.inputProxy.aimEndEvent -= OnAimEnd;
        linker.inputProxy.actionStartEvent -= OnActionStart;
        linker.inputProxy.actionEndEvent -= OnActionEnd;
        linker.inputProxy.abilityStartEvent -= OnAbilityStart;
        linker.inputProxy.abilityEndEvent -= OnAbilityEnd;
        linker.playerCollisionChannel.collision2DEvent -= OnPlayerCollision;
        linker.gameEvents.onUnfadedToPlay -= StartGame;
        linker.gameEvents.onMapGenerated -= OnMapgenerated;
        Application.quitting -= SavePlayerData;
        linker.gameEvents.onSetIsDestroyerFalse -= SetIsDestroyerFalseDelayed;

    }
    void StartGame()
    {
        if (!linker.playerVars.isMapGenerated)
        {
            linker.playerVars.isWaitingForMapGenerated = true;
            return;
        }
        linker.playerVars.isMoving = true;
    }
    void Die()
    {
        float time = 0.5f;
        linker.gameEvents.OnFadeToMenu(time);
        if(linker.playerData.lastScore > linker.playerData.highScore)
        {
            linker.playerData.highScore = linker.playerData.lastScore;
        }
        timeSurvived = 0;
        SavePlayerData();
        linker.playerData.lastScore = 0;

        StartCoroutine(ResetPlayer(time));
        linker.playerVars.isMoving = false;
        linker.gameEvents.OnPlayerDeath();
    }

    Coroutine SetIsDestroyerFalseCoroutine;
    void SetIsDestroyerFalseDelayed()
    {
        if (SetIsDestroyerFalseCoroutine != null)
        {
            StopCoroutine(SetIsDestroyerFalseCoroutine);
        }
        SetIsDestroyerFalseCoroutine = StartCoroutine(SetDestroyerFalse(0.5f));
    }
    IEnumerator SetDestroyerFalse(float time)
    {
        yield return new WaitForSeconds(time);
        linker.playerVars.isDestroyer = false;
    }

    IEnumerator ResetPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        player.transform.position = _startPos;
        player.transform.rotation = Quaternion.identity;
        linker.playerVars.hasGrapPoint = false;
        linker.playerVars.isMapGenerated = false;
        StartAimPhase();
        mapGenerator.UpdateChunks(GetCurrentChunk());
    }
    void OnPlayerCollision(GameObject player, Collision2D collision)
    {
        Vector3 tileWorldPos = collision.GetContact(0).point - (collision.GetContact(0).normal * 0.5f);
        Vector3Int tilePos = mapGenerator.collectiblesTilemap.WorldToCell(tileWorldPos);
        if (collision.gameObject.layer != 9)
        {
            if (linker.playerVars.isDestroyer)
            {
                mapGenerator.DestroyWallPiece(tilePos, 2);
            }   
            else
            {
                Die();
            }
        }
        else
        {
            
            CollectibleTile tile = mapGenerator.collectiblesTilemap.GetTile(
                tilePos) as CollectibleTile;
            mapGenerator.ChunksCollectiblesRemovedTiles[GetCurrentChunk()].Add(tilePos);

            if (tile == null)
            {
                return;
            }
            Collectible collectible = tile.type;
            switch (collectible)
            {
                case Collectible.coin:
                    linker.dataManager.playerData.coinsCount += 1;
                    timeSurvived += 5;
                    break;
                case Collectible.oneShot:
                    linker.playerVars.currentAbility = Ability.oneShot;
                    linker.playerVars.oneShot.Add();
                    linker.gameEvents.OnChangedAbility(Ability.oneShot);
                    break;
                case Collectible.sideBoost:
                    linker.playerVars.currentAbility = Ability.sideBoost;
                    linker.playerVars.sideBoost.Add();
                    linker.gameEvents.OnChangedAbility(Ability.sideBoost);
                    break;
            }
            linker.gameEvents.OnCollected(collectible);
            mapGenerator.collectiblesTilemap.SetTile(tilePos, null);


        }
    }

    void OnAim(Vector2 delta)
    {
        _aimDelta = delta;
    }
    void OnAimEnd()
    {
        if (linker.playerVars.currentPhase == PlayerPhase.aim)
        {
            if (!linker.playerVars.hasGrapPoint)
            {
                return;
            }
            StartGrapPhase();
        }
      
    }
    void OnActionStart()
    {
        if(linker.playerVars.currentPhase == PlayerPhase.aim)
        {
            switch (linker.playerVars.currentAbility)
            {
                case Ability.oneShot:
                    linker.playerVars.oneShot.Use();
                    break;
                case Ability.sideBoost:
                    sideBoostScript.ResetSideBoost();
                    sideBoostScript.SetSideBoost(0.3f, 50, (_aimDelta.x > 0 ? true : false));
                    linker.playerVars.sideBoost.Use();
                    break;
            }
        }
        else if (linker.playerVars.currentPhase == PlayerPhase.grap)
        {
            StartAimPhase();
        }
    }
    void OnActionEnd()
    {
        _grapPhase.StopPull();
        if (linker.playerVars.currentAbility == Ability.sideBoost)
        {
            linker.playerVars.sideBoost.SetUsed();
        }
        SetIsDestroyerFalseDelayed();
        SetAbilityNoneIfNoLeft();
    }

    void OnAbilityStart()
    {
        
    }
    void OnAbilityEnd()
    {
        
    }
   
    
    void SetAbilityNoneIfNoLeft()
    {
        AbilityAccess abilityAccess = linker.playerVars.GetCurrentAbilityAcccess();
        if (abilityAccess != null && abilityAccess.GetCount() < 1)
        {
            linker.playerVars.currentAbility = Ability.none;
            linker.gameEvents.OnChangedAbility(Ability.none);
        }
    }
}
