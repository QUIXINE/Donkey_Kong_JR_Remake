
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSpark_Controller : MonoBehaviour, IEnemyController
{
    //Rigidbody
    Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animation anim;
    //variable
    public float WalkSpeed;
    private bool flip = false;

    //variableWaypoints
    public GameObject[] waypoint1;
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

        if (flip == false && this.transform.position.x >= waypoint1[currentWaypointIndex].transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = true;
        }

        if (flip == true && this.transform.position.x <= waypoint1[currentWaypointIndex].transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = false;
        }

        if (waypoint1.Length > 0)
        {

            if (Vector3.Distance(transform.position, waypoint1[currentWaypointIndex].transform.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoint1.Length;
            }
        }

    }

    private void MoveToWaypoint(int index)
    {



        waypoint1 = GameMaster.Instance.waypoints1;

        transform.position = Vector3.MoveTowards(transform.position, waypoint1[index].transform.position, WalkSpeed * Time.deltaTime);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            Destroy(gameObject);
        }
    }

    public void EnemyFall()
    {
        this.enabled = false;
    }

}
