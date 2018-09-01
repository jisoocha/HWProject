using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Utils;

public class PlayerController : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2d;
    Queue<Vector2> path; // --> change to Queue
	Vector2 nextPathPosition;
    Vector2 playerMove = Vector2.zero;
	PathFindingManager pathFindingManager;
	bool isFollowingPath;
	
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        pathFindingManager = GetComponent<PathFindingManager>(); 
    }

	// Called every frame
    private void Update() 
    {
		playerMove = Vector2.zero; // reset each time
		
		// Check Inputs

		if (Input.GetMouseButtonDown(1))
		{ 
			Vector3 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 targetPos = new Vector2(mousePositon.x, mousePositon.y);
			MoveTo(targetPos);
		}
		
		// A simplifier
		if (isFollowingPath)
		{
			FollowPath();
		}
    }

	// Called every "physics" frame
    void FixedUpdate()
    {
        rb2d.velocity = playerMove.normalized * speed;
    }
	
	public void MoveTo(Vector2 newPosition)
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Queue<Node> nodePath = pathFindingManager.GetPathWithAStarAlgo(startPos, newPosition);
        StartFollowingPath (PathFindingManager.ConvertPathToWorldCoord(nodePath));
	}
	
	public void StartFollowingPath(Queue<Vector2> path)
	{
		this.path = path;
		isFollowingPath = true;
		if (path != null && path.Count > 0)
		{
			nextPathPosition = path.Dequeue();
		}
		else 
		{
			StopFollowingPath();
		}
	}
	
	public void StopFollowingPath()
	{
		isFollowingPath = false;
		path = null;
        nextPathPosition = new Vector2(transform.position.x, transform.position.y);
	}
	
	public void FollowPath()
	{
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 movement = nextPathPosition - currentPosition;
		if (GetNorm2(movement) <  2 * speed * Time.deltaTime)
		{
			if (path != null && path.Count != 0)
			{
				nextPathPosition = path.Dequeue();
				movement = nextPathPosition - currentPosition;
			}
			else
			{
				movement = Vector2.zero;
				StopFollowingPath();
			}
		}
		playerMove = movement;
	}
}
