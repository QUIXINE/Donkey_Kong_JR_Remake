using UnityEngine;

namespace ScoreManagement
{


    public class Enemy_Score : MonoBehaviour, IGetPoint 
    {
        private Rigidbody2D rb;
        private Collider2D enemyCollider;
        public bool IsAbleToGetPoint {get; private set;} = true;


        private void Start() 
        {
            rb = GetComponent<Rigidbody2D>();
            enemyCollider = GetComponent<Collider2D>();
        }

        //Collide with fruit the GetPoint
        public void GetPoint(int score)
        {
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                Score_Variables.ScorePlayer01 = Score_Variables.ScorePlayer01 + score;
            }
            else
            {
                Score_Variables.ScorePlayer02 = Score_Variables.ScorePlayer02 + score;
            }
            enemyCollider.enabled = false;
            rb.velocity = Vector2.down * 5f;
        }

        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.gameObject.layer == 11)
            {
                IsAbleToGetPoint = false;
            }
        }

        private void OnTriggerExit2D(Collider2D col) 
        {
            if (col.gameObject.layer == 11)
            {
                IsAbleToGetPoint = true;
            }
        }


    }
}