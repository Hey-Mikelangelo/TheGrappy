using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CollectibleGeneratorSO", menuName = "Game/MapGeneration/Collectible Generator")]
public class CollectibleGeneratorSO : ScriptableObject
{
    public WallGeneratorSO wallGenerator;
    public MapDataSO mapData;
    [Header("Collectibles")]
    public Tile coinTile; //tile index = 100
    public List<CollectibleTile> Powerups = new List<CollectibleTile>();
    private Tilemap tilemap;

    private int _collectibleCellSize;
    public ChunkData CreateCollectiblesChunk(int x, int y, Tilemap tilemap)
    {
        this.tilemap = tilemap;
        _collectibleCellSize = MapGenerator.CalculateCellSize(mapData.collectibleCellScaling, mapData.chunkSize);
        return GenerateCollectibles(x, y);
    }
    ChunkData GenerateCollectibles(int chunkIndexX, int chunkIndexY)
    {
        ChunkData chunkData = new ChunkData(mapData.GetCollectibleTileCountInChunk());
        chunkData.chunkIndex = new Vector2Int(chunkIndexX, chunkIndexY);
        int scaledChunkSize = mapData.chunkSize / _collectibleCellSize;
        chunkData.scaledChunkSize = scaledChunkSize;
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
                    Vector3 pos = (currentCellIndexLocal * _collectibleCellSize)
                        + new Vector3Int(chunkIndexX * mapData.chunkSize, chunkIndexY * mapData.chunkSize, 0);
                    Vector3Int gridPos = tilemap.WorldToCell(pos);
                    byte tileIndx = GetTileIndex();
                    chunkData.Tiles.Add(new ChunkData.tileInfo(gridPos, tileIndx));
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
        tilemap.SetTiles(TilePositions, TilesToSet);
        return chunkData;
    }
    private byte GetTileIndex()
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
        return mapData.GetTileIndex(collectibleTile);
    }


}
