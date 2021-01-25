using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public SceneLoadingManagerSO SceneLoadingManager;
    public MapDataSO mapData;

    public Vector2 chunks;
    
    private void Start()
    {
        GenerateChunks();
    }
    IEnumerator TryGenerateChunks()
    {
        Scene thisScene = gameObject.scene;
        while (SceneManager.GetActiveScene() != (thisScene))
        {
            yield return null;
        }
        GenerateChunks();
    }
    void GenerateChunks()
    {
        for (int i = 0; i < chunks.x; i++)
        {
            for (int j = 0; j < chunks.y; j++)
            {
                GenerateMapChunk(i, j);
                //mapData.wallGenerator.CreateWalls(i, j);
            }
        }
        Scene scene = gameObject.scene;
        SceneLoadingManager.SetSceneInited((sceneInfo)scene);
    }
    void GenerateMapChunk(int chunkX, int chunkY)
    {
        mapData.wallGenerator.CreateWalls(chunkX, chunkY);
        mapData.collectibleGenerator.CreateCollectibles(chunkX, chunkY);
        
    }
    public static Vector2 GetScaledCellIndex(Vector2 cellPos, float scale)
    {
        return new Vector2(math.floor(cellPos.x / scale), math.floor(cellPos.y / scale));
    }
    public static Vector2 GetCellPos(Vector2 mapPos, float resolution)
    {
        return new Vector2(math.floor(mapPos.x / resolution),
                    math.floor(mapPos.y / resolution));
    }
    public static bool GetAbsolutePerlin(int x, int y, float resolution, float scale, float threshold)
    {
        float xCoord = (x / resolution * scale) + 1000;// + shiftX;
        float yCoord = (y / resolution * scale) + 1000;// + shiftY;
        float perlin = Mathf.PerlinNoise(xCoord, yCoord);
        return perlin > threshold ? true : false;
    }
    public static int CalculateCellSize(int scaling, int chunkSize)
    {
        int cellSize = 1;
        for (int s = scaling; s >= 1; s--)
        {
            if (chunkSize % s != 0)
            {
                continue;
            }
            else
            {
                cellSize = s;
                break;
            }
        }
        return cellSize;
    }
}
