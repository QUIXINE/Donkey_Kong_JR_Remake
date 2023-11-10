﻿using System.Collections;
using UnityEngine;
using TMPro;
using ScoreManagement;


[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Fruit : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isCrashedEnemy01, isCrashedEnemy02, isCrashedEnemy03; //used to check how many enemies is collided
    private int fruitScore = 400;
    private bool canCollidePlayer;
    private bool collidePlayer;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        canCollidePlayer = true;
    }
    public void GetPoint()
    {
        if(PlayerPrefs.GetInt("Current_Player") == 1)
        {
            Score_Variables.ScorePlayer01 = Score_Variables.ScorePlayer01 + fruitScore;
        }
        else
        {
            Score_Variables.ScorePlayer02 = Score_Variables.ScorePlayer02 + fruitScore;
        }
        Score_PopUp_Display.PopUpScore(gameObject.transform, fruitScore);   //Pop up score
        // Score.ScoreText.text = $"{Score.ScorePlayer01}";
        rb.gravityScale = 0.5f;
    }
    private void Update()
    {
        Vector2 pos = new Vector2(-3.87f, -6.69f);
        if (transform.position.y <= pos.y)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && canCollidePlayer)
        {
            AudioPlayerTest.PlayAudio(AudioReferences.EatSound);
            GetPoint();
            collidePlayer = true;
            canCollidePlayer = false;
        }

        if(col.gameObject.layer == 8 && collidePlayer)
        {
            IGetPoint get = col.gameObject.GetComponent<IGetPoint>();
            if(!isCrashedEnemy01)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.FruitBumpSound);
                fruitScore += 400; 
                StartCoroutine(StopTime());
                get.GetPoint(fruitScore);
                Score_PopUp_Display.PopUpScore(col.gameObject.transform, fruitScore);   //Pop up score
                isCrashedEnemy01 = true;
            }
            else if(isCrashedEnemy01 && !isCrashedEnemy02)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.FruitBumpSound);
                fruitScore += 400; 
                StartCoroutine(StopTime());
                get.GetPoint(fruitScore);
                Score_PopUp_Display.PopUpScore(col.gameObject.transform, fruitScore);   //Pop up score
                isCrashedEnemy02 = true;
            }
            else if(isCrashedEnemy01 && isCrashedEnemy02 && !isCrashedEnemy03)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.FruitBumpSound);
                fruitScore += 400; 
                StartCoroutine(StopTime());
                get.GetPoint(fruitScore);
                Score_PopUp_Display.PopUpScore(col.gameObject.transform, fruitScore);   //Pop up score
                isCrashedEnemy03 = true;
            }
        }
        /*if(Collides enemy)
        {
            IGetPoint get = col.gameObj.GetComponent<>;
        if(!isCrashedEnemy01)    
            get.GetPoint();
            isCrashedEnemy01 = true;

        if(isCrashedEnemy01 && !isCrashedEnemy02)    
            get.GetPoint();
            isCrashedEnemy02 = true;
        }

        if(isCrashedEnemy01 && isCrashedEnemy02 && !isCrashedEnemy03)    
            get.GetPoint();
            isCrashedEnemy03 = true;
        }
        */
    
    
    }

    private IEnumerator StopTime()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1.30f);
        Time.timeScale = 1;
    }
}