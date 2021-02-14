using System.Collections.Generic;

public class AbilityAccess
{
    public Collectible ability;
    public int maxCount;
    private int count;
    private bool isUsing;
    public void Reset()
    {
        count = 0;
        isUsing = false;
    }
    public int GetCount()
    {
        return count;
    }
    public bool GetUse()
    {
        return isUsing;
    }
    public void Add()
    {
        if (count < maxCount)
            count++;
    }
    public void Use()
    {
        isUsing = true;
    }
    public void EndUse()
    {
        isUsing = false;
    }
    //return false if cannot use
    public bool DecreaseCount()
    {
        if(count > 0)
        {
            count--;
            return true;
        }
        else
        {
            return false;
        }
    }
    public AbilityAccess(Collectible ability, int maxCount)
    {
        this.ability = ability;
        this.maxCount = maxCount;
        count = 0;
    }
}
