using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy_Left_Only : MonoBehaviour
{
    public int maxHealth;
    public int killMoney;
    private int health;
    public Tilemap tilemap;
    public GameObject enemy;
    private Vector3Int endTilePosition = new Vector3Int(10, -3, 0);
    public float moveDelay = 1.0f;
    private HealthBar healthBar;
    private GameObject GameManager;
    private String direction = "right";
    private Vector3Int currentNode = new Vector3Int(-10, 3, 0);
    Vector3 targetPosition;
    public float moveSpeed = 2f;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        Debug.Log("EnemyMovement started");
        targetPosition = tilemap.GetCellCenterWorld(currentNode);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Traverse();
        }
    }


    private void Traverse()
    {
        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            GameManager.GetComponent<GameManager>().hitpoints -= 1;
            Debug.Log("enemy escaped" + GameManager.GetComponent<GameManager>().hitpoints);
            Destroy(gameObject);
        }
        transform.position = tilemap.GetCellCenterWorld(currentNode);
        Vector3Int topNode = new Vector3Int(currentNode.x, currentNode.y + 1, 0);
        Vector3Int rightNode = new Vector3Int(currentNode.x + 1, currentNode.y, 0);
        Vector3Int bottomNode = new Vector3Int(currentNode.x, currentNode.y - 1, 0);
        Vector3Int leftNode = new Vector3Int(currentNode.x - 1, currentNode.y, 0);
        TileBase topTile = tilemap.GetTile(topNode);

        if (direction.Equals("right"))
        {
            if (isWalkable(topNode))
            {
                currentNode = topNode;
                direction = "top";
            }
            else if (isWalkable(rightNode))
            {
                currentNode = rightNode;
                direction = "right";
            }
            else if (isWalkable(bottomNode))
            {
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (isWalkable(leftNode))
            {
                currentNode = leftNode;
                direction = "left";
            }
        }
        else if (direction.Equals("top"))
        {
            if (isWalkable(leftNode))
            {
                currentNode = leftNode;
                direction = "left";
            }
            else if (isWalkable(topNode))
            {
                currentNode = topNode;
                direction = "top";
            }
            else if (isWalkable(rightNode))
            {
                currentNode = rightNode;
                direction = "right";
            }
            else if (isWalkable(bottomNode))
            {
                currentNode = bottomNode;
                direction = "bottom";
            }
        }
        else if (direction.Equals("left"))
        {
            if (isWalkable(bottomNode))
            {
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (isWalkable(leftNode))
            {
                currentNode = leftNode;
                direction = "left";
            }

            else if (isWalkable(topNode))
            {
                currentNode = topNode;
                direction = "top";
            }
            else if (isWalkable(rightNode))
            {
                currentNode = rightNode;
                direction = "right";
            }
        }
        else if (direction.Equals("bottom"))
        {
            if (isWalkable(rightNode))
            {
                currentNode = rightNode;
                direction = "right";
            }
            else if (isWalkable(bottomNode))
            {
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (isWalkable(leftNode))
            {
                currentNode = leftNode;
                direction = "left";
            }

            else if (isWalkable(topNode))
            {
                currentNode = topNode;
                direction = "top";
            }
        }
        targetPosition = tilemap.GetCellCenterWorld(currentNode);
    }


    public void takeDamage(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        health = health - damage;
        Debug.Log("Enemy_Left_Only HealthBar reference: " + healthBar);
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().money += killMoney;
            GameManager.GetComponent<GameManager>().enemiesKilled++;
        }
    }


    private bool isWalkable(Vector3Int node)
    {
        TileBase tile = tilemap.GetTile(node);
        if (tile != null && tile.name != "Wall_Tile")
        {
            return true;
        }
        return false;
    }

}
