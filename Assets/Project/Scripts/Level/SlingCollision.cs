using UnityEngine;
using PlayerSpace;

public class SlingCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = gameObject.transform;
        }
    }
     private void OnTriggerExit2D(Collider2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = null;
        }
    }
}
