using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public event System.Action onMapGenerated;
    public Tilemap WallTilemap => wallTilemap;
    public Tilemap CollectiblesTilemap => collectiblesTilemap;

    [SerializeField] private MapDataSO mapData;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap collectiblesTilemap;

    private List<ChunkData> chunksCollectibles = new List<ChunkData>();
    private Dictionary<Vector2Int, Vector3Int[]> chunksCollectiblesRemovedTiles
        = new Dictionary<Vector2Int, Vector3Int[]>();

    private List<ChunkData> chunksWalls = new List<ChunkData>();

    private Vector2Int currentChunk;
    private List<Vector2Int> loadedChunks;
    private List<Vector2Int> chunksNeeded;
    private int chunkCountSide;
    private static Vector2 randomPerlinOffset;

    private void Awake()
    {
        chunkCountSide = Mathf.CeilToInt((float)mapData.chunksLoadingRadius / mapData.chunkSize);
        int s = (chunkCountSide * 2 + 1);
        loadedChunks = new List<Vector2Int>(s * s + s);
        chunksNeeded = new List<Vector2Int>(s * s);
    }
    
    public void ClearAreaBox(Vector2 center, Vector2 halfExtends)
    {
        halfExtends = new Vector2(Mathf.Abs(halfExtends.x), Mathf.Abs(halfExtends.y));
        
        Vector3 bottomLeft = center + new Vector2(-halfExtends.x, -halfExtends.y);
        Vector3 topRight = center + new Vector2(halfExtends.x, halfExtends.y);

        ClearArea(bottomLeft, topRight);
    }

    private void ClearArea(Vector3 bottomLeft, Vector3 topRight)
    {
        Vector3Int bottomLeftTile = wallTilemap.WorldToCell(bottomLeft);
        Vector3Int topRightTile = wallTilemap.WorldToCell(topRight);
        for (int i = bottomLeftTile.x; i <= topRightTile.x; i++)
        {
            for (int j = bottomLeftTile.y; j <= topRightTile.y; j++)
            {
                wallTilemap.SetTile(new Vector3Int(i, j, 0), null);
            }
        }
    }

    public void DestroyWallPiece(Vector3Int tilePos, int tilesPerFrame)
    {
        StartCoroutine(DestroyWall(wallTilemap, tilePos, tilesPerFrame));
    }

    public Vector2Int WorldToChunk(Vector2 worldPos)
    {
        int chunkSize = mapData.chunkSize;
        int x = Mathf.FloorToInt(worldPos.x / chunkSize);
        int y = Mathf.FloorToInt(worldPos.y / chunkSize);
        return new Vector2Int(x, y);
    }

    private IEnumerator DestroyWall(Tilemap tilemap, Vector3Int tilePos, int tilesPerFrame)
    {
        List<Vector3Int> TilePositions = new List<Vector3Int>(10);

        Vector3Int currentTile = tilePos;
        TilePositions.Add(tilePos);
        int lastTileIndx = 0;
        int tilesCounter = 0;
        CheckAdjecent(tilemap, currentTile, TilePositions);
        while (TilePositions.Count - 1 > lastTileIndx)
        {
            lastTileIndx++;
            CheckAdjecent(tilemap, TilePositions[lastTileIndx], TilePositions);
        }
        for (int i = 0; i < TilePositions.Count; i++)
        {
            tilemap.SetTile(TilePositions[i], null);
            tilesCounter++;
            if(tilesCounter >= tilesPerFrame)
            {
                tilesCounter = 0;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void CheckAdjecent(Tilemap tilemap, Vector3Int tilePos, List<Vector3Int> TilePositions)
    {
        Vector3Int[] deltaSide = new Vector3Int[] {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, -1, 0),
        };
        for (int i = 0; i < 4; i++)
        {
            Vector3Int pos = tilePos + deltaSide[i];
            if (MapGenerator.CheckForTile(tilemap, pos))
            {
                if (!TilePositions.Contains(pos))
                {
                    TilePositions.Add(pos);
                }
            }
        }
    }
    public static bool CheckForTile(Tilemap tilemap, Vector3Int tilePos)
    {
        return tilemap.GetTile(tilePos) == null ? false : true;
    }

    public void UpdateChunks(Vector2Int currentChunk)
    {
        this.currentChunk = currentChunk;

        StartCoroutine(GenerateChunks());
    }

    public void OnDrawGizmos()
    {
        float chunkCenterOffset = mapData.chunkSize / 2;
        if(loadedChunks == null)
        {
            return;
        }
        foreach (var chunk in loadedChunks)
        {
            Gizmos.DrawWireCube(
                new Vector3(chunk.x * mapData.chunkSize + chunkCenterOffset, chunk.y * mapData.chunkSize + chunkCenterOffset, 0), new Vector3(mapData.chunkSize, mapData.chunkSize, 0));
        }
    }

    IEnumerator GenerateChunks()
    {
        Debug.Log("onMapGenerated start");

        int chunksLoadingRadius = chunkCountSide;
        int xStartChunk = currentChunk.x - chunksLoadingRadius;
        int xEndChunk = currentChunk.x + chunksLoadingRadius;
        int yStartChunk = currentChunk.y - chunksLoadingRadius;
        int yEndChunk = currentChunk.y + chunksLoadingRadius;
        chunksNeeded.Clear();
        for (int i = xStartChunk; i <= xEndChunk; i++)
        {
            for (int j = yStartChunk; j <= yEndChunk; j++)
            {
                chunksNeeded.Add(new Vector2Int(i, j));
            }
        }
        for (int i = 0; i < chunksNeeded.Count; i++)
        {
            if (!loadedChunks.Contains(chunksNeeded[i]))
            {
                //if chunk is not loaded - search if it was loaded
                Vector2Int chunkIndx = chunksNeeded[i];
                ChunkData collectiblesChunk = chunksCollectibles.Find(
                    ch => ch.chunkIndex == chunkIndx);
                if (collectiblesChunk != null)
                {
                    LoadChunk(collectiblesChunk, collectiblesTilemap);
                    RemoveCollectedTiles(chunkIndx);
                    chunksWalls.Add(
                        mapData.wallGenerator.CreateWallsChunk(chunkIndx.x, chunkIndx.y, wallTilemap));
                    loadedChunks.Add(chunkIndx);
                    yield return null;

                }
                else
                {
                    GenerateMapChunk(chunkIndx.x, chunkIndx.y);
                    loadedChunks.Add(chunkIndx);
                    yield return null;
                }

            }
        }
        List<Vector2Int> LoadedChunksCopy = new List<Vector2Int>(loadedChunks);
        for (int i = 0; i < LoadedChunksCopy.Count; i++)
        {
            Vector2Int chunkIndx = LoadedChunksCopy[i];
            if (!chunksNeeded.Contains(chunkIndx))
            {
                ChunkData collectiblesChunk = chunksCollectibles.Find(
                    ch => ch.chunkIndex == chunkIndx);
                ChunkData wallsChunk = chunksWalls.Find(
                    ch => ch.chunkIndex == chunkIndx);

                UnloadChunk(collectiblesChunk, collectiblesTilemap);
                UnloadChunk(wallsChunk, wallTilemap);
                loadedChunks.Remove(chunkIndx);
                yield return null;
            }
        }
        Debug.Log("onMapGenerated");
        onMapGenerated?.Invoke();
       
    }
    public ChunkData GetWallChunkData(Vector2Int chunkIndx)
    {
        for (int i = 0; i < chunksWalls.Count; i++)
        {
            if(chunksWalls[i].chunkIndex == chunkIndx)
            {
                return chunksWalls[i];
            }
        }
        return null;
    }
    public void ClearMap()
    {
        StartCoroutine(DeleteAllChunks());
    }
    private IEnumerator DeleteAllChunks()
    {
        List<Vector2Int> LoadedChunksCopy = new List<Vector2Int>(loadedChunks);
        loadedChunks.Clear();
        chunksNeeded.Clear();
        for (int i = 0; i < LoadedChunksCopy.Count; i++)
        {
            Vector2Int chunkIndx = LoadedChunksCopy[i];
            ChunkData collectiblesChunk = chunksCollectibles.Find(
                    ch => ch.chunkIndex == chunkIndx);
            ChunkData wallsChunk = chunksWalls.Find(
                ch => ch.chunkIndex == chunkIndx);

            UnloadChunk(collectiblesChunk, collectiblesTilemap);
            UnloadChunk(wallsChunk, wallTilemap);
            yield return null;
        }
        chunksCollectibles.Clear();
        chunksCollectiblesRemovedTiles.Clear();
        chunksWalls.Clear();
    }
    private void GenerateMapChunk(int chunkX, int chunkY)
    {
        chunksWalls.Add(
            mapData.wallGenerator.CreateWallsChunk(chunkX, chunkY, wallTilemap));
        ChunkData collectiblesData
            = mapData.collectibleGenerator.CreateCollectiblesChunk(chunkX, chunkY, collectiblesTilemap);
        chunksCollectibles.Add(collectiblesData);
        chunksCollectiblesRemovedTiles[new Vector2Int(chunkX, chunkY)] = new Vector3Int[collectiblesData.Tiles.Count];
    }

    public static void UnloadChunk(ChunkData chunkData, Tilemap tilemap)
    {
        int count = chunkData.Tiles.Count;
        Vector3Int[] TilesPos = new Vector3Int[count];
        for (int i = 0; i < count; i++)
        {
            TilesPos[i] = chunkData.Tiles[i].pos;
        }
        RemoveTiles(TilesPos, tilemap);
    }
    private static void RemoveTiles(Vector3Int[] Tiles, Tilemap tilemap)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            tilemap.SetTiles(Tiles,
                new TileBase[Tiles.Length]);
        }
    }
    private void RemoveCollectedTiles(Vector2Int chunk)
    {
        RemoveTiles(chunksCollectiblesRemovedTiles[chunk], collectiblesTilemap);

    }
    public void LoadChunk(ChunkData chunkData, Tilemap tilemap)
    {
        int tilesCount = chunkData.Tiles.Count;
        Vector3Int[] TilePositions = new Vector3Int[tilesCount];
        TileBase[] TilesToSet = new TileBase[tilesCount];
        for (int i = 0; i < tilesCount; i++)
        {
            TilePositions[i] = chunkData.Tiles[i].pos;
            TilesToSet[i] = mapData.Tiles[chunkData.Tiles[i].tileIndex];
        }
        tilemap.SetTiles(TilePositions, TilesToSet);
    }
    public static Vector3Int GetScaledCellIndex(int x, int y, float scale, float resolution)
    {
        Vector3Int cellIndx = new Vector3Int((int)(x / resolution / scale),
                    (int)(y / resolution / scale), 0);

        return cellIndx;
    }
    /*public static Vector2Int GetScaledCellIndex(Vector2 cellPos, float scale)
    {
        return new Vector2Int(Mathf.FloorToInt(cellPos.x / scale), Mathf.FloorToInt(cellPos.y / scale));
    }*/
    public static Vector2Int GetCellPos(Vector2 mapPos, float resolution)
    {
        return new Vector2Int(Mathf.FloorToInt(mapPos.x / resolution),
                    Mathf.FloorToInt(mapPos.y / resolution));
    }

    public static void GenerateRandomPerlinOffset()
    {
        randomPerlinOffset.x = Random.Range(-9999f, 9999f);
        randomPerlinOffset.y = Random.Range(-9999f, 9999f);
    }
    public static bool GetAbsolutePerlin(int x, int y, float resolution, float scale, float threshold)
    {
        float xCoord = (x / resolution * scale) + randomPerlinOffset.x;
        float yCoord = (y / resolution * scale) + randomPerlinOffset.y;
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
