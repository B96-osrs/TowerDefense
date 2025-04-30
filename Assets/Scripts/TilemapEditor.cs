using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEditor : MonoBehaviour
{
    private Vector3Int mouseTilemapPosition = new Vector3Int(0, 0, 0);
    public RuleTile wallTile;
    public Tilemap tilemap;

    public static event Action<Vector3Int> OnTilePlaced;

    void Update()
    {
        mouseTilemapPosition = GetMousePosition();
        TileBase tile = tilemap.GetTile(mouseTilemapPosition);

        //check if the mouse is clicked and the turret prefab is selected and the tile is a wall tile and the tile does not already have a cannon
        if (Input.GetMouseButtonDown(0) && tile.name == "White_Tile_0")
        {
            Debug.Log("Tile clicked" + mouseTilemapPosition);

            tilemap.SetTile(mouseTilemapPosition, wallTile);
            OnTilePlaced?.Invoke(mouseTilemapPosition);
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPosition);
    }



}
