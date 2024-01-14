
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSparkController3 : MonoBehaviour, IEnemyController
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
    public GameObject[] waypoint4;
    private int currentWaypointIndex = 0;
    private MarioController mc;

    private void Start()
    {
        mc = FindObjectOfType<MarioController>();
    }

    private void Update()
    {
        EnemyPatternController GS = GetComponent<EnemyPatternController>();

        MoveToWaypoint(currentWaypointIndex);

        if (flip == false && this.transform.position.x >= waypoint4[currentWaypointIndex].transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = true;
        }

        if (flip == true && this.transform.position.x <= waypoint4[currentWaypointIndex].transform.position.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = false;
        }

        if (waypoint4.Length > 0)
        {

            if (Vector3.Distance(transform.position, waypoint4[currentWaypointIndex].transform.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoint4.Length;
            }
        }

    }

    private void MoveToWaypoint(int index)
    {



            waypoint4 = EnemyPatternController.Instance.waypoints4;

        transform.position = Vector3.MoveTowards(transform.position, waypoint4[index].transform.position, WalkSpeed * Time.deltaTime);


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