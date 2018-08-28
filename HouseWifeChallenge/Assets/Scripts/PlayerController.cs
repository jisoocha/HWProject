using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;            
    private Rigidbody2D rb2d;
    List<Vector2> pathToFollow;
    Vector2 moveDirection = Vector2.zero;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        pathToFollow = null;
    }

    private void Update()
    {
        if (pathToFollow != null && pathToFollow.Count > 0)
        {
            // a simplifier !!!
            Vector2 nextPosition = pathToFollow[0];
            Vector2 movement = nextPosition - new Vector2(transform.position.x, transform.position.y);
            float distance = Utils.GetNorm2(movement);
            if (distance < speed * Time.deltaTime)
            {
                pathToFollow.Remove(nextPosition);
                if (pathToFollow.Count == 0)
                {
                    pathToFollow = null;
                    moveDirection = Vector2.zero;
                }
                else
                {
                    nextPosition = pathToFollow[0];
                    movement = nextPosition - new Vector2(transform.position.x, transform.position.y);
                    moveDirection = (nextPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
                }
            }
            else
            {
                moveDirection = (nextPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
            }
        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = moveDirection.normalized * speed;
    }

    public void SetPathToFollow(List<Node> path)
    {
        if (path == null)
        {
            pathToFollow = null;
        }
        else
        {
            pathToFollow = new List<Vector2>();
            foreach (Node node in path)
            {
                pathToFollow.Add(node.worldPos);
            }
        }
    }
}
