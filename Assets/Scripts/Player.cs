using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class  Player : MonoBehaviour
{
    //Other class access
    private Rigidbody2D rb;
    private Animator animator;

    //Input
    private float horizontal;
    private float vertical;
    
    private bool facingRight;       //Flipping

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float firstMoveSpeed;
    [SerializeField] private float onJumpMoveSpeed;
    

    [Header("Climb")]
    [SerializeField] private float climbUpSpeed;
    [SerializeField] private float climbDownSpeed;

    [Header("Jump and Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int jumpHeight;

    [Header("Vine Check")]
    [SerializeField] private Transform vineCheckPos;
    [SerializeField] private float vineCheckRadius;
    [SerializeField] private LayerMask vineLayerMask;

   /* [Header("Sprite Changing")]
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite onVineSprite;*/
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        firstMoveSpeed = moveSpeed;
    }

    void Update()
    {
        OnGround();
        MoveInput();
        Jump();
        FlipInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Inputs
    private void MoveInput()
    {
        /* if (IsGrounded())*/ //Problem: only get input while on the ground, this makes when not on ground player move on itself because it takes the last input
        if (!OnVine())
            horizontal = Input.GetAxisRaw("Horizontal"); //--> change to DKHorizontal
        if (OnVine())
            vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 || horizontal < 0) //if move, player will get out of the first state
            animator.SetTrigger("MoveOut");

        animator.SetFloat("Move", horizontal); 
        animator.SetFloat("Climb", vertical); 
    }

    private void FlipInput()
    {
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    private bool OnVine()
    {
        return Physics2D.OverlapCircle(vineCheckPos.position, vineCheckRadius, vineLayerMask);
    }
    #endregion
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayerMask);
    }
    #region Movement
    private void OnGround()
    {
        if (IsGrounded())
        {
            animator.SetBool("OnVine", false);
            moveSpeed = firstMoveSpeed;
            animator.SetBool("Jump", false);
            animator.SetFloat("Move", 0);
        }
    }
    private void Move()
    {
        //Climbing
        if (OnVine())
        {
            horizontal = 0;
            animator.SetBool("OnVine", true);
            rb.velocity = new Vector2(rb.velocity.x, (vertical == 1? vertical * climbUpSpeed : vertical * climbDownSpeed) * Time.deltaTime);
        }

        //On grpund move
        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);


        /* if(horizontal == 1 || horizontal == -1)
         transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/  //Try not use rb
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
        {
            animator.SetBool("Jump", true);
            moveSpeed = onJumpMoveSpeed;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //transform.Translate(0, jumpHeight, 0);
        }
    }
    #endregion

  
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Vine")
        {
            //rb.gravityScale = 0;
            /*rb.gravityScale = 0;
            rb.isKinematic = true;*/
            animator.SetBool("OnVine", true);
            rb.gravityScale = 0;
            //rb.simulated = false;
            
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Vine")
        {
            //rb.gravityScale = 0;
            rb.gravityScale = 1;
            //rb.isKinematic = true;
            //rb.simulated = true;
            
        }
    }
}

//Problem: gravity after jump 
//Problem: off the ground, player don't stop moving