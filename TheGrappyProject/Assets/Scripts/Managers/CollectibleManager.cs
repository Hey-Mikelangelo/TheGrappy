using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleManager", menuName = "Game/Collectible Manager")]

public class CollectibleManager : ScriptableObject
{

    public List<string> CollectibleNames = new List<string>();
    [Space(20)]
    public string collectiblesFolder;
    private void OnEnable()
    {
        
    }
    public void GenerateEnums()
    {
        //EnumGenerator.GenerateEnum("Collectible", CollectibleNames.ToArray());
    }
    private void OnValidate()
    {
        //GenerateEnums();
    }
}
