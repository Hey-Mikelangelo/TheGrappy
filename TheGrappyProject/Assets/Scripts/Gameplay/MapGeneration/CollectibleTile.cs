using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CollectibleTile", menuName = "Tiles/Collectible Tile")]
public class CollectibleTile : Tile
{
    public Collectible type;
}
