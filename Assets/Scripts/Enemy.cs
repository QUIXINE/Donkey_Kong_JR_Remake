using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Enemy : MonoBehaviour , IGetPoint
{
    public Transform posUnderPlayer;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float desireDuration;
    private Transform firstPos;
    private Transform lastPos;
    private bool changePos = false;
    [SerializeField] private float speed;


    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private void Start() {
        firstPos = transform;
        firstPos.position = transform.position;

        if(posUnderPlayer != null)
        {
            lastPos = posUnderPlayer;
            lastPos.position = posUnderPlayer.position;
        }

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }
     private void Update() 
     {
        elapsedTime += Time.deltaTime;
        float percentageTime = elapsedTime/desireDuration;
        
        if(posUnderPlayer != null)
        transform.position = Vector2.Lerp(transform.position, lastPos.position, percentageTime);
        
    } 


    public void GetPoint(int score)
    {
        Score.TotalScore = Score.TotalScore + score;
        rb.gravityScale = 0.5f;
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.layer == 9)
        {
            ITakeDamage takeDamage = col.gameObject.GetComponent<ITakeDamage>();
            if(takeDamage != null)
            {
                takeDamage.TakeDamage();
            }
        }
    }
}