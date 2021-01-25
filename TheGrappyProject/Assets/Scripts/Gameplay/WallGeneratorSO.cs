using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "WallGeneratorSO", menuName = "Game/MapGeneration/Wall Generator")]
public class WallGeneratorSO : ScriptableObject
{
    public MapDataSO mapData;
    [Header("Walls")]
    public GameObject wallPiecePrefab;
    private int _wallCellSize;

    public void CreateWalls(int chunkX, int chunkY)
    {
        _wallCellSize = MapGenerator.CalculateCellSize(mapData.wallCellScaling, mapData.chunkSize);
        GenerateWalls(chunkX, chunkY);
    }

    void GenerateWalls(int chunkIndexX, int chunkIndexY)
    {
        int scaledChunkSize = mapData.chunkSize / _wallCellSize;
        int perlinMapSize = mapData.perlinResolution * mapData.chunkSize;
        bool[,] WallsPerlinMap = new bool[scaledChunkSize, scaledChunkSize];
        Vector2 currentCellLocal, currentCellIndexLocal;
        for (int i = 0; i < perlinMapSize; i++)
        {
            for (int j = 0; j < perlinMapSize; j++)
            {
                currentCellLocal = MapGenerator.GetCellPos(new Vector2(i, j), mapData.perlinResolution);
                currentCellIndexLocal = MapGenerator.GetScaledCellIndex(currentCellLocal, _wallCellSize);
                bool perlinValue = MapGenerator.GetAbsolutePerlin(
                    i + perlinMapSize * chunkIndexX,
                    j + perlinMapSize * chunkIndexY,
                    mapData.perlinResolution, mapData.perlinScale, mapData.wallPerlinThreshold);
                //Create wall where perlin value is below threshold
                if (!perlinValue && 
                    //and if in the current scaled cell we do not spawned wall
                    !WallsPerlinMap[(int)currentCellIndexLocal.x, (int)currentCellIndexLocal.y])
                {
                    Vector3 pos = (currentCellIndexLocal * _wallCellSize)
                        + new Vector2(chunkIndexX * mapData.chunkSize, chunkIndexY * mapData.chunkSize)
                        + new Vector2((float)_wallCellSize / 2, (float)_wallCellSize / 2);
                    GameObject point = (Instantiate(wallPiecePrefab, pos, Quaternion.identity));
                    point.transform.localScale = new Vector3(_wallCellSize, _wallCellSize, _wallCellSize);
                    WallsPerlinMap[(int)currentCellIndexLocal.x, (int)currentCellIndexLocal.y] = true;
                }
            }
        }
    }
}
