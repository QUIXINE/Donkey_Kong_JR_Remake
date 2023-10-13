using PlayerSpace;
using TakeDamage;
using UnityEngine;

public class Egg_Collision : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(TryGetComponent<PlayerTakeDamage>(out var player))
        {
            player.TakeDamage();
        }
    }    
}