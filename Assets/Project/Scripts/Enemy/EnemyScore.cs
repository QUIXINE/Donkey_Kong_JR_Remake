using UnityEngine;

namespace ScoreManagement
{


    public class EnemyScore : MonoBehaviour, IGetPoint 
    {
        private Rigidbody2D rb;
        private Collider2D enemyCollider;


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
                ScoreVariables.ScorePlayer01 = ScoreVariables.ScorePlayer01 + score;
            }
            else
            {
                ScoreVariables.ScorePlayer02 = ScoreVariables.ScorePlayer02 + score;
            }
            rb.isKinematic = false;
            rb.gravityScale = 0.5f;
            enemyCollider.enabled = false;
        }
    }
}