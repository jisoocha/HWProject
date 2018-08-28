using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFindingManager : MonoBehaviour {

    public PathFindingGrid grid;
    public static int limitStepAlgo = 1000;
    GameObject player;

	void Start () {
        player = GameObject.Find("Player");
    }

    public List<Node> GetPath(Vector2 startPos, Vector2 targetPos)
    {
        List<Node> path = new List<Node>();
        List<Node> openList = new List<Node>();
        List<Node> closeList = new List<Node>();
        
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPos);

        if (startNode == null || targetNode == null)
        {
            return null;
        }

        // First step
        // Update H value of all the nodes in the grid
        grid.ResetNodesParent();
        grid.ScanObstacles();
        foreach (Node node in grid.getNodes())
        {
            if (node != null)
            {
                node.UpdateHCost(targetNode);
            }
        }
        Node currentNode = startNode;
        int cpt = 0;
        while (!currentNode.Equals(targetNode))
        {
            openList.Remove(currentNode);
            if (closeList.Find(n => n.Equals(currentNode)) == null)
            {
                closeList.Add(currentNode);
            }
            // Ajout des noeuds voisins dans l'open list s'ils n'existent pas
            List<Node> neighbours = grid.GetNeighbours(currentNode);
            foreach (Node node in neighbours)
            {   
                if (closeList.Find(n => n.Equals(node)) != null)
                {
                    continue; // we skip nodes in the close list
                }
                // We change the parent node if it is a shorter way to access
                if (node.parent == null || node.GCost > node.ComputeGCostFrom(currentNode))
                {
                    node.parent = currentNode;
                }
                if (openList.Find(n => n.Equals(node)) == null)
                {
                    openList.Add(node);
                } 
            }
            // Choose the node with the lower F cost
            float minCost = float.MaxValue;
            foreach (Node node in openList)
            {
                if (node.Equals(targetNode))
                {
                    currentNode = node;
                    break;
                }
                else if (node.FCost < minCost)
                {
                    currentNode = node;
                    minCost = node.FCost;
                }
            }
            // Remove the current Node from openList
            cpt++;
            if (cpt > limitStepAlgo)
            {
                Debug.Log("Reached iteration limit");
                return null;
            }
        }
        // Make the path
        Debug.Log("number of iterations: " + cpt);
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Debug.Log("Pressed primary button.");
            Vector3 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPos = new Vector2(mousePositon.x, mousePositon.y);
            Vector2 startPos = new Vector2(player.transform.position.x, player.transform.position.y);
            List<Node> path = GetPath(startPos, targetPos);
            Debug.Log("New Path: " + string.Join(",", path.Select(x => x.ToString()).ToArray()));
            grid.SetPath(path);
            player.GetComponent<PlayerController>().SetPathToFollow(path);
        }
    }




}
