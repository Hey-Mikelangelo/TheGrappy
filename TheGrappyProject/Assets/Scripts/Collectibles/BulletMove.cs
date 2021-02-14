using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletMove : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float timeElapsed;
    private void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if wall
        if (collision.gameObject.layer == 8)
        {
            Tilemap collectiblesTilemap = collision.gameObject.GetComponent<Tilemap>();

            Vector3Int tilePos = collectiblesTilemap.WorldToCell(
                    collision.GetContact(0).point);

            StartCoroutine(DestroyWall(collectiblesTilemap, tilePos));
        }
    }
    IEnumerator DestroyWall(Tilemap tilemap, Vector3Int tilePos)
    {
        List<Vector3Int> TilePositions = new List<Vector3Int>(10);

        Vector3Int currentTile = tilePos;
        TilePositions.Add(tilePos);
        int lastTileIndx = 0;
        CheckAdjecent(tilemap, currentTile, TilePositions);
        while (TilePositions.Count - 1 > lastTileIndx)
        {
            lastTileIndx++;
            CheckAdjecent(tilemap, TilePositions[lastTileIndx], TilePositions);
        }
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        speed = 0;
        for (int i = 0; i < TilePositions.Count; i++)
        {
            tilemap.SetTile(TilePositions[i], null);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
    void CheckAdjecent(Tilemap tilemap, Vector3Int tilePos, List<Vector3Int> TilePositions)
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
}
