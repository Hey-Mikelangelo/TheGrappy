using UnityEngine;


[CreateAssetMenu(fileName = "PlayerVarsSO", menuName = "Game/PlayerVarsSO")]
public class PlayerVarsSO : ScriptableObject
{
    public bool isMoving;
    public bool isMapGenerated;
    public bool isWaitingForMapGenerated;
    public bool isDestroyer;
    public bool hasGrapPoint;
    public int coinsCount;
    public string playerName;
    public Vector2Int currentChunk;
    public PlayerPhase currentPhase;
    public Ability currentAbility;
    public float speed = 1;
    public float grapLength = 20;
    public Vector3 grapPos;
    public Vector3Int grapTilePos;
    public Quaternion aimWorldRotation;
    public Quaternion rotataionToGrapPoint;
    public AbilityAccess sideBoost = new AbilityAccess(Ability.sideBoost, 1000);
    public AbilityAccess oneShot = new AbilityAccess(Ability.oneShot, 1000);

    public AbilityAccess GetCurrentAbilityAcccess()
    {
        switch (currentAbility)
        {
            case Ability.none:
                return null;
            case Ability.oneShot:
                return oneShot;
            case Ability.sideBoost:
                return sideBoost;
            default:
                return null;
        }
    }
}
