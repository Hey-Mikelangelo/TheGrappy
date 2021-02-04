using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CollectibleGeneratorSO", menuName = "Game/MapGeneration/Collectible Generator")]
public class CollectibleGeneratorSO : ScriptableObject
{
    public WallGeneratorSO wallGenerator;
    public MapDataSO mapData;
    [Header("Collectibles")]
    public Tile coinTile; //tile index = 100
    public List<CollectibleTile> Powerups = new List<CollectibleTile>();
    private Tilemap _tilemap;
   /* public GameObject sideBoostPrefab;
    public GameObject oneShotPrefab;*/

    private int _collectibleCellSize;
    public ChunkData CreateCollectiblesChunk(int x, int y, Tilemap tilemap)
    {
        _tilemap = tilemap;
        _collectibleCellSize = MapGenerator.CalculateCellSize(mapData.collectibleCellScaling, mapData.chunkSize);
        return GenerateCollectibles(x, y);
    }
    ChunkData GenerateCollectibles(int chunkIndexX, int chunkIndexY)
    {
        ChunkData chunkData = new ChunkData(mapData.GetCollectibleTileCountInChunk());
        chunkData.chunkIndex = new Vector2Int(chunkIndexX, chunkIndexY);
        int scaledChunkSize = mapData.chunkSize / _collectibleCellSize;
        chunkData.size = scaledChunkSize;
        int perlinMapSize = mapData.perlinResolution * mapData.chunkSize;
        bool[,] CollectiblesPerlinMap = new bool[scaledChunkSize, scaledChunkSize];
        Vector3Int currentCellIndexLocal;
        for (int i = 0; i < perlinMapSize; i++)
        {
            for (int j = 0; j < perlinMapSize; j++)
            {
                currentCellIndexLocal = MapGenerator.GetScaledCellIndex(i, j, _collectibleCellSize, mapData.perlinResolution);

                bool perlinValue = MapGenerator.GetAbsolutePerlin(
                    i + perlinMapSize * chunkIndexX,
                    j + perlinMapSize * chunkIndexY,
                    mapData.perlinResolution, mapData.perlinScale, mapData.collectiblesPerlinThreshold);
                //Create coin where perlin value is obove threshold
                if (perlinValue &&
                    //and if in the current scaled cell we do not spawned collectible
                    !CollectiblesPerlinMap[currentCellIndexLocal.x, currentCellIndexLocal.y])
                {
                    Vector3Int pos = (currentCellIndexLocal * _collectibleCellSize)
                        + new Vector3Int(chunkIndexX * mapData.chunkSize, chunkIndexY * mapData.chunkSize, 0);
                    byte tileIndx = GetTileIndex();
                    chunkData.Tiles.Add(new ChunkData.tileInfo(pos, tileIndx));
                    CollectiblesPerlinMap[currentCellIndexLocal.x, currentCellIndexLocal.y] = true;
                }
            }
        }
       
        int tilesCount = chunkData.Tiles.Count;
        Vector3Int[] TilePositions = new Vector3Int[tilesCount];
        TileBase[] TilesToSet = new TileBase[tilesCount];
        for (int i = 0; i < tilesCount; i++)
        {
            TilePositions[i] = chunkData.Tiles[i].pos;
            TilesToSet[i] = mapData.Tiles[chunkData.Tiles[i].tileIndex];
        }
        _tilemap.SetTiles(TilePositions, TilesToSet);
        return chunkData;
    }
    
    byte GetTileIndex()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);

        Tile collectibleTile;
        if (rand > mapData.powerupChance)
        {
            collectibleTile = coinTile;
        }
        else
        {
            collectibleTile = Powerups[UnityEngine.Random.Range(0, Powerups.Count)];
        }
        //_tilemap.SetTile(gridPos, collectibleTile);
        return mapData.GetTileIndex(collectibleTile);
    }
    
    
}
