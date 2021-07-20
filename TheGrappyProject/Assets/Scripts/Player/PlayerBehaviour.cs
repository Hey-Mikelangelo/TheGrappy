using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MovePhase
{
    aim,
    grap
}

[RequireComponent(typeof(AimPhase), typeof(GrapPhase), typeof(GameProgressSaver))]
public class PlayerBehaviour : MonoBehaviour
{
    public LinkerSO linker;
    public OneShot oneShotScript;
    public SideBoost sideBoostScript;
    public MapGenerator mapGenerator;
    public Collider2D wallCollider;
    public Collider2D collectiblesCollider;

    [Header("Aim phase variables")]
    public AnimationCurve scoreToSpeedCurve;
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
    public float pullDelay = 0.00f;

    public GameObject player;
    private AimPhase aimPhase;
    private GrapPhase grapPhase;
    private GameProgressSaver progressSaver;
    private PlayerVarsSO playerVars;
    private GameEventsSO gameEvents;
    private InputEventsSO inputProxy;
    private Vector2 _aimDelta;
    private Vector2Int _prevPlayerChunk;
    private Vector3 _startPos;
    private Vector3 _positionOffset;
    private void OnEnable()
    {
        if (player == null)
        {
            player = gameObject;
        }
        LinkScripts();
        SubsribeEvents();
        ResetForPlay();
        speed = scoreToSpeedCurve.Evaluate(0);
        SetupAimPhase();
        SetupGrapPhase();
        progressSaver.LoadProgress();
    }
    private void Start()
    {
        MapGenerator.GenerateRandomPerlinOffset();
        mapGenerator.UpdateChunks(GetCurrentChunk());

    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    private void FixedUpdate()
    {
        if (!playerVars.canMove)
        {
            return;
        }
        playerVars.timeSurvived += Time.fixedDeltaTime;
        linker.playerData.lastScore = Mathf.FloorToInt(playerVars.timeSurvived * 10);
        speed = scoreToSpeedCurve.Evaluate(linker.playerData.lastScore);
        pullSpeed = speed/2;
        aimPhase.SetSpeed(speed);
        grapPhase.SetSpeed(speed, pullSpeed);
        Vector2Int currentChunk = GetCurrentChunk();
        if (_prevPlayerChunk != currentChunk)
        {
            mapGenerator.UpdateChunks(currentChunk);
        }
        _prevPlayerChunk = currentChunk;
        UseAbility(playerVars.currentActiveAbility);
        gameEvents.UseAbility(playerVars.currentActiveAbility);

       /* switch (playerVars.currentMovePhase)
        {
            case MovePhase.aim:
                player.transform.position += aimPhase.Run(_aimDelta,
                    out playerVars.grapPos,
                    out playerVars.grapTilePos);
                aimPhase.RotateAimToGrap();
                aimPhase.RotateJoystickPointer(_aimDelta);
                break;
            case MovePhase.grap:
                //position changes inside grapPhase
                grapPhase.Run();
                break;
        }*/

    }
    void UseAbility(Collectible collectible)
    {
        switch (collectible)
        {
            case Collectible.oneShot:
                playerVars.oneShot.Use();
                playerVars.oneShot.DecreaseCount();
                playerVars.SetCurrentActiveAbility(Collectible.none);
                oneShotScript.Shoot(player.transform, _aimDelta);
                playerVars.oneShot.EndUse();
                break;
            case Collectible.sideBoost:
                player.transform.position += sideBoostScript.Step(player.transform);
                break;
        }
    }
    void LinkScripts()
    {
        aimPhase = GetComponent<AimPhase>();
        grapPhase = GetComponent<GrapPhase>();
        progressSaver = GetComponent<GameProgressSaver>();
        gameEvents = linker.gameEvents;
        inputProxy = linker.inputProxy;
        playerVars = linker.playerVars;
    }
    void SubsribeEvents()
    {
        inputProxy.onAim += OnAim;
        inputProxy.onAimEnd += OnAimEnd;
        inputProxy.onMainAction += OnActionStart;
        inputProxy.onMainActionEnd += OnActionEnd;
        gameEvents.onCollision += OnCollision;
        gameEvents.onMapGenerated += OnMapGenerated;
    }
    void ResetForPlay()
    {
        playerVars.timeSurvived = 0;
        player.transform.position = Vector3.zero;
        playerVars.SetCurrentMovePhase(MovePhase.aim);
        gameEvents.ResetGameTimer();
        gameEvents.StartClearMap();
        playerVars.SetCurrentActiveAbility(Collectible.none);
        playerVars.SetCurrentSelectedAbility(Collectible.none);
        playerVars.oneShot.Reset();
        playerVars.sideBoost.Reset();
        sideBoostScript.ResetTime();
        _prevPlayerChunk = GetCurrentChunk();
        playerVars.canMove = false;
        wallCollider.enabled = false;
        collectiblesCollider.enabled = false;
        playerVars.SetIsDestroyer(false);
        gameEvents.ChangedAbility(Collectible.none);
    }
    void SetupAimPhase()
    {
        aimPhase.Setup(player.transform, grapLength, grapPointsMask, speed);
    }
    void SetupGrapPhase()
    {
        grapPhase.Setup(player.transform, speed, maxRotSpeed, rotToAlignTime, pullSpeed, minOrbitRadius, pullDelay);
    }
    public void StartAimPhase()
    {
        grapPhase.End();
        aimPhase.Switch();
        playerVars.SetCurrentMovePhase(MovePhase.aim);
    }
    public void StartGrapPhase()
    {
        aimPhase.End();
        grapPhase.Switch(playerVars.grapPos, playerVars.grapTilePos);
        playerVars.SetCurrentMovePhase(MovePhase.grap);
    }
    void UnsubscribeEvents()
    {
        inputProxy.onAim -= OnAim;
        inputProxy.onAimEnd -= OnAimEnd;
        inputProxy.onMainAction -= OnActionStart;
        inputProxy.onMainActionEnd -= OnActionEnd;
        gameEvents.onCollision -= OnCollision;
        gameEvents.onMapGenerated -= OnMapGenerated;
    }
    void Die()
    {
        if (linker.playerData.lastScore > linker.playerData.highScore)
        {
            linker.playerData.highScore = linker.playerData.lastScore;
        }
        progressSaver.SaveProgress();
        playerVars.canMove = false;
    }
    void OnAim(Vector2 delta)
    {
        _aimDelta = delta;
    }
    void OnAimEnd()
    {
        if (playerVars.currentMovePhase == MovePhase.aim)
        {
            if (!playerVars.hasGrapPoint)
            {
                return;
            }
            StartGrapPhase();
        }
    }
    
    void OnActionStart()
    {
        if (linker.playerVars.currentMovePhase == MovePhase.aim)
        {
            playerVars.SetCurrentActiveAbility(playerVars.currentSelectedAbility);
            playerVars.CheckIfWasLastAbilityUse();
            switch (playerVars.currentActiveAbility)
            {
                case Collectible.sideBoost:
                    playerVars.sideBoost.Use();
                    sideBoostScript.ResetTime();
                    sideBoostScript.SetDirection(_aimDelta.x > 0 ? true : false);
                    break;
            }
        }
        else if (linker.playerVars.currentMovePhase == MovePhase.grap)
        {
            StartAimPhase();
        }
    }
    void OnActionEnd()
    {
        if (playerVars.sideBoost.GetUse())
        {
            if (sideBoostScript.HasLeftTime())
            {
                playerVars.sideBoost.DecreaseCount();
            }
            playerVars.sideBoost.EndUse();
        }
        
        playerVars.CheckIfWasLastAbilityUse();
        playerVars.SetCurrentActiveAbility(Collectible.none);
        playerVars.SetIsDestroyer(false);
    }
    Vector2Int GetCurrentChunk()
    {
        return mapGenerator.mapData.WorldToChunk(player.transform.position);
    }
    void OnCollision(Collision2D collision)
    {
        Vector3 tileWorldPos = collision.GetContact(0).point - (collision.GetContact(0).normal * 0.5f);
        Vector3Int tilePos = mapGenerator.collectiblesTilemap.WorldToCell(tileWorldPos);
        if (collision.gameObject.layer != 9)
        {
            if (linker.playerVars.isDestroyer)
            {
                mapGenerator.DestroyWallPiece(tilePos, 1);
            }
            else
            {
                Die();
                gameEvents.PlayerDeath();
            }
        }
        else
        {

            CollectibleTile tile = mapGenerator.collectiblesTilemap.GetTile(
                tilePos) as CollectibleTile;
            int newIndex = mapGenerator.ChunksCollectiblesRemovedTiles[GetCurrentChunk()].Length;
            mapGenerator.ChunksCollectiblesRemovedTiles[GetCurrentChunk()][newIndex] = tilePos;

            if (tile == null)
            {
                return;
            }
            Collectible collectible = tile.type;
            switch (collectible)
            {
                case Collectible.coin:
                    playerVars.timeSurvived += 5;
                    break;
                case Collectible.oneShot:
                    playerVars.oneShot.Add();
                    playerVars.SetCurrentSelectedAbility(Collectible.oneShot);
                    break;
                case Collectible.sideBoost:
                    linker.playerVars.sideBoost.Add();
                    playerVars.SetCurrentSelectedAbility(Collectible.sideBoost);
                    break;
            }
            gameEvents.Collected(collectible);

            mapGenerator.collectiblesTilemap.SetTile(tilePos, null);
        }
    }
    void OnMapGenerated()
    {
        if (!playerVars.canMove)
        {
            Vector3 bottomLeft = player.transform.position + new Vector3(-clearAreaRadius, -clearAreaRadius + 5, 0);
            Vector3 topRight = player.transform.position + new Vector3(clearAreaRadius, clearAreaRadius + 5, 0);
            mapGenerator.ClearAreaBox(bottomLeft, topRight);
            playerVars.canMove = true;
            //wallCollider.enabled = true;
            collectiblesCollider.enabled = true;
            gameEvents.MapReady();
            Scene scene = gameObject.scene;
            linker.sceneLoadingChannel.SetSceneInited(scene.buildIndex);
            //StartCoroutine(UnfadeAfterFrame());
        }
       
    }
    IEnumerator UnfadeAfterFrame()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Inited");
        
    }
}
