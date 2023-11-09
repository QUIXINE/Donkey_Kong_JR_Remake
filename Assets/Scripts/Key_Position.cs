using UnityEngine;

public class Key_Position : MonoBehaviour 
{
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