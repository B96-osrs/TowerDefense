using UnityEngine;
using UnityEngine.Tilemaps;

public class TurretBuilder : MonoBehaviour
{
    private Vector3Int mousePosition = new Vector3Int(0, 0, 0);
    public Tilemap tilemap;
    public GameObject basicTurret;
    public GameObject speedTurret;
    public GameObject megaTurret;
    private GameObject turretPrefab;
    private int turretPrefabCost;
    private GameObject GameManager;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }
    void Update()
    {
        mousePosition = GetMousePosition();
        TileBase tile = tilemap.GetTile(mousePosition);

        //check if the mouse is clicked and the turret prefab is selected and the tile is a wall tile and the tile does not already have a cannon
        if (Input.GetMouseButtonDown(0) && turretPrefab != null && tile.name == "Wall_Tile" && tileIsEmpty(mousePosition))
        {
            Instantiate(turretPrefab, tilemap.GetCellCenterWorld(mousePosition), Quaternion.identity);
            GameManager.GetComponent<GameManager>().money -= turretPrefabCost;
            turretPrefab = null;
        }
    }

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
                Debug.Log("Turret already exists here");
                return false;
            }
        }
        Debug.Log("No turret here");
        return true;
    }

    public void SelectBasicTurret()
    {
        if(GameManager.GetComponent<GameManager>().money >= 500)
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
