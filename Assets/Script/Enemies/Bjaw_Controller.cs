using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Bjaw_Controller : MonoBehaviour
{
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
    private bool isBott = false;

    void Start()
    {
        anim = GetComponent<Animation>();
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

                if (RndNumLF >= 1)
                {
                    transform.position = transform.position + new Vector3(-1 * WalkSpeed * Time.deltaTime, 0, 0);
                

                }
                if (RndNumLF == 0)
                {
                    transform.position = transform.position + new Vector3(1 * WalkSpeed * Time.deltaTime, 0, 0);
                
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
            Debug.Log("Down");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopRope"))
        {
            RndLF();
            RndUD();
            Debug.Log("UD" + RndNumUD);
            Debug.Log("LF" + RndNumLF);
            
            ObjectRope = collision.gameObject;
            Debug.Log(ObjectRope);
        }

        if (collision.CompareTag("Fruit"))
        {

            /* StartCoroutine(WaitAnim());*/
           Destroy(gameObject);

        }
    }
}
