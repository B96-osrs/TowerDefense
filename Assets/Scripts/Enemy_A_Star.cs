using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;


public class Enemy_A_Star : MonoBehaviour
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
    private Vector3Int endTilePosition = new Vector3Int(10, -3, 0);
    private Vector3Int startTilePosition = new Vector3Int(-9, 3, 0);
    private Node startNode = new Node((new Vector3Int(-9, 3, 0)), null);

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        GameManager = GameObject.Find("GameManager");
        healthBar.SetHealth(health, maxHealth);
        Debug.Log("EnemyMovement started");
        targetPosition = tilemap.GetCellCenterWorld(startTilePosition);
        Traverse();
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        /*
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    Traverse();
                }*/
    }

    private void Traverse()
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        startNode.gCost = -1;
        startNode.hCost = -1;
        openList.Add(startNode);
        Node currentNode = openList[0];
        int counter = 0;
        while (openList.Count > 0 && currentNode.position != endTilePosition && counter < 5000)
        {
            currentNode = getNodeWithLeastFValue(openList);
            openList.Remove(currentNode);

            if (currentNode.position == endTilePosition)
            {
                return;
            }
            List<Node> neighbours = getWalkableNeighbours(currentNode);
            foreach (Node neighbour in neighbours)
            {
                if (!containsNode(closedList, neighbour))
                {
                    neighbour.gCost = currentNode.gCost + 1; //in our maze the distance between parent and child node is always 1
                    neighbour.hCost = Mathf.Abs(neighbour.position.x - endTilePosition.x) + Mathf.Abs(neighbour.position.y - endTilePosition.y);


                    if (!containsNode(openList, neighbour))
                    {
                        openList.Add(neighbour);
                    }
                    else
                    {
                        Node existingNode = getExistingNode(openList, neighbour.position.x, neighbour.position.y);

                        if (neighbour.fCost < existingNode.fCost)
                        {
                            Debug.Log("neighbor gCost: " + neighbour.gCost + " existingNode gCost: " + existingNode.gCost);
                            existingNode.gCost = neighbour.gCost;
                            existingNode.parent = currentNode;
                        }
                    }
                }
                else if (!(getExistingNode(closedList, neighbour.position.x, neighbour.position.y).fCost < neighbour.fCost))
                {
                    openList.Add(getExistingNode(openList, neighbour.position.x, neighbour.position.y));
                }
            }
            Debug.Log("Current Node: " + currentNode);
            Debug.Log("Closed List: " + string.Join(", ", closedList));
            //Debug.Log("Open List: " + string.Join(", ", openList));
            closedList.Add(currentNode);
            counter++;
        }//while
    }


    private List<Node> getWalkableNeighbours(Node currentNode)
    {
        Vector3Int topNode = new Vector3Int(currentNode.position.x, currentNode.position.y + 1, 0);
        Vector3Int rightNode = new Vector3Int(currentNode.position.x + 1, currentNode.position.y, 0);
        Vector3Int bottomNode = new Vector3Int(currentNode.position.x, currentNode.position.y - 1, 0);
        Vector3Int leftNode = new Vector3Int(currentNode.position.x - 1, currentNode.position.y, 0);

        List<Node> walkableNeighbours = new List<Node>();

        if (isWalkable(topNode)) walkableNeighbours.Add(new Node(topNode, currentNode));
        if (isWalkable(rightNode)) walkableNeighbours.Add(new Node(rightNode, currentNode));
        if (isWalkable(bottomNode)) walkableNeighbours.Add(new Node(bottomNode, currentNode));
        if (isWalkable(leftNode)) walkableNeighbours.Add(new Node(leftNode, currentNode));

        return walkableNeighbours;
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


    private bool isWalkable(Vector3Int node)
    {
        TileBase tile = tilemap.GetTile(node);
        if (tile != null && tile.name == "White_Tile_0")
        {
            return true;
        }
        return false;
    }


    private Node getNodeWithLeastFValue(List<Node> openList)
    {
        Node nodeWithLeastFValue = openList[0];
        foreach (Node node in openList)
        {
            if (node.fCost < nodeWithLeastFValue.fCost || nodeWithLeastFValue.fCost < 0)
            {
                nodeWithLeastFValue = node;
            }
        }
        Debug.Log("Node with least fCost: " + nodeWithLeastFValue);
        return nodeWithLeastFValue;
    }

    private Node getExistingNode(List<Node> openList, int xValue, int yValue)
    {
        Node existingNode;
        foreach (Node node in openList)
        {
            if (node.position.x == xValue && node.position.y == yValue)
            {
                return existingNode = node;
            }
        }
        return null;
    }

    private bool containsNode(List<Node> nodeList, Node node)
    {
        Debug.Log("Checking if node exists: " + node.position);
        Debug.Log("Node List: " + string.Join(", ", nodeList));
        if (nodeList == null || nodeList.Count == 0)
        {
            Debug.LogError("openListempty!");
            return false;
        }
        foreach (Node n in nodeList)
        {
            if (n.position.x == node.position.x && n.position.y == node.position.y)
            {
                Debug.Log("Node already exists: " + n.position);
                return true;
            }
        }
        Debug.Log("Node does not exist: " + node.position);
        return false;
    }

}

public class Node
{
    public Vector3Int position;
    public Node parent;
    public int gCost; //g -> distance between current node and start node
    public int hCost; //heuristic: distance from current node to end node
    public int fCost { get { return gCost + hCost; } } //total cost of node: g + h = f
    public Node(Vector3Int position, Node parent)
    {
        this.position = position;
        this.parent = parent;
    }
    override
    public String ToString()
    {
        if (this.parent == null)
        {
            return ("Node Position: " + this.position + ", Parent: null, gCost: " + this.gCost + ", hCost: " + this.hCost);
        }
        else
        {
            return ("Node Position: " + this.position + ", Parent: " + this.parent.position + ", gCost: " + this.gCost + ", hCost: " + this.hCost);
        }

    }

}
