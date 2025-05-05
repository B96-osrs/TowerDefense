using System;
using System.Collections.Generic;
using System.Linq;
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
        Traverse();
    }
    void Update()
    {

        //while the enemy position is not at the targetposition, move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            moveEnemy();
        }

    }

    //Eventhandler calculates path again when a tile is placed
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
        Traverse();
        Debug.Log("Tilemap raised event: " + position);

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
            targetPosition = tilemap.GetCellCenterWorld(finalPath[0].position);
            finalPath.RemoveAt(0);
        }
    }



    private void Traverse()
    {
        Node startNode = new Node(tilemap.WorldToCell(transform.position), null);
        Debug.Log("startnode pos: " + startNode.position);
        finalPath.Clear();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        startNode.gCost = 0;
        startNode.hCost = 0;
        openList.Add(startNode);
        Node currentNode = openList[0];
        int counter = 0;
        while (openList.Count > 0 && counter < 1000) //1000 iterations is just a measure to prevent accidental infinite loop
        {
            counter++;
            //take the Node with least fCost from openList
            currentNode = getNodeWithLeastFValue(openList);

            //remove current node from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            //Debug.Log("Current Node: " + currentNode);

            //end loop when target is reached
            if (currentNode.position == endTilePosition)
            {
                Node tempNode = currentNode;
                while (tempNode != null)
                {
                    finalPath.Add(tempNode);
                    tempNode = tempNode.parent;
                }
                finalPath.Reverse();
                break;
            }

            //get walkable neighbours of current node
            List<Node> children = getWalkableNeighbours(currentNode);
            foreach (Node child in children)
            {
                child.parent = currentNode; //set parent of child to current node
                child.hCost = Mathf.Abs(child.position.x - endTilePosition.x) + Mathf.Abs(child.position.y - endTilePosition.y); //Manhattan distance
                child.gCost = currentNode.gCost + 1; //in our maze the cost of moving to a neighbour is always 1

                if (containsNode(openList, child))
                {
                    Node existingNode = getExistingNode(openList, child.position.x, child.position.y);
                    if (existingNode.gCost < child.gCost)
                    {
                        continue; //skip to next child
                    }
                }

                if (containsNode(closedList, child))
                {
                    Node existingNode = getExistingNode(closedList, child.position.x, child.position.y);
                    if (existingNode.gCost < child.gCost)
                    {
                        continue; //skip to next child
                    }
                }
                openList.Remove(getExistingNode(openList, child.position.x, child.position.y)); //remove existing node from openList list if it exists
                closedList.Remove(getExistingNode(closedList, child.position.x, child.position.y)); //remove existing node from openList list if it exists
                openList.Add(child); //add child to open list

                //add child to open list
                //Debug.Log("Current Node: " + currentNode);
                //Debug.Log("Closed List: " + string.Join(", ", closedList));
            }
        }//while
        Debug.Log("Path found in " + counter + "iterations");
    }


    //checks all adjacent neighbours to determine if they are walkable
    //diagonal movement is not allowed
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

    //reduces enemy hitpoints by the damage taken
    //updates healthbar
    //updates money and enemies killed in GameManager if enemy is dead
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

    //returns true if the parameter node is not a wall tile
    private bool isWalkable(Vector3Int node)
    {
        TileBase tile = tilemap.GetTile(node);
        if (tile != null && tile.name != "Wall_Tile")
        {
            return true;
        }
        return false;
    }

    //takes in a list as parameter, we only use this method on openList
    //returns node with the lowest fCost 
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
        return nodeWithLeastFValue;
    }

    //takes in List, x and y and if there is a node with those coordinates in the list, it returns that node
    //if not, it returns null
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

    //checks if node is already in the list based on the position
    private bool containsNode(List<Node> nodeList, Node node)
    {
        if (nodeList == null || nodeList.Count == 0)
        {
            return false;
        }
        foreach (Node n in nodeList)
        {
            if (n.position.x == node.position.x && n.position.y == node.position.y)
            {
                return true;
            }
        }
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
