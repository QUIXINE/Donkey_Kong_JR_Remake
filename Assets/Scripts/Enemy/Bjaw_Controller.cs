using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bjaw_Controller : MonoBehaviour
{
    //Layer
    [SerializeField] private LayerMask GroundLayer;
    //Box
    public BoxCollider2D box2d;
    //Rigidbody
    Rigidbody2D rb;
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


    void Start()
    {
        anim = GetComponent<Animation>();
        rb = GetComponent<Rigidbody2D>();
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
                UpDown();
            }
            
        }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
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

                if (RndNumLF >= 1)
                {
                    transform.position = transform.position + new Vector3(-1 * WalkSpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            }
                if (RndNumLF == 0)
                {
                    transform.position = transform.position + new Vector3(1 * WalkSpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }

    }

    private void UpDown()
    {
        isMove = false;
        transform.position = new Vector3(ObjectRope.transform.position.x, transform.position.y + (-1.5f * (WalkSpeed * Time.deltaTime)), ObjectRope.transform.position.z);

    }

    private void RndLF()
    {
        RndNumLF = Random.Range(0, 2);
    }
    private void RndUD()
    {
        RndNumUD = Random.Range(0, 6);
        if (RndNumUD >= 3)
        {
            isDown = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
            rb.isKinematic = true;
            Debug.Log("Down");
        }
    }

    private bool IsGround()
    {
        float Height = 1f;
        RaycastHit2D rayhit = Physics2D.Raycast(box2d.bounds.center, Vector2.down, box2d.bounds.extents.y + Height, GroundLayer);
        Color rayColor;
        if (rayhit.collider != null)
        {
            rayColor = Color.green;

        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(box2d.bounds.center, Vector2.down * (box2d.bounds.extents.y + Height), rayColor);
        return rayhit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopRope"))
        {
            RndUD();
            Debug.Log("UD" + RndNumUD);
            ObjectRope = collision.gameObject;
            Debug.Log(ObjectRope);
        }

        if (collision.CompareTag("TopRope") && !IsGround())
        {
            isDown = true;
            Debug.Log("!isground");
            rb.isKinematic = true;
        }
        if (collision.CompareTag("Fruit"))
        {

            /* StartCoroutine(WaitAnim());*/
           Destroy(gameObject);

        }
        if (collision.CompareTag("BottRope"))
        {
            rb.isKinematic = false;
        }
    }
}
