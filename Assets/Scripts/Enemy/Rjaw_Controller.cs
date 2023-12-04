using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Rjaw_Controller : MonoBehaviour, IEnemyController
{
    //Layer
    [SerializeField] private LayerMask GroundLayer;
    //Box
    public BoxCollider2D box2d;
    //Rigidbody
    private Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animator anim;
    //variable
    private int dirX ;
    public GameObject Sprite;
    private GameObject ObjectRope;
    private float TimeLaunch;
    public float WalkSpeed;
    private int RndNumLF;
    private int RndNumUD;
    private bool runyet = false;
    private bool isMove = false;
    private bool isDown = false;
    public bool isBott = false;
    private bool isUp = false;
    private bool isUpper = false;
    private bool isflip = false;

    void Start()
    {
        anim = Sprite.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        RndLF();
    }

    private void Update()
    {
        if (!IsGround())
        {
            Back();
        }
    }
    private void FixedUpdate()
    {
        if(runyet == false)
        {
            StartCoroutine(Wait());
            runyet = true;
        }

        if (runyet == true)
        {
            
            if(isMove == true)
            {
                LeftRight();
                flip();
            }
            if(isDown == true)
            {
                Down();
            }
            if (isDown == false)
            {
                Up();
            }
            if (isUp == true)
            {
                Up();
            }

            
        }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        RndLF();
        isMove = true;
    }

    /*IEnumerator WaitAnim(float _deelay = 0)
    {
        yield return new WaitForSeconds(_deelay);
        Destroy(this);
    }*/
    private void LeftRight()
    {

        {
            anim.SetBool("Walk", true);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            if (RndNumLF == 1)
                {
                dirX = 1;
                 rb.velocity = new Vector2 (dirX * WalkSpeed, rb.velocity.y);
                isflip = false;

            }
                if (RndNumLF == 0)
                {
                dirX = -1;
                rb.velocity = new Vector2(dirX * WalkSpeed, rb.velocity.y);
                isflip = true;
            }
        }

    }

    private void Down()
    {
        
        if (isBott == false)
        {
            anim.SetBool("Down", true);
            anim.SetBool("Up", false);
            anim.SetBool("Walk", false);
            isMove = false;
            transform.position = new Vector2(ObjectRope.transform.position.x, transform.position.y + (-1.5f * (WalkSpeed * Time.deltaTime)));
        }

    }

    private void Up()
    {
        if (isBott == true || isUp == true)
        {
            anim.SetBool("Up", true);
            anim.SetBool("Down", false);
            anim.SetBool("Walk", false);
            isMove = false;
            transform.position = new Vector3(ObjectRope.transform.position.x, transform.position.y + (1f * (WalkSpeed * Time.deltaTime)), transform.position.z);
        }
    }

    private void Back() 
    {
       if (RndNumLF == 1)
        {
            RndNumLF = 0;

        }
        else
        {
            RndNumLF = 1;

        }
    }

    void flip()
    {
        if (!isflip)
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if (isflip)
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void RndLF()
    {
        RndNumLF = Random.Range(0, 2);
    }
    private void RndUD()
    {
            RndNumUD = Random.Range(0, 10);
         if (RndNumUD >= 7)
         {
             isDown = true;
             GetComponent<BoxCollider2D>().isTrigger = true;


         }
         /*if (RndNumUD <= 3)
         {
        isUp = true;
             GetComponent<BoxCollider2D>().isTrigger = true;
             Debug.Log("Up");
         }*/
    }

    private bool IsGround()
    {
        float Height = 0.3f;

        RaycastHit2D rayhit = Physics2D.Raycast(box2d.bounds.center, Vector2.down, box2d.bounds.extents.y + Height, GroundLayer);
        Color rayColor;
        if (rayhit.collider != null)
        {
            rayColor = Color.green;
            
        }else
        {
            rayColor= Color.red;
        }

        Debug.DrawRay(box2d.bounds.center, Vector2.down * (box2d.bounds.extents.y + Height), rayColor);
        return rayhit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopRope"))
        {
            if (isBott == false)
            {
                RndLF();
                RndUD();
                isMove = true;
                ObjectRope = collision.gameObject;
            }

            if(isBott == true && isDown == false)
            {
                transform.position = new Vector3(ObjectRope.transform.position.x, ObjectRope.transform.position.y, transform.position.z);
                isBott = false;
                isMove = true;
            }



        }



        if (collision.CompareTag("BottRope") && !IsGround())
        {
            isBott = true;
            isDown = false;
        }
       /* if (collision.CompareTag("Trigger") && IsGround())
        {
            if (isUp == true)
            {
                ObjectRope = collision.gameObject;
                transform.position = new Vector3(ObjectRope.transform.position.x, ObjectRope.transform.position.y, ObjectRope.transform.position.z);
                isUp = false;
                isMove = true;
                isUpper = false;
            }
            if (isUp == false)
            {
                ObjectRope = collision.gameObject;
                isDown = true;
                isMove = false;
            }*/

        }

        public void EnemyFall()
        {
            this.enabled = false;
        }

    }
