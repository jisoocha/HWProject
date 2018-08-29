using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

[RequireComponent(typeof(PathFindingManager))]
[RequireComponent(typeof(PathFindingGrid))]
public class PathFindingDebug : MonoBehaviour {

    // textures for visual debug
	PathFindingGrid grid;
	PathFindingManager pathFindingManager;

	public Color wallColor = Color.red;
	public Color groundColor = Color.green;
	public Color pathColor = Color.blue;
		
	Texture2D wallTexture;
	Texture2D groundTexture;
	Texture2D pathTexture;
  
	public void Start()
	{
		pathFindingManager = GetComponent<PathFindingManager>();
		grid = GetComponent<PathFindingGrid>();
		InitTextures();
	}

	public void InitTextures()
	{
		wallTexture = GenerateBasicTexture (wallColor);
		groundTexture = GenerateBasicTexture (groundColor);
        pathTexture = GenerateBasicTexture (pathColor);
	}
	
    public void OnDrawGizmos()
    {
        if (grid!= null && grid.getNodes() != null)
        {
            foreach (Node node in grid.getNodes())
            {
                if (node != null)
                {
                    Texture2D texture = node.isWall ? wallTexture : groundTexture;
                    Gizmos.DrawGUITexture(new Rect(node.worldPos - Vector2.one * grid.CellSize / 2, Vector2.one * grid.CellSize), texture);
                }
            }
        }
		
		if (pathFindingManager != null && pathFindingManager.getCurrentPath() != null)
		{
			foreach (Node node in pathFindingManager.getCurrentPath())
            {
                if (node != null)
                {
                    Gizmos.DrawGUITexture(new Rect(node.worldPos - Vector2.one * grid.CellSize / 2, Vector2.one * grid.CellSize), pathTexture);
                }
            }
		}
    }
}
