using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

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

    public static event Action OnTilePlaced;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        mouseTilemapPosition = GetMousePosition();
        TileBase tile = tilemap.GetTile(mouseTilemapPosition);

        //places a tile if the chosen position is valid and user has available block tiles
        if (Input.GetMouseButtonDown(0) && tile != null && tile.name == "Block_Tile" 
            && GameManager.GetComponent<GameManager>().blockTilesAvailable >= 1)
        {
            tilemap.SetTile(mouseTilemapPosition, null);
            tilemap.SetTile(mouseTilemapPosition, wallTile);
            OnTilePlaced?.Invoke();
            GameManager.GetComponent<GameManager>().blockTilesAvailable--;
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPosition);
    }


    public void MakeTileRedder(Vector3Int position)
    {
        Color currentColor = tilemap.GetColor(position);
        if (currentColor == default(Color)) currentColor = Color.white;

        float newR = Mathf.Min(currentColor.r * 1.5f, 1f);  // increase red
        float newG = currentColor.g * 0.85f;                 // reduce green
        float newB = currentColor.b * 0.85f;                 // reduce blue

        Color newColor = new Color(newR, newG, newB, currentColor.a);
        tilemap.SetColor(position, newColor);
    }




}
