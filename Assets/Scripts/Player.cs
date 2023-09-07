using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
sealed public class  Player : MonoBehaviour
{
    //Other class access
    private Rigidbody2D rb;
    private Animator animator;

    //Input
    private float horizontal;
    private float horizontalOnVine;
    private float vertical;
    
    private bool facingRight;       //Flipping

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float firstMoveSpeed;
    [SerializeField] private float onJumpMoveSpeed;
    

    [Header("Climb")]
    [SerializeField] private float climbUpSpeed;
    [SerializeField] private float climbDownSpeed;
    private bool isOnVine;

    [Header("Jump and Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int jumpHeight;

    [Header("Vine Check")]
    [SerializeField] private Transform vineCheckPos;
    [SerializeField] private Transform vineCheckPosOnHead;
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
        GetInput();
        Jump();
        FlipInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        Move();
        Climb();
    }

    #region Inputs
    private void GetInput()
    {
        /* if (IsGrounded())*/ //Problem: only get input while on the ground, this makes when not on ground player move on itself because it takes the last input
        if (!OnVine() && IsGrounded())
            horizontal = Input.GetAxisRaw("Horizontal"); //--> change to DKHorizontal
        if (OnVine())
        {
            vertical = Input.GetAxisRaw("Vertical");
           /* horizontalOnVine = Input.GetAxisRaw("Horizontal");*/ //--> change to DKHorizontal
        }
        
        
        if (OnVine() && Input.GetKeyDown(KeyCode.LeftArrow) && !facingRight)
        {
            Flip();
        }
        else if (OnVine() && Input.GetKeyDown(KeyCode.RightArrow) && facingRight)
        {
            Flip();
        }
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
        /*else if (OnVine() && horizontalOnVine < 0 && !facingRight)
        {
            Flip();
        }
        else if (OnVine() && horizontalOnVine > 0 && facingRight)
        {
            Flip();
        }*/ //used with horizontalOnVine
    }
    #endregion

    #region Checkers
    private bool OnVine()
    {
        //check if player jump from the sides of vine
        return Physics2D.Raycast(vineCheckPos.position, -vineCheckPos.right, vineCheckRadius, vineLayerMask);
    }
    private bool CollideVineOnHead()
    {
        //check if player jump from under of vine
        return Physics2D.Raycast(vineCheckPosOnHead.position, vineCheckPos.up, vineCheckRadius, vineLayerMask);
    }
    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayerMask);
        return Physics2D.OverlapBox(groundCheckPos.position, new Vector2(0.8f,0f), 0, groundLayerMask);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(groundCheckPos.position, transform.localScale);
    }
    private void OnGround()
    {
        if (IsGrounded())
        {
            rb.gravityScale = 1;
            animator.SetBool("OnVine", false);
            moveSpeed = firstMoveSpeed;
            animator.SetBool("Jump", false);
            animator.SetFloat("Move", 0);
            vertical = 0;
            isOnVine = false;
        }
    }
    #endregion

    #region Movement
    private void Move()
    {
        //On ground move
        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
        /* if(horizontal == 1 || horizontal == -1)
         transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/  //Try not use rb
    }

    private void Climb()
    {
        //Climbing
        if (OnVine())
        {
            rb.gravityScale = 0;
            horizontal = 0;
            animator.SetBool("OnVine", true);

            //Condition to make Dk jr's hands don't go off the vine while OnVine, but is in the boundary of the vine
            if (isOnVine == false)
            {
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x -= 0.02f;
                    transform.position = pos;
                }
                else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x += 0.02f;
                    transform.position = pos;
                }
                isOnVine = true;
            }
            rb.velocity = new Vector2(rb.velocity.x, (vertical == 1 ? vertical * climbUpSpeed : vertical * climbDownSpeed) * Time.deltaTime);
        }
        else if (CollideVineOnHead())
        {
            rb.gravityScale = 0;
            horizontal = 0;
            animator.SetBool("OnVine", true);

            //Condition to make Dk jr's hands don't go off the vine while OnVine, but is in the boundary of the vine
            if (isOnVine == false)
            {
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x -= 0.2f;
                    pos.y += 0.4f;
                    transform.position = pos;
                }
                else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x += 0.2f;
                    pos.y += 0.4f;
                    transform.position = pos;
                }
                isOnVine = true;
            }
            rb.velocity = new Vector2(rb.velocity.x, (vertical == 1 ? vertical * climbUpSpeed : vertical * climbDownSpeed) * Time.deltaTime);
        }
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

    
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);

    }

    private void HandleAnimation()
    {
        if (horizontal > 0 || horizontal < 0) //if move, player will get out of the first state
            animator.SetTrigger("MoveOut");

        animator.SetFloat("Move", horizontal);
        animator.SetFloat("Climb", vertical);
    }
}

//Problem: gravity after jump 
//Problem: off the ground, player don't stop moving
//Problem: fliping doesn't move sprite to left or right side of the vine, but only flip itself
//Problem: In original game, after get down from the vine to the ground, can't move for a short time, but now mine can move
//immediately after get to the ground
//Problem : if OnGround and hold the x (jump), the jump animation will continue to play, and by getting on vine then get back to the ground

//8/31/2023
//make dk jr's hands are on the vine within the its boundary

/*Check layermask
private LayerMask GetLayerMask()
{
    RaycastHit2D hitInfo = Physics2D.Raycast(groundLayerCheckPos.position, groundLayerCheckPos.up * -1, groundCheckRadius);
    LayerMask mask = new LayerMask();
    if (hitInfo.collider != null)
    {
        mask.value = hitInfo.collider.gameObject.layer;
        Debug.Log(hitInfo.collider.gameObject.layer);
        return hitInfo.collider.gameObject.layer;
    }
    return mask;
}*/