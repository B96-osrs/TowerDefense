using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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


        if (Input.GetMouseButtonDown(0) && tile.name == "Block_Tile" && GameManager.GetComponent<GameManager>().blockTilesAvailable >= 1)
        {
            Debug.Log("Trying to place tile, " + GameManager.GetComponent<GameManager>().blockTilesAvailable + "tiles available");
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
