using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//this script is used to let the user place tiles on the map
//while the game is running
//events are used to notify the GameManager
//enemies listen to events to adapt their pathfinding
public class TilemapEditor : MonoBehaviour
{
    private Vector3Int mouseTilemapPosition = new Vector3Int(0, 0, 0);
    public RuleTile wallTile;
    public Tilemap tilemap;
    private GameObject GameManager;

    public static event Action<Vector3Int> OnTilePlaced;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        mouseTilemapPosition = GetMousePosition();
        TileBase tile = tilemap.GetTile(mouseTilemapPosition);

        //places a tile if the chosen position is valid and user has available block tiles
        if (Input.GetMouseButtonDown(0) && tile.name == "Block_Tile" && GameManager.GetComponent<GameManager>().blockTilesAvailable >= 1)
        {
            tilemap.SetTile(mouseTilemapPosition, null);
            tilemap.SetTile(mouseTilemapPosition, wallTile);
            OnTilePlaced?.Invoke(mouseTilemapPosition);
            GameManager.GetComponent<GameManager>().blockTilesAvailable--;
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPosition);
    }



}
