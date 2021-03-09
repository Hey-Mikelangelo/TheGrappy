using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkData 
{
    public struct tileInfo
    {
        public Vector3Int pos;
        public byte tileIndex;
        public tileInfo(Vector3Int pos, byte index)
        {
            this.pos = pos;
            this.tileIndex = index;
        }
    }
    public ChunkData(int tileCount)
    {
        Tiles = new List<tileInfo>(tileCount);
    }
    public static Dictionary<int, TileBase> TilesIndexes;
    public Vector2Int chunkIndex;
    public int scaledChunkSize;
    public List<tileInfo> Tiles;
}
