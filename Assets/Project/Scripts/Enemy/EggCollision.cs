using PlayerSpace;
using TakeDamage;
using UnityEngine;

public class EggCollision : MonoBehaviour 
{
    [SerializeField] private LayerMask groundLayerMask;
    Animator animator;
    Rigidbody2D rb;
    private void Start() 
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.TryGetComponent<PlayerTakeDamage>(out var player))
        {
            print("Hit");
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Destroy(gameObject, 0.3f);
            animator.SetBool("Hit", true);
            player.animator.SetBool("DieOtherCondition", true);
            player.TakeDamage();
        }
        else if(col.gameObject.layer == 6)  //Ground
        {
            print("Hit");
            rb.velocity = Vector2.zero;
            animator.SetBool("Hit", true);
            Destroy(gameObject, 0.2f);
        }
        else if (col.CompareTag("End"))
        {
            Destroy(gameObject);
        }
    }    
}