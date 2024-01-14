using UnityEngine;

public class KeyPosition : MonoBehaviour 
{
    //used to limit position of keys in level 4, which won't let it go down further than limit
    BoxCollider2D keyCollider;
    private void Start() 
    {
        keyCollider = GetComponent<BoxCollider2D>();    
    }
    private void Update() {
        if(transform.position.y <= -3.512f)
        {
            Vector2 pos = transform.position;
            pos.y =  -3.512f;
            transform.position = pos;
        }
        
    }
}