using UnityEngine;
using System.Collections.Generic;
using TakeDamage;
using ScoreManagement;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
sealed public partial class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle ,TwoHanded, DualHanded
    }
   
    #region Variables
    //Other type access
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D dkCol;
    private Player_TakeDamage playerTakeDamage;

    //Input
    public float Horizontal         {get; private set;}
    public float HorizontalOnVine   {get; private set;}
    public float HorizontalOnVine02 {get; private set;}
    public float Vertical           {get; private set;}

    //Flipping
    private bool facingRight;       

    //State
    public PlayerState CurrentState {get; private set;}
    private bool isTwoHanded;
    private bool isDualHanded;

    private bool isNewState;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    
    [Header("Climb")]
    [SerializeField] private float climbUpSpeed;
    [SerializeField] private float climbDownSpeed;
    [SerializeField] private float climbDualHandSpeed; //faster than climbUpSpeed, slower than climbDownSpeed
    

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpPadForceDefault;
    [SerializeField] private int jumpPadForceSpecial;
    public bool IsJumped {get; private set;}

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos01;
    [SerializeField] private Transform groundCheckPos02;
    [SerializeField] private Transform groundCheckPos03;
    [SerializeField] private Transform groundCheckPosOnHead;
    [SerializeField] private LayerMask groundLayerMask;
    [Tooltip("length of x and y axis, has 'Ground Check Position' as the center pivot. Used as vector point x and y in OverlapBox()")]
    [SerializeField] private float xLengthGroundCheck, yLengthGroundCheck;      //0.2, 0.05
    
    #region Vine Check
    
        [Header("Vine Check")]
        [SerializeField] private LayerMask vineLayerMask;
        [SerializeField] private Transform vineCheckPosBody;
        [SerializeField] private Transform vineCheckPosOnHead, vineCheckPosOnHead02;
        [SerializeField] private Transform vineCheckPosDualHand01, vineCheckPosDualHand02;
        [SerializeField] private float rayDistanceOnBody;
        [Tooltip("length of x axis, has 'Vine Check Position On Head' as the center pivot")]
        [SerializeField] private float xLengthCheckOnHead;
        [SerializeField] private float rayDistanceOnHead;
        [Tooltip("radius of 'Vine Check Position On DualHand02' used in OverlapCircle to check if player is two-handed")]
        [SerializeField] private float rayDistanceOnHand;       //used in FoundAnotherVine()
        [SerializeField] private float rayDistanceGetCloseToVine;
        [SerializeField] private float rayDistanceToReachVineCloser;
        public bool IsOnVine {get; private set;}
        private bool canFlip;
        private bool canChangeToReach;
        private bool canReach;
        private bool canReachFirstGetOnVine;        //used to allow player to reach out on the same side after get on vine first time(after getting off the ground and get on the vine) 
                                                    //Why? --> beacause if don't do this player can't do as the reason say
        //private bool canReachVineCloser;            //used as a condition to change DK jr. position to get closer to the vine while player is Dual-Handed
        private bool canGetToAnotherVine;           //used as a condition if player allowed to get, and to not immediately get to another vine
        private bool reachToRTriggered01, reachToRTriggered02, reachToRTriggered03, reachToLTriggered01, reachToLTriggered02;   //check which reachToL and reachToR is used to move position
        private float reachToL01 = 0.2f, reachToL02 = 0.3f, reachToR01 = 0.26f, reachToR02 = 0.31f, reachToR03 = 0.35f;         //used to move position
        private bool canFallFromVine;
        private bool checkGravityVineExit;
        
    #endregion

    [Header("Enemy Check")]
    [SerializeField] private LayerMask enemyLayerMask;
    private bool canGetPointFromEnemy;
    private List<EnemyScore> enemyList = new List<EnemyScore>();    //used to get score from enemy, used w/ PlayerGetScore script
    private int scoreOfEnemy;

    [Header("Water Check")]
    [SerializeField] private LayerMask waterLayerMask;
    public bool collideWithWater {get; private set;} //Water colliding check

    [Header("Obstacle Check")]
    [SerializeField] private LayerMask boundaryLayerMask;
    [SerializeField] private Transform obstacleCheckPos01;
    [SerializeField] private Transform obstacleCheckPos02;
    [SerializeField] private float xLengthObstacleCheck;
    [SerializeField] private float yLengthObstacleCheck;

    #endregion

    [SerializeField] private LayerMask itemLayerMask;
    [SerializeField] private ParticleSystem waterSplashParticle;
    [SerializeField] private ParticleSystem vineFallenLeafParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerTakeDamage = GetComponent<Player_TakeDamage>();
        dkCol = GetComponent<Collider2D>();
        IsOnVine = false;
        canChangeToReach = false;
        canReachFirstGetOnVine = true;
        canGetToAnotherVine = false;
        checkGravityVineExit = false;
        CurrentState = PlayerState.Idle;
        canFlip = true;
        isDualHanded = false;
        canGetPointFromEnemy = false;
        gameObject.layer = 9;
        collideWithWater = false;
    }

    void Update()
    {
        IsGrounded();
        GetInput();
        Jump();
        OnGroundFlip();
        HandleAnimation();
        MovePosGetOnVine();
        StateManager();
        IfOnVine();
        ReachVineCloser();
        HandleGravity();
        HandleCollider();
        FlipBackFromObstacle();
    }

    private void FixedUpdate()
    {
        Move();
        if(CurrentState == PlayerState.TwoHanded)
            TwoHandedClimb();
        if(CurrentState == PlayerState.DualHanded)
            DualHandedClimb();
    }

    private void GetInput()
    {
        if (CurrentState != PlayerState.DualHanded)
        {
            isDualHanded = false;
        }
        else if (CurrentState == PlayerState.DualHanded) { isDualHanded = true; }
        /* if (IsGrounded())*/ //Problem: only get input while on the ground, this makes when not on ground player move on itself because it takes the last input
        
        //Walk
        if (!IsOnVineChecker() && IsGroundedChecker())
            Horizontal = Input.GetAxisRaw("DKHorizontal"); 
        else if (!IsGroundedChecker() || IsOnVineChecker())
        {
            Horizontal = 0;
        }

        //Two handed
        if (IsOnVineChecker() /* || FoundAnotherVine() */ && isTwoHanded && CurrentState == PlayerState.TwoHanded)
        {
            HorizontalOnVine = Input.GetAxis("DKHorizontal");
        }
        else if(!IsOnVineChecker() || !IsOnVine)
        {
            HorizontalOnVine = 0;
        }
        else
        {
            HorizontalOnVine = 0;
        }

        if (isDualHanded /* && IsOnVineChecker() */ && CurrentState == PlayerState.DualHanded)
        {
            HorizontalOnVine02 = Input.GetAxis("DKHorizontal"); 
        }
        else
        {
            HorizontalOnVine02 = 0;
        }
        
        

        if (IsOnVineChecker() && CurrentState == PlayerState.TwoHanded)
        {
            Vertical = Input.GetAxisRaw("Vertical");
        }
        else if (FoundAnotherVine() && CurrentState == PlayerState.DualHanded)
        {
            Vertical = Input.GetAxisRaw("Vertical");
        }
        else if (!FoundAnotherVine() && CurrentState == PlayerState.DualHanded)
        {
            Vertical = 0;   //prevents value that is still stuck on 1/-1 when got out of the condition
        }
        


      
        
    }

    private void OnGroundFlip()
    {
        //Original sprite walk animation looks to left side, so at first facingRight will be false
        //if original looks to right side, facingRight will be true
        //Gets input then use it to flip while onGround
        if (!IsOnVine && Horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (!IsOnVine && Horizontal < 0 && facingRight)
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
            if (IsOnVine == false)
            {
                //Instantiate fallen leaf particle after get on vine
                if(SceneManager.GetActiveScene().buildIndex != 6 && SceneManager.GetActiveScene().buildIndex != 5)
                {
                    Vector3 shortenScale = new Vector3(0.2f, 0.2f, 0.2f);
                    vineFallenLeafParticle.transform.localScale =  shortenScale;
                    Instantiate(vineFallenLeafParticle, vineCheckPosDualHand01.position, Quaternion.identity);
                }
                /* if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    Vector2 pos = transform.position;
                    //pos.x += GetDistanceToGetOnVine();
                    transform.position = pos;
                }
                else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    Vector2 pos = transform.position;
                    //pos.x -= GetDistanceToGetOnVine();
                    transform.position = pos;
                } */
                IsOnVine = true;
                animator.SetBool("TwoHanded", true);

                //Get into Two-handed state
                CurrentState = PlayerState.TwoHanded;
            }
        }
        
        //Cllide by head
       else if (CollideVineOnHead())   //Next --> Problem: The hands get off sometimes, check x,y
       {
            //Condition to make Dk jr's hands don't go off the vine while OnVine, but is in the boundary of the vine
            if (IsOnVine == false)
            {
                //Instantiate fallen leaf particle after get on vine
                if(SceneManager.GetActiveScene().buildIndex != 6 && SceneManager.GetActiveScene().buildIndex != 5)
                {
                    Vector3 shortenScale = new Vector3(0.2f, 0.2f, 0.2f);
                    vineFallenLeafParticle.transform.localScale =  shortenScale;
                    Instantiate(vineFallenLeafParticle, vineCheckPosDualHand01.position, Quaternion.identity);
                }
               
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
                
                IsOnVine = true;
                animator.SetBool("TwoHanded", true);

                //Get into Two-handed state
                CurrentState = PlayerState.TwoHanded;
            }
       }
    }


    #region Movement and States Methods
    private void Move()
    {
        //On ground move
        //if there's no IsGrounded, the move while not on ground, move will be last value, the jump will be able to get left and right
        //if there is, when not on ground, will use the last value
        
        if(IsGroundedChecker() &&  !IsOnVineChecker())
        {
            rb.velocity = new Vector2(Horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
        }
        else if (IsOnVineChecker())     //check so that player won't move horizontal while on vine
        {
            Horizontal = 0;
            rb.velocity = new Vector2(Horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);

        }
        /* if(horizontal == 1 || horizontal == -1)
         transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/  //Try not use rb
    }
    
    private void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.X) && IsGroundedChecker() && !IsJumped) 
        {
            IsJumped = true;
            animator.SetBool("StopJump", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("Jump", true);
            canGetPointFromEnemy = true;
            EnemyStack();
            IsJumped = true;
        }
    }
    
    private void TwoHandedClimb()
    {
        if (IsOnVineChecker() && IsOnVine && CurrentState == PlayerState.TwoHanded)
        {
            rb.velocity = new Vector2(rb.velocity.x, (Vertical == 1 ? Vertical * climbUpSpeed : Vertical * climbDownSpeed) * Time.deltaTime);
        }

    }

    private void DualHandedClimb()
    {
        //Climb up with faster speed
        if (FoundAnotherVine() && IsOnVine && CurrentState == PlayerState.DualHanded)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, (Vertical * climbDualHandSpeed) * Time.deltaTime);
            //Problem: if there is this code player can't get off the vine???
        }
        else if (!FoundAnotherVine() && IsOnVine && CurrentState == PlayerState.DualHanded)
        {
            Vertical = 0;
            rb.velocity = new Vector2(rb.velocity.x, (Vertical * climbDualHandSpeed) * Time.deltaTime);
        }
    }
    #endregion

    
}
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