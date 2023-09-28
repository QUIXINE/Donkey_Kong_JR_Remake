using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBird_Controller2 : MonoBehaviour
{
    //Rigidbody
    Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animation anim;
    //variable
    public float WalkSpeed;

    //variableWaypoints
    public GameObject[] waypoint2;
    private int currentWaypointIndex = 0;
    private Mario_Controller mc;

    private void Start()
    {
        mc = FindObjectOfType<Mario_Controller>();
    }

    private void Update()
    {
        GameMaster GS = GetComponent<GameMaster>();

        MoveToWaypoint(currentWaypointIndex);



        if (waypoint2.Length > 0)
        {

            if (Vector3.Distance(transform.position, waypoint2[currentWaypointIndex].transform.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoint2.Length;
            }
        }
    }

    private void MoveToWaypoint(int index)
    {

        waypoint2 = GameMaster.Instance.waypoints2;


        transform.position = Vector3.MoveTowards(transform.position, waypoint2[index].transform.position, WalkSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Fruit"))
        {

            /* StartCoroutine(WaitAnim());*/
            Destroy(gameObject);

        }
    }
}
