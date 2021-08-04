using System.Collections.Generic;
using UnityEngine;

public class CurrentGameData : System.ICloneable
{
    public float TimeSurvived { get; set; }
    public Dictionary<Collectible, int> CollectedThings { get; set; }
    public int Score => Mathf.FloorToInt(TimeSurvived * TIME_TO_SCORE_MULT);

    private const int TIME_TO_SCORE_MULT = 10;

    public object Clone()
    {
        return (CurrentGameData)this.MemberwiseClone();
    }
}
