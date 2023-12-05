using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SBird_Controlleer : MonoBehaviour, IEnemyController
{
    //Rigidbody
    Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animator anim;
    //variable
    public GameObject SpawnEgg;
    public GameObject Sprite;
    private int dirX;
    private int dirY;
    public float WalkSpeed;
    private bool runyet = false;
    public bool isMoveR = false;
    private bool isMoveL = false;
    private bool isDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = Sprite.GetComponent<Animator>();
    }

    void Update()
    {
        if (runyet == false)
        {
            StartCoroutine(Wait());
        }

        if (isMoveR == true)
        {
            GO();
        }

        if (isDown == true) 
        {
            DOWN();
            
        }

        if (isMoveL == true)
        {
            LEFT();
        }
    }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(Timeremain);
            isMoveR = true;
        }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trigger"))
        {
            isDown = true;
            isMoveR = false;
            anim.SetBool("Down", true);
            anim.SetBool("Walk", false);
        }

        if (collision.CompareTag("TopRope"))
        {

            isDown = false;
            isMoveL = true;
            collision.gameObject.SetActive(false);
            anim.SetBool("Walk", true);
            anim.SetBool("Down", false);
        }

        if (collision.CompareTag("BottRope"))
        {
            isDown = false;
            isMoveL = true;
            Instantiate(SpawnEgg, transform.position, Quaternion.identity);
            anim.SetBool("Walk", true);
            anim.SetBool("Down", false);
        }
        if (collision.CompareTag("End"))
        {
            Destroy(gameObject);
        }
    }

    void GO()
    {
        if(isDown == false)
        {
            dirX = 1;
            rb.velocity = new Vector2(dirX * WalkSpeed, rb.velocity.y);
            Debug.Log("Go");
        }

        runyet = true;
    }

    private void DOWN()
    {
        dirX = 0;
        rb.velocity = new Vector2(dirX * WalkSpeed, rb.velocity.y);
        transform.position = new Vector2(transform.position.x, transform.position.y + (-1.5f * (WalkSpeed * Time.deltaTime)));
        isMoveR = false;
    }
    private void LEFT()
    {
        dirX = -1;
        rb.velocity = new Vector2(dirX * WalkSpeed, rb.velocity.y);
        if(transform.localScale.x >= 0)
        transform.localScale = new Vector3(transform.localScale.x * - 1, transform.localScale.y, transform.localScale.z);
    }

    public void EnemyFall()
    {
        this.enabled = false;
    }
}


