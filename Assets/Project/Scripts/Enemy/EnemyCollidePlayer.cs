using UnityEngine;

namespace TakeDamage
{
    public class EnemyCollidePlayer : MonoBehaviour 
    {
        public bool IsAbleToGetPoint {get; private set;} = true;
        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.gameObject.layer == 9)  //player
            {
                PlayerTakeDamage takeDamage = col.gameObject.GetComponent<PlayerTakeDamage>();
                if(takeDamage != null)
                {
                    takeDamage.animator.SetBool("DieOtherCondition", true);
                    AudioPlayerTest.PlayAudio(AudioReferences.BiteSound);       //play bite sound
                    takeDamage.IsCollidedWithEnemy = true;
                    takeDamage.TakeDamage();
                }
            }
            else if (col.gameObject.layer == 11)
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