using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "Game/MapGeneration/Map Data")]
public class MapDataSO : ScriptableObject
{
    public CollectibleGeneratorSO collectibleGenerator;
    public WallGeneratorSO wallGenerator;
    [HideInInspector]public int perlinResolution = 3;
    [HideInInspector]public float perlinScale = 0.3f;
    public int chunkSize;
    public int chunksLoadingRadius = 1;
    [Header("Walls")]
    [Range(0.01f, 0.15f)]
    public float wallPerlinThreshold = 0.07f;
    public int wallCellScaling = 2;
    [Header("Coins")]
    [Range(0.9f, 1f)]
    public float collectiblesPerlinThreshold = 0.9f;
    public float powerupChance = 0.05f;
    public int collectibleCellScaling = 1;
    [Header("tiles")]
    public List<Tile> Tiles = new List<Tile>();

    public byte GetTileIndex(Tile tile)
    {
        for (byte i = 0; i < Tiles.Count; i++)
        {
            if (tile == Tiles[i])
            {
                return i;
            }
        }
        return 0;
    }


    public int GetWallTileCountInChunk()
    {
        float x = (wallPerlinThreshold * 100);
        return ((chunkSize * chunkSize) / ((int)((0.4f * x - 6.5f) * (0.4f * x - 6.5f)) + 5)) / 2 * wallCellScaling;
    }
    public int GetCollectibleTileCountInChunk()
    {
        float x = (collectiblesPerlinThreshold * 100);
        return (int)((0.6f * x - 62) * (0.6f * x - 62)) + 5;
    }
}
