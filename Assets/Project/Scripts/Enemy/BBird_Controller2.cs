using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BBird_Controller2 : MonoBehaviour, IEnemyController
{
    //Rigidbody
    Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animator anim;
    public GameObject Sprite;
    //variable
    public float WalkSpeed;
    private bool flip;
    private bool down =false;

    //variableWaypoints
    public GameObject[] waypoint2;
    private int currentWaypointIndex = 0;
    private Mario_Controller mc;

    private void Start()
    {
        mc = FindObjectOfType<Mario_Controller>();
        anim = Sprite.GetComponent<Animator>();
    }

    private void Update()
    {
        GameMaster GS = GetComponent<GameMaster>();
        MoveToWaypoint(currentWaypointIndex);

 

        if (flip == false && this.transform.position.x >= waypoint2[currentWaypointIndex].transform.position.x)
        {
            anim.SetBool("Down", false);
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = true;
        }

        if (flip == true && this.transform.position.x <= waypoint2[currentWaypointIndex].transform.position.x)
        {
            anim.SetBool("Down", false);
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flip = false;
        }
        if (this.transform.position.y >= (waypoint2[currentWaypointIndex].transform.position.y + 0.5))
        {
            anim.SetBool("Down", true);
        }
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
    }

    public void EnemyFall()
    {
        this.enabled = false;
    }
}
