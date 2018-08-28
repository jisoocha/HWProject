using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraManager : MonoBehaviour {

    public int pixelRes = 64;
    public float offset;
    public float borderOffset = 2;
    public float speed;
    public Tilemap worldMap;

    float screenWidth;
    float screenHeight;
    Vector3 cameraMove;
    Vector2 minMaxXPosition;
    Vector2 minMaxYPosition;
 
    // Use this for initialization
    void Start () {
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / pixelRes / 2;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;
        float camHeight = 2f * camera.orthographicSize;
        float camWidth = camHeight * camera.aspect;
        Debug.Log("CameraSize" + camHeight + " " + camWidth);
        // define the map limits
        worldMap.CompressBounds();
        float cellSize = worldMap.cellSize.x;
        Vector2 cellZeroPosition = worldMap.CellToWorld(new Vector3Int(worldMap.cellBounds.xMin, worldMap.cellBounds.yMin, 0));
        minMaxXPosition = new Vector2(cellZeroPosition.x + camWidth / 2 - borderOffset, cellZeroPosition.x + worldMap.size.x * cellSize - camWidth / 2 + borderOffset);
        minMaxYPosition = new Vector2(cellZeroPosition.y + camHeight / 2 - borderOffset, cellZeroPosition.y + worldMap.size.y * cellSize - camHeight / 2 + borderOffset);
        Debug.Log("Camera min/max position: x: " + minMaxXPosition + " y: " + minMaxYPosition);
    }

    void Update()
    {
        //Move camera
        if ((Input.mousePosition.x > screenWidth - offset) && transform.position.x < minMaxXPosition.y)
        {
            cameraMove.x += MoveSpeed();
        }
        if ((Input.mousePosition.x < offset) && transform.position.x > minMaxXPosition.x)
        {
            cameraMove.x -= MoveSpeed();
        }
        if ((Input.mousePosition.y > screenHeight - offset) && transform.position.y < minMaxYPosition.y)
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
}
