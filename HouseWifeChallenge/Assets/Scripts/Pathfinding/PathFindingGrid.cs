using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Utils;

// A intégrer dans le game manager ?
public class PathFindingGrid : MonoBehaviour {

    public Tilemap worldMap; // tileMap defining the word size and grid size
    public int Width => worldMap.size.x;
    public int Height => worldMap.size.y;
    public float CellSize => worldMap.cellSize.x;
	Node[,] nodes;
	
    // Use this for initialization
    void Start() {
        Init(worldMap);
    }

	public void Init(Tilemap worldMap)
	{
        this.worldMap = worldMap;
        this.worldMap.CompressBounds();
        Debug.Log(string.Format("Grid Initialized. Properties: width:{0}, height:{1}, cellsize: {2}", Width, Height, CellSize));
        InitNodes();
	}
	
    public void InitNodes()
    {
        nodes = new Node[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int gridX = worldMap.cellBounds.xMin + x;
                int gridY = worldMap.cellBounds.yMin + y;
                Vector3Int localPlace = new Vector3Int(gridX, gridY, (int)worldMap.transform.position.z);
                if (worldMap.HasTile(localPlace))
                {
                    Vector3 worldPos = worldMap.GetCellCenterWorld(localPlace);
                    nodes[x, y] = new Node(x, y, new Vector2(worldPos.x, worldPos.y));
                }
            }
        }
    }

    public void ResetNodesParent()
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                node.parent = null;
            }
        }
    }

    public void ScanObstacles()
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                node.isWall = HasCollider(node.worldPos);
                //Debug.Log(string.Format("Wall detected at position ({0},{1}) ", node.worldPos.x, node.worldPos.y));
            }
        }
    }

    public bool HasCollider(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, CellSize / 4);
        if (colliders.Length == 0)
        {
            return false;
        }
        else
        {
            return !CollidersIsPlayer(colliders);
        }
    }

    public bool CollidersIsPlayer(Collider2D[] colliders)
    {
        return colliders.Length >= 1 && colliders[0].gameObject.name.Equals("Player");
    }

    // Return the neighbours node of one defined node
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        if (node != null)
        {
            int xMin = Mathf.Max(node.gridX - 1, 0);
            int xMax = Mathf.Min(node.gridX + 1, Width - 1);
            int yMin = Mathf.Max(node.gridY - 1, 0);
            int yMax = Mathf.Min(node.gridY + 1, Height - 1);

            bool isTopWall = !CanWalkPosition(node.gridX, node.gridY + 1);
            bool isBottomWall = !CanWalkPosition(node.gridX, node.gridY - 1);
            bool isLeftWall = !CanWalkPosition(node.gridX - 1, node.gridY);
            bool isRightWall = !CanWalkPosition(node.gridX + 1, node.gridY);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    if ((x == node.gridX && y == node.gridY)
                        || !CanWalkPosition(x,y))
                    {
                        // ne rien faire
                    }
                    else
                    {
                        if ((isTopWall && y == node.gridY + 1)
                            || (isBottomWall && y == node.gridY - 1)
                            || (isLeftWall && x == node.gridX - 1)
                            || (isRightWall && x == node.gridX + 1))
                        {
                            // ne rien faire
                        }
                        else
                        {
                            neighbours.Add(nodes[x, y]);
                        }                
                    }                    
                }
            }
        }
        return neighbours;
    }


    public bool CanWalkPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1)
        {
            return false;
        }
        else
        {
            return nodes[x, y] != null && !nodes[x, y].isWall;
        }
    }

    public Node GetNodeFromWorldPosition(Vector2 position)
    {
        Vector2Int localPosition = GetNodePositionFromWorldPosition(position);
		if (localPosition.x < 0 || localPosition.x > Width - 1 
				|| localPosition.y < 0 || localPosition.y > Height - 1)
		{
            return null;
		}
		else
		{
			return nodes[localPosition.x, localPosition.y];
		}
    }

    public Vector2Int GetNodePositionFromWorldPosition(Vector2 position)
    {
        Vector2 nodeZeroPosition = nodes[0, 0].worldPos;
        float dX = position.x - nodeZeroPosition.x;
        float dY = position.y - nodeZeroPosition.y;
        int x = (int) Mathf.Round(dX / CellSize);
        int y = (int) Mathf.Round(dY / CellSize);
        return new Vector2Int(x, y);
    }

    public Node[,] getNodes()
    {
        return nodes;
    }

    public Node GetClosestFreeNodeFromPos(Vector2 worldPos)
    {
        Node closestNode = null;
        // à développer
        return closestNode;
    }
}
