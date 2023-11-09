using PlayerSpace;
using TakeDamage;
using UnityEngine;

public class Egg_Collision : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(TryGetComponent<Player_TakeDamage>(out var player))
        {
            player.animator.SetBool("DieOtherCondition", true);
            player.TakeDamage();
        }
    }    
}