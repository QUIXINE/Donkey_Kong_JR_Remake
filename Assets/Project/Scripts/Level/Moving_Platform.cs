using UnityEngine;
using PlayerSpace;

//Only used to check x axis
[SelectionBase]
public class Moving_Platform : MonoBehaviour {
    [SerializeField] private Transform wayPoint01, wayPoint02, targetPoint;
    [SerializeField] private float moveSpeed;
    private void Start() 
    {
        targetPoint = wayPoint02;
    }

    private void Update() 
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPoint.position.x, transform.position.y), moveSpeed * Time.deltaTime);  
        if(transform.position.x == wayPoint01.position.x)
        {
            targetPoint = wayPoint02;
        }  
        else if(transform.position.x == wayPoint02.position.x)
        {
            targetPoint = wayPoint01;
        }    
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = gameObject.transform;
        }
    }
     private void OnCollisionExit2D(Collision2D col) 
    {
        if(col.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.transform.parent = null;
        }
    }
}