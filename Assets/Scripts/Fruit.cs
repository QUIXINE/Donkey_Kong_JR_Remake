using System.Collections;
using UnityEngine;
using TMPro;
using ScoreManagement;


[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Fruit : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isCrashedEnemy01, isCrashedEnemy02, isCrashedEnemy03; //used to check how many enemies is collided
    private int fruitScore;
    private bool canCollidePlayer;
    private void Start()
    {
        fruitScore = 400;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        canCollidePlayer = true;
    }
    public void GetPoint()
    {
        Score.ScorePlayer01 = Score.ScorePlayer01 + fruitScore;
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
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && canCollidePlayer)
        {
            GetPoint();
            canCollidePlayer = false;
        }

        if(col.gameObject.layer == 8)
        {
            IGetPoint get = col.gameObject.GetComponent<IGetPoint>();
            if(!isCrashedEnemy01)
            {
                fruitScore += 400; 
                get.GetPoint(fruitScore);
                isCrashedEnemy01 = true;
            }
            else if(isCrashedEnemy01 && !isCrashedEnemy02)
            {
                fruitScore += 400; 
                get.GetPoint(fruitScore);
                isCrashedEnemy02 = true;
            }
            else if(isCrashedEnemy01 && isCrashedEnemy02 && !isCrashedEnemy03)
            {
                fruitScore += 400; 
                get.GetPoint(fruitScore);
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

}
