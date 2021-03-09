using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerVarsSO", menuName = "Game/PlayerVarsSO")]
public class PlayerVarsSO : ScriptableObject
{
    public GameEventsSO gameEvents;
    public bool canMove;
    public bool isMapGenerated;
    public bool isWaitingForMapGenerated;
    public bool isDestroyer { get; private set; }
    public bool hasGrapPoint { get; set; }
    public int coinsCount;
    public float timeSurvived;
    public float lastScore;
    public string playerName;
    public Vector2Int currentChunk;
    //public PlayerPhase currentPhase;
    public MovePhase currentMovePhase;
    public Collectible currentSelectedAbility;
    public Collectible currentActiveAbility;
    public Vector3 grapPos;
    public Vector3Int grapTilePos;
    public Vector2 aimDir;
    public Quaternion aimWorldRotation;
    public Quaternion rotataionToGrapPoint;
    public AbilityAccess sideBoost = new AbilityAccess(Collectible.sideBoost, 6);
    public AbilityAccess oneShot = new AbilityAccess(Collectible.oneShot, 6);
    public void SetIsDestroyer(bool setTrue)
    {
        if (setTrue)
        {
            isDestroyer = true;
        }
        else
        {
            isDestroyer = false;
        }
    }
    public void SetCurrentMovePhase(MovePhase phase)
    {
        currentMovePhase = phase;
    }
    public void SetCurrentActiveAbility(Collectible collectible)
    {
        currentActiveAbility = collectible;
    }
    public void SetCurrentSelectedAbility(Collectible collectible)
    {
        currentSelectedAbility = collectible;
        gameEvents.ChangedAbility(collectible);

    }
    public AbilityAccess GetCurrentAbilityAcccess()
    {
        return GetAbilityAcccess(currentSelectedAbility);

    }
    public AbilityAccess GetNotZeroAbility()
    {
        if(sideBoost.GetCount() > 0)
        {
            return sideBoost;
        }else if(oneShot.GetCount() > 0)
        {
            return oneShot;
        }
        else
        {
            return null;
        }
    }
    public AbilityAccess GetAbilityAcccess(Collectible ability)
    {
        switch (ability)
        {
            case Collectible.none:
                return null;
            case Collectible.oneShot:
                return oneShot;
            case Collectible.sideBoost:
                return sideBoost;
            default:
                return null;
        }
    }
    public void CheckIfWasLastAbilityUse()
    {
        AbilityAccess abilityAccess = GetAbilityAcccess(currentSelectedAbility);
        if (abilityAccess == null)
        {
            return;
        }
        if (abilityAccess.GetCount() > 0)
        {
            return;
        }
        else
        {
            SetCurrentSelectedAbility(Collectible.none);
        }
        if (sideBoost.GetCount() > 0)
        {
            SetCurrentSelectedAbility(sideBoost.ability);
        }
        else if (oneShot.GetCount() > 0)
        {
            SetCurrentSelectedAbility(oneShot.ability);
        }
    }
}
