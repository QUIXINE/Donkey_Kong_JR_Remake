using UnityEngine;

namespace ScoreManagement
{


    public class EnemyScore : MonoBehaviour, IGetPoint 
    {
        private Rigidbody2D rb;
        private BoxCollider2D enemyCollider;


        private void Start() 
        {
            rb = GetComponent<Rigidbody2D>();
            enemyCollider = GetComponent<BoxCollider2D>();
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
            rb.gravityScale = 0.5f;
            enemyCollider.enabled = false;
        }
    }
}