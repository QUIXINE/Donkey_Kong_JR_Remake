using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class  Player : MonoBehaviour
{
    //Movement variables
    private Rigidbody2D rb;
    
    //Move Variables
    private float horizontal;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float firstMoveSpeed;
    [SerializeField] private float onJumpMoveSpeed;

    //Jump variables
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int jumpHeight;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        firstMoveSpeed = moveSpeed;
    }
    
    void Update()
    {
        MoveInput();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Inputs
    private void MoveInput()
    {
        if (IsGrounded()) //Problem: only get input while on the ground, this makes when not pn ground player move on itself
            horizontal = Input.GetAxisRaw("Horizontal");
        /*else
        {
            horizontal = 0;
        }*/
        /*else
        {
            transform.position += new Vector3(0, -9.8f * Time.deltaTime);
        }*/
    }
    #endregion
    private bool IsGrounded()
    {
        moveSpeed = firstMoveSpeed;
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayerMask);
    }
    #region Movement

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
       /* if(horizontal == 1 || horizontal == -1)
        transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
        {
            print("Jump");
            moveSpeed = onJumpMoveSpeed;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //transform.Translate(0, jumpHeight, 0);
        }
        

    }
    #endregion

    private void VineGrab()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Vine")
        {
            rb.gravityScale = 0;
        }
    }
}

//Problem: gravity after jump 