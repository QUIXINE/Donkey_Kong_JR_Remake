using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Fruit : MonoBehaviour, IGetPoint
    {
        private Rigidbody2D rb;
        private bool isCrashedEnemy01, isCrashedEnemy02, isCrashedEnemy03; //used to check how many enemies is collided
        private int fruitScore;
        private void Start()
        {
            fruitScore = 400;
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
        public void GetPoint()
        {
            Score.TotalScore = Score.TotalScore + fruitScore;
            rb.gravityScale = 1;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
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
}