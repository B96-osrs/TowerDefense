using System;
using UnityEngine;

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