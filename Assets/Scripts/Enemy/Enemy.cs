using UnityEngine;

    
[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]

public class Enemy : MonoBehaviour
{
    public Transform posUnderPlayer;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float desireDuration;
    private Transform firstPos;
    private Transform lastPos;
    [SerializeField] private float speed;


    
    private void Start() {
        firstPos = transform;
        firstPos.position = transform.position;

        if(posUnderPlayer != null)
        {
            lastPos = posUnderPlayer;
            lastPos.position = posUnderPlayer.position;
        }
    }
     private void Update() 
     {
        elapsedTime += Time.deltaTime;
        float percentageTime = elapsedTime/desireDuration;
        
        if(posUnderPlayer != null)
        transform.position = Vector2.Lerp(transform.position, lastPos.position, percentageTime);
        
    } 

}
