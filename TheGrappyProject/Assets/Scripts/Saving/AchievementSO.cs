using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "Game/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string title;
    public string description;
    public int id { get; private set; }

    public void SetId(int id)
    {
        this.id = id;
    }
}
