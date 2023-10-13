using UnityEngine;

namespace TakeDamage
{
    public class EnemyCollision : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D col) 
        {
            if (col.gameObject.layer == 9)  //player
            {
                ITakeDamage takeDamage = col.gameObject.GetComponent<ITakeDamage>();
                if(takeDamage != null)
                {
                    takeDamage.TakeDamage();
                }
            }
        }
    }
}