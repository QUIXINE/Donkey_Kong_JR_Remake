using UnityEngine;

namespace TakeDamage
{
    public class Enemy_Collision : MonoBehaviour 
    {
        public bool IsAbleToGetPoint {get; private set;} = true;
        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.gameObject.layer == 9)  //player
            {
                Player_TakeDamage takeDamage = col.gameObject.GetComponent<Player_TakeDamage>();
                if(takeDamage != null)
                {
                    takeDamage.animator.SetBool("DieOtherCondition", true);
                    AudioPlayerTest.PlayAudio(AudioReferences.BiteSound);       //play bite sound
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