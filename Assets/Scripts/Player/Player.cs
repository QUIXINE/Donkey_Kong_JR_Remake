using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
sealed public partial class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle ,TwoHanded, DualHanded
    }
   
    #region Variables
    //Other class access
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerTakeDamage playerTakeDamage;

    //Input
    private float horizontal;
    private float horizontalOnVine;
    private float vertical;
    float horizontalOnVine02;

    private bool facingRight;       //Flipping

    //State
    PlayerState currentState;
    private bool isTwoHanded;
    private bool isDualHanded;

    private bool isNewState;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    
    [Header("Climb")]
    [SerializeField] private float climbUpSpeed;
    [SerializeField] private float climbDownSpeed;
    [SerializeField] private float climbDualHandSpeed; //faster than climbUpSpeed, slower than climbDownSpeed
    private bool isOnVine;
    private bool canReach;

    [Header("Jump and Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int jumpHeight;
    [SerializeField] private int jumpPadForce;
    private bool canGetPointFromEnemy;


    [Header("Vine Check")]
    [SerializeField] private LayerMask vineLayerMask;
    [SerializeField] private Transform vineCheckPosBody;
    [SerializeField] private Transform vineCheckPosOnHead;
    [SerializeField] private Transform vineCheckPosDualHand01, vineCheckPosDualHand02;
    [SerializeField] private float rayDistanceOnBody;
    [SerializeField] private float radiusCheckOnHand;
    [SerializeField] private float rayDistanceOnHand;
    [SerializeField] private float rayDistanceGetCloseToVine;
    private bool canChangeToReach;
    private bool canReachFirstGetOnVine;        //used to allow player to reach out on the same side after get on vine first time(after getting off the ground and get on the vine) 
                                                //Why? --> beacause if don't do this player can't do as the reason say
    private float reachToL01 = 0.2f, reachToL02 = 0.3f, reachToR01 = 0.26f, reachToR02 = 0.31f, reachToR03 = 0.35f;         //used to move position
    private bool reachToRTriggered01, reachToRTriggered02, reachToRTriggered03, reachToLTriggered01, reachToLTriggered02;   //check which reachToL and reachToR is used to move position
    private bool canGetToAnotherVine;           //used as a condition if player allowed to get, and to not immediately get to another vine
    //private bool isBackFromAnotherVine;         //used as a condition if player get back from another vine to check if player hands are off the vine
    private bool canReachVineCloser;            //used as a condition to change DK jr. position to get closer to the vine while player is Dual-Handed
    private bool checkGravityVineExit;
    private bool canGetOffVine;
    
    [Header("Enemy Check")]
    private List<Enemy> enemyList = new List<Enemy>();
    [SerializeField] private LayerMask enemyLayerMask;


    #endregion

    private bool canFlip;

    private int jumpTimes;
    private int score;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTakeDamage = GetComponent<PlayerTakeDamage>();
        isOnVine = false;
        canChangeToReach = false;
        canReachFirstGetOnVine = true;
        canGetToAnotherVine = false;
        canReachVineCloser = true;
        checkGravityVineExit = false;
        currentState = PlayerState.Idle;
        canFlip = true;
        isDualHanded = false;
        canGetPointFromEnemy = false;
        jumpTimes = 1;
    }

    void Update()
    {
        IsGrounded();
        IsOnVine();
        GetInput();
        Jump();
        OnGroundFlip();
        HandleAnimation();
        MovePosGetOnVine();
        StateManager();
        ReachVineCloser();
        OnVineGravityCheck();
    }

    private void FixedUpdate()
    {
        Move();
        if(currentState == PlayerState.TwoHanded)
            TwoHandedClimb();
        if(currentState == PlayerState.DualHanded)
            DualHandedClimb();
    }

    private void GetInput()
    {
        if (currentState != PlayerState.DualHanded)
        {
            isDualHanded = false;
        }
        else if (currentState == PlayerState.DualHanded) { isDualHanded = true; }
        /* if (IsGrounded())*/ //Problem: only get input while on the ground, this makes when not on ground player move on itself because it takes the last input
        if (!IsOnVineChecker() && IsGroundedChecker())
            horizontal = Input.GetAxisRaw("Horizontal"); //--> change to DKHorizontal
        else if (!IsGroundedChecker())
        {
            horizontal = 0;
        }

        if (IsOnVineChecker() || FoundAnotherVine() && IsTwoHanded() && isTwoHanded && currentState == PlayerState.TwoHanded)
        {
            horizontalOnVine = Input.GetAxis("Horizontal"); //--> change to DKHorizontal
        }
        else
        {
            horizontalOnVine = 0;
        }

        if (isDualHanded && IsOnVineChecker())
        {
            horizontalOnVine02 = Input.GetAxis("Horizontal"); //--> change to DKHorizontal
        }
        else
        {
            horizontalOnVine02 = 0;
        }
        
        

        if (IsOnVineChecker() && currentState == PlayerState.TwoHanded)
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
        else if (FoundAnotherVine() && currentState == PlayerState.DualHanded)
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
        else if (!FoundAnotherVine() && IsOnVineChecker() && currentState == PlayerState.DualHanded)
        {
            vertical = 0;   //prevents value that is still stuck on 1/-1 when got out of the condition
        }


      
        
    }

    private void OnGroundFlip()
    {
        //Original sprite walk animation looks to left side, so at first facingRight will be false
        //if original looks to right side, facingRight will be true
        //Gets input then use it to flip while onGround
        if (!isOnVine && horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (!isOnVine && horizontal < 0 && facingRight)
        {
            Flip();
        }
      
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void MovePosGetOnVine()
    {
        //Collide from L/R side
        if (IsOnVineChecker())
        {
            //Condition to make Dk jr's hands don't go off the vine while OnVine, but is in the boundary of the vine
            if (isOnVine == false)
            {
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x += GetDistanceToGetOnVineCloser();
                    transform.position = pos;
                }
                else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x -= GetDistanceToGetOnVineCloser();
                    transform.position = pos;
                }

                isOnVine = true;
                animator.SetBool("TwoHanded", true);

                //Get into Two-handed state
                currentState = PlayerState.TwoHanded;
            }
        }
        
        //Cllide by head
       else if (CollideVineOnHead())   //Next --> Problem: The hands get off sometimes, check x,y
       {
            //Condition to make Dk jr's hands don't go off the vine while OnVine, but is in the boundary of the vine
            if (isOnVine == false)
            {
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x -= 0.1f;
                    pos.y += 0.4f;
                    transform.position = pos;
                }
                else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    Vector2 pos = transform.position;
                    pos.x += 0.1f;
                    pos.y += 0.4f;
                    transform.position = pos;
                }
                
                isOnVine = true;
                animator.SetBool("TwoHanded", true);

                //Get into Two-handed state
                currentState = PlayerState.TwoHanded;
            }
       }
    }


    #region Movement and States Methods
    private void Move()
    {
        //On ground move
        //if there's no IsGrounded, the move while not on ground, move will be last value, the jump will be able to get left and right
        //if there is, when not on ground, will use the last value
        
        if(IsGroundedChecker() && !IsOnVineChecker())
        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
        else if (IsOnVineChecker())
        {
            horizontal = 0;
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);

        }
        /* if(horizontal == 1 || horizontal == -1)
         transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/  //Try not use rb
    }
    
    private void Jump()
    {
        if(IsGroundedChecker() && rb.velocity.y <= 0)
        {
            canGetPointFromEnemy = false;
            enemyList.Clear();
        }
        if (Input.GetKeyDown(KeyCode.X) && IsGroundedChecker()) 
        {
            //moveSpeed = onJumpMoveSpeed;    //for jump straight the get 0 speed
            jumpTimes = 0;
            animator.SetBool("StopJump", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            animator.SetBool("Jump", true);
            canGetPointFromEnemy = true;
            EnemyStack();
            //transform.Translate(0, jumpHeight, 0);
        }

        /* if(canGetPointFromEnemy)
        {
            CallEnemyStack();
        } */

    }
    
    private void TwoHandedClimb()
    {
        if (IsOnVineChecker() && isOnVine && currentState == PlayerState.TwoHanded)
        {
            rb.velocity = new Vector2(rb.velocity.x, (vertical == 1 ? vertical * climbUpSpeed : vertical * climbDownSpeed) * Time.deltaTime);
        }

    }

    private void DualHandedClimb()
    {
        //Climb up with faster speed
        if (FoundAnotherVine() && isOnVine && currentState == PlayerState.DualHanded)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, (vertical * climbDualHandSpeed) * Time.deltaTime);
            //Problem: if there is this code player can't get off the vine???
        }
        else if (!FoundAnotherVine() && isOnVine && currentState == PlayerState.DualHanded)
        {
            vertical = 0;
            rb.velocity = new Vector2(rb.velocity.x, (vertical * climbDualHandSpeed) * Time.deltaTime);
        }
    }
    #endregion

    
}


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