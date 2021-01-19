using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;

public class MapGeneratorManager : MonoBehaviour
{
    public struct ChunkData
    {
        public List<GameObject> grapPoints;
    }
    public bool generateOnChange;
    [SerializeField] private Renderer perlinRenderer;
    [SerializeField] private Renderer cellRenderer;
    [Header("Grid")]
    [Range(1, 100)]
    public int chunkSize;
    public List<ChunkData> Chunks;
    [Header("Grap Points")]
    public GameObject grapPoint;
    public Transform grapPointsHolder;
    public float shiftX;
    public float shiftY;
    public int resolution;
    public float scale;
    [Range(0,0.5f)]
    public float threshold;
    [Range(1, 10)] public int grapPointCellScaling;
    [ShowOnly] public int grapPointCellSize;
    [Range(1, 100)]
    public float minGrapPointsDistance;
    [Range(1, 100)]
    public float maxGrapPointsDistance;

    public int chunkX, chunkY;
    private void OnEnable()
    {
        Chunks = new List<ChunkData>(4);
    }
    private void OnValidate()
    {

    }
    public void GenerateChunk(int chunkIndexX, int chunkIndexY)
    {
        Vector3 zeroPos = new Vector3(chunkIndexX * chunkSize, chunkIndexY * chunkSize);
        GenerateGrapChunk(chunkX, chunkY, resolution, scale, threshold);
        /*chunkY++;
        chunkX++;*/
    }
    /*Vector2 WorldToPerlin(Vector2 worldPos, float perlinResolution, float perlinScale)
    {

    }*/
    Vector2 GetCellPos(Vector2 mapPos, float resolution)
    {
        return new Vector2(math.floor(mapPos.x / resolution),
                    math.floor(mapPos.y / resolution));
    }
    Vector2 GetScaledCellIndex(Vector2 cellPos, float scale)
    {
        return new Vector2(math.floor(cellPos.x / scale), math.floor(cellPos.y / scale));
    }
    void CalculateGrapPointCellSize()
    {
        for (int s = grapPointCellScaling; s >= 1; s--)
        {
            if (chunkSize % s != 0)
            {
                continue;
            }
            else
            {
                grapPointCellSize = s;
                break;
            }
        }
    }
    List<GameObject> GrapPoints = new List<GameObject>(1000);
    void DeletePoints()
    {
        foreach (var point in GrapPoints)
        {
            DestroyImmediate(point);
        }
        GrapPoints.Clear();

    }
    void GenerateGrapChunk(int chunkIndexX, int chunkIndexY,
        int perlinResolution = 3, float perlinScale = 0.5f, float threshold = 0.07f)
    {
        DeletePoints();
        CalculateGrapPointCellSize();
        int scaledChunkSize = chunkSize / grapPointCellSize;
        int perlinMapSize = perlinResolution * chunkSize;
        bool[,] GrapPerlinMap = new bool[scaledChunkSize, scaledChunkSize];
        bool[,] PerlinMap = new bool[perlinResolution*chunkSize, perlinResolution * chunkSize];
        
        Vector2 currentCell, currentCellIndex;
        Texture2D cellTexture = new Texture2D(scaledChunkSize, scaledChunkSize);
        Texture2D texture = new Texture2D(perlinMapSize, perlinMapSize);
        for (int i = 0; i < perlinMapSize; i++)
        {
            for (int j = 0; j < perlinMapSize; j++)
            {
                currentCell = GetCellPos(new Vector2(i, j), perlinResolution);
                currentCellIndex = GetScaledCellIndex(currentCell, grapPointCellSize);

                bool perlinValue = GetAbsolutePerlin(
                    i + perlinMapSize * chunkIndexX, 
                    j + perlinMapSize * chunkIndexY, 
                    perlinResolution, perlinScale, threshold);

                PerlinMap[i, j] = perlinValue;
                float whiteness = perlinValue ? 1 : 0;
                texture.SetPixel(i, j, new Color(whiteness, whiteness, whiteness));
                if (!perlinValue && !GrapPerlinMap[(int)currentCellIndex.x, (int)currentCellIndex.y])
                {
                    Vector3 pos = (currentCellIndex * grapPointCellSize) 
                        + new Vector2((float)grapPointCellSize / 2, (float)grapPointCellSize / 2);
                    GameObject point = (Instantiate(grapPoint, pos, Quaternion.identity, grapPointsHolder));
                    point.transform.localScale = new Vector3(grapPointCellSize, grapPointCellSize, grapPointCellSize);
                    GrapPoints.Add(point);
                    GrapPerlinMap[(int)currentCellIndex.x, (int)currentCellIndex.y] = true;
                    cellTexture.SetPixel((int)currentCellIndex.x, (int)currentCellIndex.y, new Color(0, 0, 0));
                }
            }
        }
        cellTexture.filterMode = FilterMode.Point;
        cellTexture.Apply();
        cellRenderer.sharedMaterial.mainTexture = cellTexture;

        texture.filterMode = FilterMode.Point;
        texture.Apply();
        perlinRenderer.sharedMaterial.mainTexture = texture;
    }
    bool GetAbsolutePerlin(int x, int y, float resolution, float scale, float threshold)
    {
        float xCoord = (x / resolution * scale) + 1000 + shiftX;
        float yCoord = (y / resolution * scale) + 1000 + shiftY;
        float perlin = Mathf.PerlinNoise(xCoord, yCoord);
        return perlin > threshold ? true : false;
    }
    
    void Update()
    {
        
    }
}
