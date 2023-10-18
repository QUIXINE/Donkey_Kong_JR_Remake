using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Rjaw_Controller : MonoBehaviour
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
    public Animation anim;
    //variable
    private GameObject ObjectRope;
    private float TimeLaunch;
    public float WalkSpeed;
    private int RndNumLF;
    private int RndNumUD;
    private bool runyet = false;
    private bool isMove = false;
    private bool isDown = false;
    private bool isBott = false;
    private bool isUp = false;
    private bool isUpper = false;

    void Start()
    {
        anim = GetComponent<Animation>();
        rb = GetComponent<Rigidbody2D>();
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
        Debug.Log("Go");
    }

    /*IEnumerator WaitAnim(float _deelay = 0)
    {
        yield return new WaitForSeconds(_deelay);
        Destroy(this);
    }*/
    private void LeftRight()
    {

        {

                if (RndNumLF == 1)
                {
                    transform.position = transform.position + new Vector3(-1 * WalkSpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            }
                if (RndNumLF == 0)
                {
                    transform.position = transform.position + new Vector3(1 * WalkSpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);


            }
        }

    }

    private void Down()
    {   

        if (isBott == false)
        {
            isMove = false;
            transform.position = new Vector3(ObjectRope.transform.position.x, transform.position.y + (-1.5f * (WalkSpeed * Time.deltaTime)), ObjectRope.transform.position.z);
        }

    }

    private void Up()
    {
        if (isBott == true || isUp == true)
        {
            isMove = false;
            transform.position = new Vector3(ObjectRope.transform.position.x, transform.position.y + (0.5f * (WalkSpeed * Time.deltaTime)), ObjectRope.transform.position.z);
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
             Debug.Log("Down");
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
        float Height = 1f;

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
                Debug.Log("UD" + RndNumUD);
                Debug.Log("LF" + RndNumLF);
                isMove = true;
                ObjectRope = collision.gameObject;
                Debug.Log(ObjectRope);
            }

            if(isBott == true && isDown == false)
            {
                transform.position = new Vector3(ObjectRope.transform.position.x, ObjectRope.transform.position.y, ObjectRope.transform.position.z);
                isBott = false;
                isMove = true;
            }



        }

        if (collision.CompareTag("Fruit"))
        {

            /* StartCoroutine(WaitAnim());*/
           Destroy(gameObject);

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

    }
