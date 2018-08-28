using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindingGrid : MonoBehaviour {

    public Tilemap worldMap; // tileMap defining the word size and grid size
    int width;
    int height;
    Node[,] nodes;
    Node[] finalPath;
    float cellSize;
    List<Node> path;
    Texture2D wallTexture;
    Texture2D groundTexture;
    Texture2D pathTexture;
    // Use this for initialization
    void Start() {
        worldMap.CompressBounds();
        width = worldMap.size.x;
        height = worldMap.size.y;
        cellSize = worldMap.cellSize.x;
        Debug.Log(string.Format("Grid properties: width:{0}, height:{1}, cellsize: {2}", width, height, cellSize));
        path = new List<Node>();
        InitNodes();
        ScanObstacles();
        InitTextures();

        // test
        List<Node> test = GetNeighbours(nodes[2, 2]);
        Debug.Log(string.Join(",", test.Select(x => x.ToString()).ToArray()));

        Vector2 wordPosition = new Vector2(9, 5);
        Node node = GetNodeFromWorldPosition(wordPosition);
        Debug.Log(node.ToString());
    }

    public void InitNodes()
    {
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
                Debug.Log(string.Format("Wall detected at position ({0},{1}) ", node.worldPos.x, node.worldPos.y));
            }
        }
    }

    public bool HasCollider(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, cellSize / 4);
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
        if (nodes != null)
        {
            int xMin = Mathf.Max(node.gridX - 1, 0);
            int xMax = Mathf.Min(node.gridX + 1, width - 1);
            int yMin = Mathf.Max(node.gridY - 1, 0);
            int yMax = Mathf.Min(node.gridY + 1, height - 1);

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
        if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
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
        Vector2Int nodePosition = GetNodePositionFromWorldPosition(position);
        return nodes[nodePosition.x, nodePosition.y];
    }

    public Vector2Int GetNodePositionFromWorldPosition(Vector2 position)
    {
        Vector2 nodeZeroPosition = nodes[0, 0].worldPos;
        float dX = position.x - nodeZeroPosition.x;
        float dY = position.y - nodeZeroPosition.y;
        int x = (int)Mathf.Round(dX / cellSize);
        int y = (int)Mathf.Round(dY / cellSize);
        return new Vector2Int(x, y);
    }

    // graphical utilities (debug)
    public void InitTextures()
    {
        // wall
        Color color = Color.red;
        wallTexture = new Texture2D(1, 1);
        wallTexture.SetPixel(0, 0, color);
        wallTexture.Apply();
        // ground
        color = Color.green;
        groundTexture = new Texture2D(1, 1);
        groundTexture.SetPixel(0, 0, color);
        groundTexture.Apply();
        //path
        color = Color.blue;
        pathTexture = new Texture2D(1, 1);
        pathTexture.SetPixel(0, 0, color);
        pathTexture.Apply();
    }

    public void OnDrawGizmos()
    {
        // Draw obstacles
        if (nodes != null)
        {
            foreach (Node node in nodes)
            {
                //if (node != null && node.worldPos.x == 1.5 && node.worldPos.y == 0.5)
                if (node != null)
                {
                    Texture2D texture = node.isWall ? wallTexture : groundTexture;
                    Gizmos.DrawGUITexture(new Rect(node.worldPos - Vector2.one * cellSize / 2, Vector2.one * cellSize), texture);
                }

            }
        }

        // Draw path
        if (path != null)
        {
            foreach (Node node in path)
            {
                //if (node != null && node.worldPos.x == 1.5 && node.worldPos.y == 0.5)
                if (node != null)
                {
                    Gizmos.DrawGUITexture(new Rect(node.worldPos - Vector2.one * cellSize / 2, Vector2.one * cellSize), pathTexture);
                }

            }
        }

    }

    public Node[,] getNodes()
    {
        return nodes;
    }


    public void SetPath(List<Node> path)
    {
        this.path = path;
    }

    

}
