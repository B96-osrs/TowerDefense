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
    private GameObject GameManager;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }
    void Update()
    {
        mousePosition = GetMousePosition();
        if (Input.GetMouseButtonDown(0) && turretPrefab != null)
        {
            Instantiate(turretPrefab, tilemap.GetCellCenterWorld(mousePosition), Quaternion.identity);
            turretPrefab = null;
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPosition);
    }

    public void SelectBasicTurret()
    {
        if(GameManager.GetComponent<GameManager>().money >= 500)
        {
            turretPrefab = basicTurret;
            GameManager.GetComponent<GameManager>().money -= 500;
        }
        
    }

    public void SelectSpeedTurret()
    {
        if (GameManager.GetComponent<GameManager>().money >= 1200)
        {
            turretPrefab = speedTurret;
            GameManager.GetComponent<GameManager>().money -= 1200;
        }
        
    }

    public void SelectMegaTurret()
    {
        if (GameManager.GetComponent<GameManager>().money >= 2500)
        {
            turretPrefab = speedTurret;
            GameManager.GetComponent<GameManager>().money -= 2500;
        }
    }
}
