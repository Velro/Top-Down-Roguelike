using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Pathfinding))]
public class ChasingEnemyAI : Enemy
{
    Transform target;
    private Vector3 destination;
    private Animator animator;
    private Pathfinding pathfindingAgent;

    void Start()
    {
        // Cache agent component and destination
        pathfindingAgent = GetComponent<Pathfinding>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        pathfindingAgent.FindPath(transform.position, target.position);
    }

    void Update()
    {
        //if (Vector3.Distance(pathfindingAgent.Path[pathfindingAgent.Path.Count -1], target.position) > 1)//only find a new path if the target deviates from the current path's endpoint by 1 meter
        {
            pathfindingAgent.FindPath(transform.position, target.position);
        }
        //If path count is bigger than zero then call a move method
        if (pathfindingAgent.Path.Count > 0)
        {
            Move();
        }

        if (health < 0)
        {
            Die();
        }
    }

    //A test move function, can easily be replaced – call it to test the map!
    public void Move()
    {
        if (pathfindingAgent.Path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathfindingAgent.Path[0], Time.deltaTime);
            if (Vector3.Distance(transform.position, pathfindingAgent.Path[0]) < 0.1F)
            {
                pathfindingAgent.Path.RemoveAt(0);
            }
        }
    }
}
