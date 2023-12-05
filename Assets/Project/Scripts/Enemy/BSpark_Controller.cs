using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpark_Controller : MonoBehaviour, IEnemyController
{
    public Transform[] waypoints;
    public GameObject way;
    private int wayhold;
    public float moveSpeed = 5f;
    private int currentWaypointIndex = 0;


    private void Start()
    {
        wayhold = way.transform.childCount;
        waypoints = new Transform[wayhold];
        for (int i = 0; i < wayhold; i++)
        {
            waypoints[i] = way.transform.GetChild(i).transform;
        }
    }
    private void Update()
    {
        MoveToWaypoint(currentWaypointIndex);
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            MoveToWaypoint(currentWaypointIndex);
        }
    }


    private void MoveToWaypoint(int index)
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[index].position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void EnemyFall()
    {
        this.enabled = false;
    }
}
