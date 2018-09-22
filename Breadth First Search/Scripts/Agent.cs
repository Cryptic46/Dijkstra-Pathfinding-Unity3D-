using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private BreadthFirstSearch pathfinder;
    public Vector2 currentPath;
    public bool setCurrentPath = false;
    private Vector3 travel;

    void Start ()
    {
        pathfinder = GameObject.Find("Player").GetComponent<BreadthFirstSearch>();
    }

	void Update ()
    {
        if (pathfinder.isPathfindingComplete)
        {
            if(!setCurrentPath)
            {
                currentPath = pathfinder.keyCameFromValueMap[transform.position];
                setCurrentPath = true;
            }

            if (Vector2.Distance(currentPath, transform.position) > 0.1f)
            {
                travel = new Vector3((currentPath.x - transform.position.x) / 2.0f, (currentPath.y - transform.position.y) / 2.0f);
            }
            else
            {
                currentPath = pathfinder.keyCameFromValueMap[currentPath];
            }
        }

        if(setCurrentPath)
        {
            transform.position += travel;
        }
	}
}
