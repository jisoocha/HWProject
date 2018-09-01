using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Sprite))]
public class MovableObject : MonoBehaviour {

    private Tilemap worldMap;
    public Color moveColorOk = new Color(0, 1, 0, 0.5f);
    public Color moveColorNOk = new Color(1, 0, 0, 0.5f);
    private SpriteRenderer sprite;
    private Color initColor;
    private Vector2 initPosition;
    bool isMoving;

    public void Start()
    {
        // init attributes
        worldMap = GlobalManager.GetGroundTileMap();
        worldMap.CompressBounds();
        sprite = GetComponent<SpriteRenderer>();
        initColor = sprite.color;
    }

    public void Update()
    {
        if (isMoving)
        {
            OnMove();
        }
    }

    public void OnBeginMove()
    {
        isMoving = true;
        initPosition = new Vector2(transform.position.x, transform.position.y);
        DisableColliders();
    }

    public void OnMove()
    {
        Vector3Int cellPosition = getCellPositionFromMouseInput();
        sprite.color = IsFreeCell(cellPosition) ? moveColorOk : moveColorNOk;
        Vector3 cellWorldPosition = worldMap.GetCellCenterWorld(cellPosition);
        transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, transform.position.z);
        //Debug.Log("OnMove");
    }

    public void OnEndMove()
    {
        Vector3Int cellPosition = getCellPositionFromMouseInput();
        if (IsFreeCell(cellPosition))
        {
            EnableColliders();
            isMoving = false;
            sprite.color = initColor;
        }
    }

    public Vector3Int getCellPositionFromMouseInput()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return worldMap.WorldToCell(mousePosition);
    }


    public void DisableColliders()
    {
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void EnableColliders()
    {
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public bool IsFreeCell(Vector3Int cellPosition)
    {
        if (!worldMap.HasTile(cellPosition))
        {
            return false;
        }    
        Vector3 cellWorldPosition = worldMap.GetCellCenterWorld(cellPosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(cellWorldPosition, worldMap.cellSize.x / 4);
        if (colliders.Length == 1 && colliders[0].gameObject == gameObject)
        {
            return true;
        }
        return colliders.Length == 0;
    }

    public void OnMouseDown()
    {
        if (isMoving)
        {
            OnEndMove();
        }
        else
        {
            OnBeginMove();
        }
    }

}
