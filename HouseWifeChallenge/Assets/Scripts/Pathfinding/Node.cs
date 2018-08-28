using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX;
    public int gridY;
    public Node parent;
    public bool isWall;
    public Vector2 worldPos;

    // Cost variables
    private int HCost; // distance from the target Node (manhattan distance)
    public int FCost => GCost + HCost; // sum
    public int GCost => ComputeGCostFrom(parent);
    
    // Constructeur
    public Node(int gridX, int gridY, Vector2 worldPos)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.worldPos = worldPos;
    }
	
    // A utliser à chaque changement de target
    public void UpdateHCost(Node targetNode)
    {
        HCost = 10 * ComputeManhattanDistance (this, targetNode);
    }

    static private int ComputeManhattanDistance(Node startNode, Node targetNode)
    {
        int d = Mathf.Abs(targetNode.gridX - startNode.gridX) + Mathf.Abs(targetNode.gridY - startNode.gridY);
        return d;
    }

    override
    public string ToString()
    {
        return string.Format("({0},{1})", gridX, gridY);
    }

    public int ComputeGCostFrom(Node parent)
    {
        if (parent == null) return 0;
        int addCost = 10;
        if (parent.gridX != this.gridX && parent.gridY != this.gridY)
        {
            addCost = 14;
        }
        return parent.GCost + addCost;
    }
}
