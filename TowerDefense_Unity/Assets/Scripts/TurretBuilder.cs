using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurretBuilder : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject basicTurret;
    public GameObject speedTurret;
    public GameObject megaTurret;
    private GameObject turretPrefab;
    private GameObject GameManager;

    private Vector3Int mousePosition = new Vector3Int(0, 0, 0);
    private int turretPrefabCost;

    public static event Action OnTurretPlaced;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }
    void Update()
    {
        mousePosition = GetMousePosition();
        TileBase tile = tilemap.GetTile(mousePosition);

        //check if the mouse is clicked and the turret prefab is selected and the tile is a wall tile and the tile does not already have a cannon

        if (Input.GetMouseButtonDown(0) && tileIsEmpty(mousePosition) && tile != null && tile.name == "Wall_Tile" && turretPrefab != null)
        {
            Instantiate(turretPrefab, tilemap.GetCellCenterWorld(mousePosition), Quaternion.identity);
            GameManager.GetComponent<GameManager>().money -= turretPrefabCost;
            GameManager.GetComponent<GameManager>().moneySpent += turretPrefabCost;
            OnTurretPlaced?.Invoke();
            turretPrefab = null;
        }

    }
    //returns the mouse position in tilemap coordinates
    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPosition);
    }


    public bool tileIsEmpty(Vector3 mousePosition)
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in turrets)
        {
            if (tilemap.WorldToCell(turret.transform.position) == mousePosition)
            {
                return false;
            }
        }
        return true;
    }

    public void SelectBasicTurret()
    {
        if (GameManager.GetComponent<GameManager>().money >= 500)
        {
            turretPrefab = basicTurret;
            turretPrefabCost = 500;
        }

    }

    public void SelectSpeedTurret()
    {
        if (GameManager.GetComponent<GameManager>().money >= 1200)
        {
            turretPrefab = speedTurret;
            turretPrefabCost = 1200;
        }

    }

    public void SelectMegaTurret()
    {
        if (GameManager.GetComponent<GameManager>().money >= 2500)
        {
            turretPrefab = megaTurret;
            turretPrefabCost = 2500;
        }
    }
}
