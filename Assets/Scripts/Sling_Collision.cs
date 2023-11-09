using UnityEngine;
using PlayerSpace;

public class Sling_Collision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = gameObject.transform;
        }
        /* if(col.gameObject.CompareTag("Player"))
        {
            print("Collide");
        } */
    }
     private void OnTriggerExit2D(Collider2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = null;
        }
    }
}
