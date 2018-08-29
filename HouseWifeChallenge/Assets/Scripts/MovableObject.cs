using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Sprite))]
public class MovableObject : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Color selectedColor = Color.green;
    private SpriteRenderer sprite;
    private Color initColor;
    private Vector2 initPosition;

    public void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        initColor = sprite.color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        sprite.color = selectedColor;
        initPosition = new Vector2 (transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        sprite.color = initColor;
        transform.position = initPosition;
    }
}
