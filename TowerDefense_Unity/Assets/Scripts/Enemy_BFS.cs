using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy_BFS : MonoBehaviour
{
    // Public configuration variables
    public int maxHealth;
    public int killMoney;
    public float moveDelay = 1.0f;
    public float moveSpeed = 2f;
    public Tilemap tilemap;
    public GameObject enemy;

    // State and pathfinding variables
    private int health;
    private Vector3Int startTilePosition = new Vector3Int(-9, 3, 0);
    private Vector3Int endTilePosition = new Vector3Int(9, -3, 0);
    private Vector3 targetPosition;
    private List<Vector3Int> finalPath = new List<Vector3Int>();

    // private reference variables
    private HealthBar healthBar;
    private GameObject GameManager;


    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        transform.position = tilemap.GetCellCenterWorld(startTilePosition);
        targetPosition = tilemap.GetCellCenterWorld(startTilePosition);
        findPathBFS();

    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            moveEnemy();
        }

    }
    //Eventhandler calculates the path when a tile is placed
    void OnEnable()
    {
        TilemapEditor.OnTilePlaced += HandleTilePlaced;
    }

    void OnDisable()
    {
        TilemapEditor.OnTilePlaced -= HandleTilePlaced;
    }

    void HandleTilePlaced()
    {
        findPathBFS();
        Debug.Log("Tilemap raised event" );

    }


    public void takeDamage(int damage)
    {
        health = health - damage;
        Debug.Log("Enemy HealthBar reference: " + healthBar);
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().money += killMoney;
            GameManager.GetComponent<GameManager>().enemiesKilled++;
        }
    }


    private void findPathBFS()
    {
        finalPath.Clear();
        int loopCounter = 0;
        Vector3Int currentNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = endTilePosition;
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>(); //to keep track of visited nodes
        Queue<Vector3Int> q = new Queue<Vector3Int>(); //keep track of nodes to visit
        Dictionary<Vector3Int, Vector3Int> parentMap = new Dictionary<Vector3Int, Vector3Int>(); //to reconstruct the path
        q.Enqueue(currentNode);
        visited.Add(currentNode);

        while (q.Count > 0)
        {
            loopCounter++;
            currentNode = q.Dequeue();

            //if we reach our end tile
            if (currentNode == endNode)
            {
                Debug.Log("Path found");
                finalPath = (ReconstructPath(parentMap));
                break;
            }


            Vector3Int topNode = new Vector3Int(currentNode.x, currentNode.y + 1, 0);
            Vector3Int rightNode = new Vector3Int(currentNode.x + 1, currentNode.y, 0);
            Vector3Int bottomNode = new Vector3Int(currentNode.x, currentNode.y - 1, 0);
            Vector3Int leftNode = new Vector3Int(currentNode.x - 1, currentNode.y, 0);


            //check if the top tile is walkable
            TileBase topTile = tilemap.GetTile(topNode);
            if (topTile != null && topTile.name != "Wall_Tile" && !visited.Contains(topNode))
            {
                q.Enqueue(topNode);
                visited.Add(topNode);
                parentMap[topNode] = currentNode;
            }

            //check if the right tile is walkable
            TileBase rightTile = tilemap.GetTile(rightNode);
            if (rightTile != null && rightTile.name != "Wall_Tile" && !visited.Contains(rightNode))
            {
                q.Enqueue(rightNode);
                visited.Add(rightNode);
                parentMap[rightNode] = currentNode;
            }

            //check if the bottom tile is walkable
            TileBase bottomTile = tilemap.GetTile(bottomNode);
            if (bottomTile != null && bottomTile.name != "Wall_Tile" && !visited.Contains(bottomNode))
            {
                q.Enqueue(bottomNode);
                visited.Add(bottomNode);
                parentMap[bottomNode] = currentNode;
            }

            //check if the left tile is walkable
            TileBase leftTile = tilemap.GetTile(leftNode);
            if (leftTile != null && leftTile.name != "Wall_Tile" && !visited.Contains(leftNode))
            {
                q.Enqueue(leftNode);
                visited.Add(leftNode);
                parentMap[leftNode] = currentNode;
            }
        } //while
    } //FindPathBFS

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> parentMap)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int current = endTilePosition;
        while (current != tilemap.WorldToCell(transform.position))
        {
            path.Add(current);
            current = parentMap[current];
        }

        path.Reverse();
        Debug.Log(path.ToArray());
        return path;
    }
    private void moveEnemy()
    {
        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            GameManager.GetComponent<GameManager>().hitpoints -= 1;
            Debug.Log("enemy escaped" + GameManager.GetComponent<GameManager>().hitpoints);
            Destroy(gameObject);
            return;
        }
        if (finalPath.Count > 0)
        {
            targetPosition = tilemap.GetCellCenterWorld(finalPath[0]);
            finalPath.RemoveAt(0);
        }
    }

}
