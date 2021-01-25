using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "Game/MapGeneration/Map Data")]
public class MapDataSO : ScriptableObject
{
    public CollectibleGeneratorSO collectibleGenerator;
    public WallGeneratorSO wallGenerator;
    public int chunkSize;
    public int perlinResolution = 3;
    public float perlinScale = 0.3f;
    [Header("Walls")]
    public float wallPerlinThreshold = 0.07f;
    public int wallCellScaling = 2;
    [Header("Coins")]
    public float coinsPerlinThreshold = 0.9f;
    public int coinsCellScaling = 1;
    
}
