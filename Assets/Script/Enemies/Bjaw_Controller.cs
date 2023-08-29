using System.Collections;
using System.Collections.Generic;
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
    private float TimeLaunch;
    private float WalkSpeed;
    private int RndNum;

    void Start()
    {
    }


    void Update()
    {

    }

    private void Launch()
    {
        if (Timeremain > 0)
        {
            Timeremain -= Time.deltaTime;
            Timeremain = TimeLaunch;
        }
        if (TimeLaunch <= 0)
        {RndNum = Random.Range(0, 2);
        }

        if (RndNum >=1)
        {
           new Vector2(-1, 0);
        }
        else
        {
            new Vector2(1, 0);
        }
    }

    private void WalkleftRigh()
    {
       
    }
}
