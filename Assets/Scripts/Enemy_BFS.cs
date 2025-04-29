using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int killMoney;
    private int health;
    public Tilemap tilemap;
    public GameObject enemy;
    private Vector3Int startingTilePosition = new Vector3Int(-10, 3, 0);
    private Vector3Int endTilePosition = new Vector3Int(10, -3, 0);
    public float moveDelay = 1.0f;
    private HealthBar healthBar;
    private GameObject GameManager;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        Debug.Log("EnemyMovement started");
        StartCoroutine(FindPathBFS());
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


    IEnumerator FindPathBFS()
    {
        int loopCounter = 0;
        Vector3Int currentNode = startingTilePosition;
        Vector3Int endNode = endTilePosition;
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>(); //to keep track of visited nodes
        Queue<Vector3Int> q = new Queue<Vector3Int>(); //keep track of nodes to visit
        Dictionary<Vector3Int, Vector3Int> parentMap = new Dictionary<Vector3Int, Vector3Int>(); //to reconstruct the path
        q.Enqueue(currentNode);
        visited.Add(currentNode);

        while (q.Count > 0)
        {
            currentNode = q.Dequeue();

            //if we reach our end tile
            if (currentNode == endNode)
            {
                Debug.Log("Path found");
                List<Vector3Int> path = (ReconstructPath(parentMap));
                StartCoroutine(MoveEnemy(path));
                yield break;
            }

            
            Vector3Int topNode = new Vector3Int(currentNode.x, currentNode.y + 1, 0);
            Vector3Int rightNode = new Vector3Int(currentNode.x + 1, currentNode.y, 0);
            Vector3Int bottomNode = new Vector3Int(currentNode.x, currentNode.y - 1, 0);
            Vector3Int leftNode = new Vector3Int(currentNode.x - 1, currentNode.y, 0);


            //check if the top tile is walkable
            TileBase topTile = tilemap.GetTile(topNode);
            if (topTile != null && topTile.name == "White_Tile_0" && !visited.Contains(topNode))
            {
                q.Enqueue(topNode);
                visited.Add(topNode);
                parentMap[topNode] = currentNode;
            }

            //check if the right tile is walkable
            TileBase rightTile = tilemap.GetTile(rightNode);
            if (rightTile != null && rightTile.name == "White_Tile_0" && !visited.Contains(rightNode))
            {
                q.Enqueue(rightNode);
                visited.Add(rightNode);
                parentMap[rightNode] = currentNode;
            }

            //check if the bottom tile is walkable
            TileBase bottomTile = tilemap.GetTile(bottomNode);
            if (bottomTile != null && bottomTile.name == "White_Tile_0" && !visited.Contains(bottomNode))
            {
                q.Enqueue(bottomNode);
                visited.Add(bottomNode);
                parentMap[bottomNode] = currentNode;
            }

            //check if the left tile is walkable
            TileBase leftTile = tilemap.GetTile(leftNode);
            if (leftTile != null && leftTile.name == "White_Tile_0" && !visited.Contains(leftNode))
            {
                q.Enqueue(leftNode);
                visited.Add(leftNode);
                parentMap[leftNode] = currentNode;
            }
            loopCounter++;
            Debug.Log("Loop counter: " + loopCounter);
        } //while
    } //FindPathBFS

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> parentMap)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int current = endTilePosition;
        while (current != startingTilePosition)
        {
            path.Add(current);
            current = parentMap[current];
        }

        path.Add(startingTilePosition);
        path.Reverse();
        Debug.Log(path.ToArray());
        return path;
    }

    IEnumerator MoveEnemy(List<Vector3Int> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {

            Vector3 currentPosition = tilemap.CellToWorld(path[i]) + new Vector3(0.5f, 0.5f, 0);
            Vector3 nextPosition = tilemap.CellToWorld(path[i + 1]) + new Vector3(0.5f, 0.5f, 0);
            float journeyLength = Vector3.Distance(currentPosition, nextPosition);
            float moveDuration = journeyLength / 2.8f;
            float timeElapsed = 0f;

            while (timeElapsed < moveDuration)
            {
                transform.position = Vector3.Lerp(currentPosition, nextPosition, timeElapsed / moveDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.0f);
            transform.position = nextPosition;
        }


        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            GameManager.GetComponent<GameManager>().hitpoints -= 1;
            Debug.Log("enemy escaped" + GameManager.GetComponent<GameManager>().hitpoints);
            Destroy(gameObject);

        }

    }

}
