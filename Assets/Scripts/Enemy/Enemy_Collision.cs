using UnityEngine;

namespace TakeDamage
{
    public class Enemy_Collision : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.gameObject.layer == 9)  //player
            {
                Player_TakeDamage takeDamage = col.gameObject.GetComponent<Player_TakeDamage>();
                if(takeDamage != null)
                {
                    takeDamage.animator.SetBool("DieOtherCondition", true);
                    takeDamage.TakeDamage();
                }
            }
        }
    }
}