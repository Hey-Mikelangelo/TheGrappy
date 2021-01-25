using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleGeneratorSO", menuName = "Game/MapGeneration/Collectible Generator")]
public class CollectibleGeneratorSO : ScriptableObject
{
    public WallGeneratorSO wallGenerator;
    public MapDataSO mapData;
    [Header("Collectibles")]
    public GameObject coinPrefab;

    private int _coinCellSize;
    public void CreateCollectibles(int x, int y)
    {
        _coinCellSize = MapGenerator.CalculateCellSize(mapData.coinsCellScaling, mapData.chunkSize);
        GenerateCoins(x, y);
    }
    void GenerateCoins(int chunkIndexX, int chunkIndexY)
    {
        int scaledChunkSize = mapData.chunkSize / _coinCellSize;
        int perlinMapSize = mapData.perlinResolution * mapData.chunkSize;
        bool[,] CoinsPerlinMap = new bool[scaledChunkSize, scaledChunkSize];
        Vector2 currentCellLocal, currentCellIndexLocal;
        for (int i = 0; i < perlinMapSize; i++)
        {
            for (int j = 0; j < perlinMapSize; j++)
            {
                currentCellLocal = MapGenerator.GetCellPos(new Vector2(i, j), mapData.perlinResolution);
                currentCellIndexLocal = MapGenerator.GetScaledCellIndex(currentCellLocal, _coinCellSize);
                bool perlinValue = MapGenerator.GetAbsolutePerlin(
                    i + perlinMapSize * chunkIndexX,
                    j + perlinMapSize * chunkIndexY,
                    mapData.perlinResolution, mapData.perlinScale, mapData.coinsPerlinThreshold);
                //Create coin where perlin value is obove threshold
                if (perlinValue &&
                    //and if in the current scaled cell we do not spawned coin
                    !CoinsPerlinMap[(int)currentCellIndexLocal.x, (int)currentCellIndexLocal.y])
                {
                    Vector3 pos = (currentCellIndexLocal * _coinCellSize)
                        + new Vector2(chunkIndexX * mapData.chunkSize, chunkIndexY * mapData.chunkSize)
                        + new Vector2((float)_coinCellSize / 2, (float)_coinCellSize / 2);
                    GameObject point = (Instantiate(coinPrefab, pos, Quaternion.identity));
                    //point.transform.localScale = new Vector3(_coinCellSize, _coinCellSize, _coinCellSize);
                    CoinsPerlinMap[(int)currentCellIndexLocal.x, (int)currentCellIndexLocal.y] = true;
                }
            }
        }
    }
    
}
