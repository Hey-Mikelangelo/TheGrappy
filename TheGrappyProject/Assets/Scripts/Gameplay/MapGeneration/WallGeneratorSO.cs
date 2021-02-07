using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WallGeneratorSO", menuName = "Game/MapGeneration/Wall Generator")]
public class WallGeneratorSO : ScriptableObject
{
    public MapDataSO mapData;
    [Header("Walls")]
    //public GameObject wallPiecePrefab;
    public Tile wallTile;
    private int _wallCellSize;
    private Tilemap _tilemap;

    public ChunkData CreateWallsChunk(int chunkX, int chunkY, Tilemap tilemap)
    {
        _tilemap = tilemap;
        _wallCellSize = MapGenerator.CalculateCellSize(mapData.wallCellScaling, mapData.chunkSize);
        return GenerateWalls(chunkX, chunkY);
    }
    
    ChunkData GenerateWalls(int chunkIndexX, int chunkIndexY)
    {
        ChunkData chunkData = new ChunkData(mapData.GetWallTileCountInChunk());
        chunkData.chunkIndex = new Vector2Int(chunkIndexX, chunkIndexY);
        byte wallTileIndex = mapData.GetTileIndex(wallTile);
        int scaledChunkSize = mapData.chunkSize / _wallCellSize;
        chunkData.size = scaledChunkSize;
        int perlinMapSize = mapData.perlinResolution * mapData.chunkSize;
        bool[,] WallsPerlinMap = new bool[scaledChunkSize, scaledChunkSize];
        Vector3Int currentCellIndexLocal;
        for (int i = 0; i < perlinMapSize; i++)
        {
            for (int j = 0; j < perlinMapSize; j++)
            {
                currentCellIndexLocal = MapGenerator.GetScaledCellIndex(i, j, _wallCellSize, mapData.perlinResolution);
                bool perlinValue = MapGenerator.GetAbsolutePerlin(
                    i + perlinMapSize * chunkIndexX,
                    j + perlinMapSize * chunkIndexY,
                    mapData.perlinResolution, mapData.perlinScale, mapData.wallPerlinThreshold);
                //Create wall where perlin value is below threshold
                if (!perlinValue && 
                    //and if in the current scaled cell we do not spawned wall
                    !WallsPerlinMap[currentCellIndexLocal.x, currentCellIndexLocal.y])
                {
                    Vector3Int pos = (currentCellIndexLocal * _wallCellSize)
                        + new Vector3Int(chunkIndexX * mapData.chunkSize, chunkIndexY * mapData.chunkSize, 0);

                    Vector3Int gridTilePos = _tilemap.WorldToCell(pos);
                    //Vector2Int[] Cells = CreateCellFromTiles(gridTilePos, _wallCellSize);
                    for (int q = 0; q < _wallCellSize; q++)
                    {
                        for (int w = 0; w < _wallCellSize; w++)
                        {
                            Vector3Int tilePos = gridTilePos + new Vector3Int(q, w, 0);
                            chunkData.Tiles.Add(new ChunkData.tileInfo(
                                tilePos, wallTileIndex));
                        }
                    }
                    WallsPerlinMap[currentCellIndexLocal.x, currentCellIndexLocal.y] = true;
                }
            }
        }
        int tilesCount = chunkData.Tiles.Count;
        Vector3Int[] TilePositions = new Vector3Int[tilesCount];
        TileBase[] TilesToSet = new TileBase[tilesCount];
        for (int i = 0; i < tilesCount; i++)
        {
            TilePositions[i] = chunkData.Tiles[i].pos;
            TilesToSet[i] = wallTile;
        }
        _tilemap.SetTiles(TilePositions, TilesToSet);
        return chunkData;
    }
    
}
