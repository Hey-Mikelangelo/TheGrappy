using System.Collections.Generic;

public class AbilityAccess
{
    public Ability ability;
    public int maxCount;
    private List<bool> Available;
    public bool doUse;

    public int GetCount()
    {
        return Available.Count;
    }
    public bool CheckUse()
    {
        return doUse;
    }

    public void Use()
    {
        if (Available.Count > 0)
        {
            doUse = true;
        }
    }
    public void Add()
    {
        if(Available.Count < maxCount)
            Available.Add(true);
    }
    
    //returns false if there is no more collected ability
    public bool SetUsed()
    {
        doUse = false;
        if (Available.Count > 0)
        {
            Available.RemoveAt(Available.Count - 1);
        }
        return Available.Count > 0 ? true : false;
    }
    public AbilityAccess(Ability ability, int maxCount)
    {
        this.ability = ability;
        this.maxCount = maxCount;
        Available = new List<bool>(maxCount);

        for (int i = 0; i < maxCount; i++)
        {
            Available.Add(true);
        }
    }
}
