using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SBird_Controlleer : MonoBehaviour
{
    //Rigidbody
    Rigidbody2D rb;
    //Timer
    public float Timeremain = 2;
    //Animation
    public Animation anim;
    //variable
    public GameObject SpawnEgg;
    public float WalkSpeed;
    private bool runyet = false;
    public bool isMoveR = false;
    private bool isMoveL = false;
    private bool isDown = false;


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
            
        }

        if (collision.CompareTag("TopRope"))
        {
            isDown = false;
            isMoveL = true;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("BottRope"))
        {
            isDown = false;
            isMoveL = true;
            Instantiate(SpawnEgg, transform.position, Quaternion.identity);
        }

        if (collision.CompareTag("Fruit"))
        {
            Destroy(gameObject);
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
            transform.position = transform.position + new Vector3(1 * WalkSpeed * Time.deltaTime, 0, 0);
        }

        runyet = true;
    }

    private void DOWN()
    {
        transform.position = transform.position + new Vector3(0, -1 * WalkSpeed * Time.deltaTime, 0);
        isMoveR = false;
    }
    private void LEFT()
    {
        transform.position = transform.position + new Vector3(-1 * WalkSpeed * Time.deltaTime, 0, 0);
    }

}

