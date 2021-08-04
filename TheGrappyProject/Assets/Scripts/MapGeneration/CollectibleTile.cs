using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CollectibleTile", menuName = "Tiles/Collectible Tile")]
public abstract class CollectibleTile : Tile {
    public Collectible collectible;
}

