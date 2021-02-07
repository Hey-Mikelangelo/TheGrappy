using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public SceneLoadingChannelSO sceneLoadingChannel;
    public MapDataSO mapData;
    public LinkerSO linker;
    public Tilemap wallTilemap;
    public Tilemap collectiblesTilemap;

    private Vector2Int _currentChunk;
    public List<ChunkData> ChunksCollectibles = new List<ChunkData>();
    public Dictionary<Vector2Int, List<Vector3Int>> ChunksCollectiblesRemovedTiles
        = new Dictionary<Vector2Int, List<Vector3Int>>();

    public List<ChunkData> ChunksWalls = new List<ChunkData>();

    public List<Vector2Int> _LoadedChunks;
    public List<Vector2Int> _ChunksNeeded;
    public int chunkCountSide;
    private void Awake()
    {
        chunkCountSide = Mathf.CeilToInt((float)mapData.chunksLoadingRadius / mapData.chunkSize);
        int s = (chunkCountSide * 2 + 1);
        _LoadedChunks = new List<Vector2Int>(s * s + s);
        _ChunksNeeded = new List<Vector2Int>(s * s);


    }
    
    public void ClearAreaBox(Vector3 bottomLeft, Vector3 topRight)
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
    public static bool CheckForTile(Tilemap tilemap, Vector3Int tilePos)
    {
        return tilemap.GetTile(tilePos) == null ? false : true;
    }
    public void UpdateChunks(Vector2Int currentChunk)
    {

        _currentChunk = currentChunk;

        StartCoroutine(GenerateChunks());
    }
    public void OnDrawGizmos()
    {
        float chunkCenterOffset = mapData.chunkSize / 2;
        foreach (var chunk in _LoadedChunks)
        {
            Gizmos.DrawWireCube(
                new Vector3(chunk.x * mapData.chunkSize + chunkCenterOffset, chunk.y * mapData.chunkSize + chunkCenterOffset, 0), new Vector3(mapData.chunkSize, mapData.chunkSize, 0));
        }
    }
    IEnumerator GenerateChunks()
    {
        int chunksLoadingRadius = chunkCountSide;
        int xStartChunk = _currentChunk.x - chunksLoadingRadius;
        int xEndChunk = _currentChunk.x + chunksLoadingRadius;
        int yStartChunk = _currentChunk.y - chunksLoadingRadius;
        int yEndChunk = _currentChunk.y + chunksLoadingRadius;
        _ChunksNeeded.Clear();
        for (int i = xStartChunk; i <= xEndChunk; i++)
        {
            for (int j = yStartChunk; j <= yEndChunk; j++)
            {
                _ChunksNeeded.Add(new Vector2Int(i, j));
            }
        }
        for (int i = 0; i < _ChunksNeeded.Count; i++)
        {
            if (!_LoadedChunks.Contains(_ChunksNeeded[i]))
            {
                //if chunk is not loaded - search if it was loaded
                Vector2Int chunkIndx = _ChunksNeeded[i];
                ChunkData collectiblesChunk = ChunksCollectibles.Find(
                    ch => ch.chunkIndex == chunkIndx);
                if (collectiblesChunk != null)
                {
                    LoadChunk(collectiblesChunk, collectiblesTilemap);
                    RemoveCollectedTiles(chunkIndx);
                    ChunksWalls.Add(
                        mapData.wallGenerator.CreateWallsChunk(chunkIndx.x, chunkIndx.y, wallTilemap));
                    _LoadedChunks.Add(chunkIndx);
                    yield return null;

                }
                else
                {
                    GenerateMapChunk(chunkIndx.x, chunkIndx.y);
                    _LoadedChunks.Add(chunkIndx);
                    yield return null;
                }

            }
        }
        List<Vector2Int> LoadedChunksCopy = new List<Vector2Int>(_LoadedChunks);
        for (int i = 0; i < LoadedChunksCopy.Count; i++)
        {
            Vector2Int chunkIndx = LoadedChunksCopy[i];
            if (!_ChunksNeeded.Contains(chunkIndx))
            {
                ChunkData collectiblesChunk = ChunksCollectibles.Find(
                    ch => ch.chunkIndex == chunkIndx);
                ChunkData wallsChunk = ChunksWalls.Find(
                    ch => ch.chunkIndex == chunkIndx);

                UnloadChunk(collectiblesChunk, collectiblesTilemap);
                UnloadChunk(wallsChunk, wallTilemap);
                _LoadedChunks.Remove(chunkIndx);
                yield return null;
            }
        }

        Scene scene = gameObject.scene;
        linker.gameEvents.OnMapGenerated();
        sceneLoadingChannel.SetSceneInited(scene.buildIndex);
    }
    void GenerateMapChunk(int chunkX, int chunkY)
    {
        ChunksWalls.Add(
            mapData.wallGenerator.CreateWallsChunk(chunkX, chunkY, wallTilemap));
        ChunkData collectiblesData
            = mapData.collectibleGenerator.CreateCollectiblesChunk(chunkX, chunkY, collectiblesTilemap);
        ChunksCollectibles.Add(collectiblesData);
        ChunksCollectiblesRemovedTiles[new Vector2Int(chunkX, chunkY)] = new List<Vector3Int>(collectiblesData.Tiles.Count);
    }

    public void UnloadChunk(ChunkData chunkData, Tilemap tilemap)
    {
        int count = chunkData.Tiles.Count;
        Vector3Int[] TilesPos = new Vector3Int[count];
        for (int i = 0; i < count; i++)
        {
            TilesPos[i] = chunkData.Tiles[i].pos;
        }
        RemoveTiles(TilesPos, tilemap);
    }
    void RemoveTiles(Vector3Int[] Tiles, Tilemap tilemap)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            tilemap.SetTiles(Tiles,
                new TileBase[Tiles.Length]);
        }
    }
    void RemoveCollectedTiles(Vector2Int chunk)
    {
        RemoveTiles(ChunksCollectiblesRemovedTiles[chunk].ToArray(), collectiblesTilemap);

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
    public static bool GetAbsolutePerlin(int x, int y, float resolution, float scale, float threshold)
    {
        float xCoord = (x / resolution * scale) + 1000;
        float yCoord = (y / resolution * scale) + 1000;
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
