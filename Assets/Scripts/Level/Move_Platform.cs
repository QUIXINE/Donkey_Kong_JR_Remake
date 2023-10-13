using UnityEngine;

public class Move_Platform : MonoBehaviour {
    [SerializeField] private Transform wayPoint01, wayPoint02, targetPoint;
    [SerializeField] private float moveSpeed;
    private void Start() 
    {
        targetPoint = wayPoint02;
    }

    private void Update() 
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);  
        if(transform.position == wayPoint01.position)
        {
            targetPoint = wayPoint02;
        }  
        else if(transform.position == wayPoint02.position)
        {
            targetPoint = wayPoint01;
        }    
    }
}