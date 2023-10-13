using UnityEngine;

public class Locker_Collision : MonoBehaviour
{
    private SpriteRenderer spR;


    private void Start() 
    {
        spR = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.CompareTag("Key"))
        {
            print("collide");
            Destroy(col.gameObject);
            //increase 200 point per each
            //change sprite
            spR.color = Color.green;
        }
    }
}
