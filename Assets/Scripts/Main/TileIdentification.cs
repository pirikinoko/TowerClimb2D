using UnityEngine;
using UnityEngine.Tilemaps;

public class TileIdentification : MonoBehaviour
{
    public Tilemap tilemap;

    void Update()
    {
        Vector3Int gridPosition = tilemap.WorldToCell(transform.position);
        TileBase tile = tilemap.GetTile(gridPosition);

        if (tile != null)
        {
            // タイルが存在する場合の処理
            Debug.Log("Tile ID: " + tile.name);
        }
    }
}