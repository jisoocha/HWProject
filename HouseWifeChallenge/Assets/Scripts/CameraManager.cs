using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Mouse moving + pixel perfect camera ----
public class CameraManager : MonoBehaviour {

    public int pixelRes = 64;
	public float speed;
    public float offset;
    public float borderOffset = 2; // in world unit -> outside word portion visible
    public Tilemap worldMap;

    Vector3 cameraMove;
    Vector2 minMaxXPosition;
    Vector2 minMaxYPosition;
	
	Vector2 CameraSize
	{
        get
        {
            Camera camera = GetComponent<Camera>();
            float camHeight = 2f * camera.orthographicSize;
            return new Vector2 (camHeight * camera.aspect, camHeight);
        }
    }
	
    // Use this for initialization
    void Start () {
		cameraMove = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        UpdateCameraSize();
        UpdateCameraLimit();
    }

    void Update()
    {
        //Move camera
        if ((Input.mousePosition.x > Screen.width - offset) && transform.position.x < minMaxXPosition.y)
        {
            cameraMove.x += MoveSpeed();
        }
        if ((Input.mousePosition.x < offset) && transform.position.x > minMaxXPosition.x)
        {
            cameraMove.x -= MoveSpeed();
        }
        if ((Input.mousePosition.y > Screen.height - offset) && transform.position.y < minMaxYPosition.y)
        {
            cameraMove.y += MoveSpeed();
        }
        if ((Input.mousePosition.y < offset) && transform.position.y > minMaxYPosition.x)
        {
            cameraMove.y -= MoveSpeed();
        }
        transform.position = cameraMove;
    }

    float MoveSpeed()
    {
        return speed * Time.deltaTime;
    }
	
	// Pixel perfect size
	public void UpdateCameraSize()
	{
        GetComponent<Camera>().orthographicSize = Screen.height / pixelRes / 2;
    }
	
	private void UpdateCameraLimit()
	{
		worldMap.CompressBounds();
        float cellSize = worldMap.cellSize.x;
        Vector2 cellZeroPosition = worldMap.CellToWorld(new Vector3Int(worldMap.cellBounds.xMin, worldMap.cellBounds.yMin, 0));
        minMaxXPosition = new Vector2(cellZeroPosition.x + CameraSize.x / 2 - borderOffset, 
		                              cellZeroPosition.x + worldMap.size.x * cellSize - CameraSize.x / 2 + borderOffset);
        minMaxYPosition = new Vector2(cellZeroPosition.y + CameraSize.y / 2 - borderOffset, 
		                              cellZeroPosition.y + worldMap.size.y * cellSize - CameraSize.y / 2 + borderOffset);
        Debug.Log("Camera min/max position: x: " + minMaxXPosition + " y: " + minMaxYPosition);
	}
}
