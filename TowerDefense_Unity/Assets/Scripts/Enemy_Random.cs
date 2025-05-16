using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy_Random : MonoBehaviour
{
    // Public configuration variables
    public int maxHealth;
    public int killMoney;
    public float moveDelay = 1.0f;
    public float moveSpeed = 2f;
    public Tilemap tilemap;
    public GameObject enemy;

    // Private state and pathfinding variables
    private int health;
    private Vector3 targetPosition;
    private Vector3Int startTilePosition = new Vector3Int(-9, 3, 0);
    private Vector3Int endTilePosition = new Vector3Int(9, -3, 0);
    private Vector3Int currentNode = new Vector3Int(-9, 3, 0);
    private Vector3Int previousNode = new Vector3Int(-11, 3, 0);
    private bool startLineCrossed = false;

    // Private reference variables
    private HealthBar healthBar;
    private GameObject GameManager;


    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        Debug.Log("EnemyMovement started");
        targetPosition = tilemap.GetCellCenterWorld(currentNode);
        InvokeRepeating("takeDamageOverTime", 30f, 3f);
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
        Debug.Log("Current coordinates: " + currentNode);
        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            GameManager.GetComponent<GameManager>().hitpoints -= 1;
            Debug.Log("enemy escaped" + GameManager.GetComponent<GameManager>().hitpoints);
            Destroy(gameObject);
            return;
        }

        //transform.position = tilemap.GetCellCenterWorld(currentNode);
        List<Vector3Int> possibleMoves = new List<Vector3Int>();

        Vector3Int topNode = new Vector3Int(currentNode.x, currentNode.y + 1, 0);
        Vector3Int rightNode = new Vector3Int(currentNode.x + 1, currentNode.y, 0);
        Vector3Int bottomNode = new Vector3Int(currentNode.x, currentNode.y - 1, 0);
        Vector3Int leftNode = new Vector3Int(currentNode.x - 1, currentNode.y, 0);

        if (isWalkable(topNode) && topNode != previousNode) possibleMoves.Add(topNode);
        if (isWalkable(rightNode) && rightNode != previousNode) possibleMoves.Add(rightNode);
        if (isWalkable(bottomNode) && bottomNode != previousNode) possibleMoves.Add(bottomNode);
        if (isWalkable(leftNode) && leftNode != previousNode) possibleMoves.Add(leftNode);

        if (possibleMoves.Count > 0)
        {
            System.Random random = new System.Random();
            Vector3Int nextNode = possibleMoves[random.Next(possibleMoves.Count)];
            previousNode = currentNode;
            currentNode = nextNode;

            if (startLineCrossed != true && currentNode == startTilePosition)
            {
            }
            else
            {
                targetPosition = tilemap.GetCellCenterWorld(currentNode);
            }

            if (nextNode == startTilePosition)
            {
                startLineCrossed = true;
            }
        }
        else
        {
            targetPosition = tilemap.GetCellCenterWorld(previousNode);
            Vector3Int tempNode = previousNode;
            previousNode = currentNode;
            currentNode = tempNode;
        }
    }




    public void takeDamage(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        health = health - damage;
        Debug.Log("Enemy_Right_Only HealthBar reference: " + healthBar);
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().money += killMoney;
            GameManager.GetComponent<GameManager>().enemiesKilled++;
        }
    }


    public void takeDamageOverTime()
    {
        takeDamage(15);
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
