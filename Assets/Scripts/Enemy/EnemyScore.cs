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
            Score.ScorePlayer01 = Score.ScorePlayer01 + score;
            // Score.ScoreText.text = $"{Score.ScorePlayer01}";
            rb.gravityScale = 0.5f;
            enemyCollider.enabled = false;
        }
    }
}