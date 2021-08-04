using System.Collections.Generic;

public class CollectedAbilitiesPocket
{
    private Dictionary<Collectible, int> collectedAbilitiesCount = new Dictionary<Collectible, int>();
    private const int MAX_CAPACITY = 5;
   
    public int GetCollectedAbilityCount(Collectible collectibleType)
    {
        return collectedAbilitiesCount[collectibleType];
    }

    public bool AddCollectible(Collectible collectibleType)
    {
        if (collectedAbilitiesCount[collectibleType] < MAX_CAPACITY)
        {
            collectedAbilitiesCount[collectibleType] += 1;
            return true;
        }
        return false;
    }

    public bool RemoveCollectible(Collectible collectibleType)
    {
        if (collectedAbilitiesCount[collectibleType] > 0)
        {
            collectedAbilitiesCount[collectibleType] -= 1;
            return true;
        }
        return false;
    }

    public System.Type GetAbilityController(Collectible collectibleType) 
    {
        switch (collectibleType)
        {
            case Collectible.coin:
                return null;
            case Collectible.oneShot:
                return null;
            default:
                return null;    
        }
    }
}
