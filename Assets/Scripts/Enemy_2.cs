using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_2 : MonoBehaviour
{
    public int maxHealth;
    public int killMoney;
    private int health;
    public Tilemap tilemap;
    public GameObject enemy;
    private Vector3Int endTilePosition = new Vector3Int(10, -3, 0);
    public float moveDelay = 1.0f;
    public HealthBar healthBar;
    private GameObject GameManager;
    private String direction = "right";
    private Vector3Int currentNode = new Vector3Int(-10, 3, 0);
    private Boolean reachedEnd = false;

    void Start()
    {
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        Debug.Log("EnemyMovement started");
        InvokeRepeating("Traverse", 0.0f, moveDelay);
    }


    private void Traverse()
    {
        if (tilemap.WorldToCell(transform.position) == endTilePosition)
        {
            reachedEnd = true;
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
            if ((tilemap.GetTile(topNode) != null && tilemap.GetTile(topNode).name == "White_Tile_0"))
            {
                transform.position = tilemap.GetCellCenterWorld(topNode);
                currentNode = topNode;
                direction = "top";
            }
            else if ((tilemap.GetTile(rightNode) != null && tilemap.GetTile(rightNode).name == "White_Tile_0"))
            {
                transform.position = tilemap.GetCellCenterWorld(rightNode);
                currentNode = rightNode;
                direction = "right";
            }
            else if (tilemap.GetTile(bottomNode) != null && tilemap.GetTile(bottomNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(bottomNode);
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (tilemap.GetTile(leftNode) != null && tilemap.GetTile(leftNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(leftNode);
                currentNode = leftNode;
                direction = "left";
            }
            Debug.Log("Current Node: " + currentNode);
            Debug.Log("Direction: " + direction);
        }

        else if (direction.Equals("top"))
        {
            if (tilemap.GetTile(leftNode) != null && tilemap.GetTile(leftNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(leftNode);
                currentNode = leftNode;
                direction = "left";
            }
            else if ((tilemap.GetTile(topNode) != null && tilemap.GetTile(topNode).name == "White_Tile_0"))
            {
                transform.position = tilemap.GetCellCenterWorld(topNode);
                currentNode = topNode;
                direction = "top";
            }
            else if (tilemap.GetTile(rightNode) != null && tilemap.GetTile(rightNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(rightNode);
                currentNode = rightNode;
                direction = "right";
            }
            else if (tilemap.GetTile(bottomNode) != null && tilemap.GetTile(bottomNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(bottomNode);
                currentNode = bottomNode;
                direction = "bottom";
            }
        }
        else if (direction.Equals("left"))
        {
            if (tilemap.GetTile(bottomNode) != null && tilemap.GetTile(bottomNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(bottomNode);
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (tilemap.GetTile(leftNode) != null && tilemap.GetTile(leftNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(leftNode);
                currentNode = leftNode;
                direction = "left";
            }

            else if (tilemap.GetTile(topNode) != null && tilemap.GetTile(topNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(topNode);
                currentNode = topNode;
                direction = "top";
            }
            else if (tilemap.GetTile(rightNode) != null && tilemap.GetTile(rightNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(rightNode);
                currentNode = rightNode;
                direction = "right";
            }
        }

        else if (direction.Equals("bottom"))
        {
            if (tilemap.GetTile(rightNode) != null && tilemap.GetTile(rightNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(rightNode);
                currentNode = rightNode;
                direction = "right";
            }
            else if (tilemap.GetTile(bottomNode) != null && tilemap.GetTile(bottomNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(bottomNode);
                currentNode = bottomNode;
                direction = "bottom";
            }
            else if (tilemap.GetTile(leftNode) != null && tilemap.GetTile(leftNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(leftNode);
                currentNode = leftNode;
                direction = "left";
            }

            else if (tilemap.GetTile(topNode) != null && tilemap.GetTile(topNode).name == "White_Tile_0")
            {
                transform.position = tilemap.GetCellCenterWorld(topNode);
                currentNode = topNode;
                direction = "top";
            }
        }
    }


    public void takeDamage(int damage)
    {
        health = health - damage;
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.GetComponent<GameManager>().money += killMoney;
            GameManager.GetComponent<GameManager>().enemiesKilled++;
        }
    }
}
