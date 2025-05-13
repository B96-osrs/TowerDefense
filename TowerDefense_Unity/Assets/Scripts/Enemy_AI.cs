using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class Enemy_AI : MonoBehaviour
{
    public int maxHealth;
    public int killMoney;
    private int health;
    public Tilemap tilemap;
    public GameObject enemy;
    public float moveDelay = 1.0f;
    private HealthBar healthBar;
    private GameObject GameManager;
    Vector3 targetPosition;
    public float moveSpeed = 2f;
    private Vector3Int endTilePosition = new Vector3Int(9, -3, 0);
    private Vector3Int startTilePosition = new Vector3Int(-9, 3, 0);
    private List<Node> finalPath = new List<Node>();

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        transform.position = tilemap.GetCellCenterWorld(startTilePosition);
        targetPosition = tilemap.GetCellCenterWorld(startTilePosition);
        getAIPath();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            moveEnemy();
        }
    }

    void OnEnable()
    {
        TilemapEditor.OnTilePlaced += HandleTilePlaced;
    }

    void OnDisable()
    {
        TilemapEditor.OnTilePlaced -= HandleTilePlaced;
    }

    void HandleTilePlaced(Vector3Int position)
    {
        getAIPath();
    }

    private void moveEnemy()
    {
        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            GameManager.GetComponent<GameManager>().hitpoints -= 1;
            Destroy(gameObject);
            return;
        }
        if (finalPath.Count > 0)
        {
            targetPosition = tilemap.GetCellCenterWorld(finalPath[0].position);
            finalPath.RemoveAt(0);
        }
    }

    void getAIPath()
    {
        var mazeData = new
        {
            path_tiles = getPathTiles().Select(pos => new { x = pos.x, y = pos.y }).ToArray(),
            wall_tiles = getWallTiles().Select(pos => new { x = pos.x, y = pos.y }).ToArray(),
            maze_height = tilemap.size.y,
            startTile = new { x = startTilePosition.x, y = startTilePosition.y },
            endTile = new { x = endTilePosition.x, y = endTilePosition.y },
            //turrets = GetTurretPositions().Select(pos => new { x = pos.x, y = pos.y }).ToArray()
        };

        string mazeJson = JsonConvert.SerializeObject(mazeData, Formatting.None);

        var requestPayload = new
        {
            model = "qwen3:4b",
            prompt = $"Given the following maze data from a maze made in Unity with a tilemap:\n{mazeJson}\nFind the shortest path from start to end, avoiding turrets if there are any. Return the path as a JSON array of coordinate objects like {{\"x\":0,\"y\":0}}.",
            stream = false
        };

        string finalJson = JsonConvert.SerializeObject(requestPayload);
        StartCoroutine(SendQwenRequest(finalJson));
    }

    IEnumerator SendQwenRequest(string jsonPayload)
    {
        Debug.Log("Starting SendQwenRequest coroutine with payload: " + jsonPayload);

        string url = "http://localhost:11434/api/generate";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending request: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            ProcessPathFromOllamaResponse(request.downloadHandler.text);
        }
    }


    void ProcessPathFromOllamaResponse(string jsonResponse)
    {
        Debug.Log("Processing response: " + jsonResponse);
        try
        {
            JObject outer = JObject.Parse(jsonResponse);
            string responseText = outer["response"].ToString();
            JArray path = JArray.Parse(responseText);
            finalPath.Clear();
            foreach (var node in path)
            {
                int x = (int)node["x"];
                int y = (int)node["y"];
                finalPath.Add(new Node(new Vector3Int(x, y, 0), null));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse Ollama response: " + e.Message);
        }
    }


    List<Vector3Int> GetTurretPositions()
    {
        List<Vector3Int> turrets = new List<Vector3Int>();
        foreach (GameObject turret in GameObject.FindGameObjectsWithTag("Turret"))
        {
            Vector3Int pos = tilemap.WorldToCell(turret.transform.position);
            turrets.Add(pos);
        }
        return turrets;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().money += killMoney;
            GameManager.GetComponent<GameManager>().enemiesKilled++;
        }
    }


    public List<Vector2Int> getPathTiles()
    {
        List<Vector2Int> pathTiles = new List<Vector2Int>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos) && tilemap.GetTile(pos).name != "Wall_Tile")
            {
                pathTiles.Add(new Vector2Int(pos.x, pos.y));
            }
        }

        Debug.Log("Path Tiles: " + string.Join(", ", pathTiles));
        return pathTiles;
    }


    public List<Vector2Int> getWallTiles()
    {
        List<Vector2Int> wallTiles = new List<Vector2Int>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos) && tilemap.GetTile(pos).name == "Wall_Tile")
            {
                wallTiles.Add(new Vector2Int(pos.x, pos.y));
            }
        }

        Debug.Log("Wall Tiles: " + string.Join(", ",wallTiles));
        return wallTiles;
    }
}
