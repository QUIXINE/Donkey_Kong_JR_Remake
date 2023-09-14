using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    //Input
    private float horizontal;
    private float horizontalOnVine;
    private float vertical;
    float horizontalOnVine02;

    private bool facingRight;       //Flipping

    //State
    private bool isTwoHanded;
    private bool isNewState;
    PlayerState currentState;

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

    [Header("Vine Check")]
    [SerializeField] private LayerMask vineLayerMask;
    [SerializeField] private Transform vineCheckPosBody;
    [SerializeField] private Transform vineCheckPosOnHead;
    [SerializeField] private float vineCheckRadius;
    [SerializeField] private Transform vineCheckPosDualHand01, vineCheckPosDualHand02;
    [SerializeField] private float distanceCheckOnHand;
    [SerializeField] private float radiusCheckOnHand;
    [SerializeField] private float rayDistanceGetCloseToVine;
    private bool canChangeToReach;
    private bool canReachFirstGetOnVine;        //used to allow player to reach out on the same side after get on vine first time(after getting off the ground and get on the vine) 
                                                //Why? --> beacause if don't do this player can't do as the reason say
    private float reachToL01 = 0.2f, reachToL02 = 0.3f, reachToR01 = 0.26f, reachToR02 = 0.31f, reachToR03 = 0.35f;         //used to move position
    private bool reachToRTriggered01, reachToRTriggered02, reachToRTriggered03, reachToLTriggered01, reachToLTriggered02;   //check which reachToL and reachToR is used to move position
    private bool canGetToAnotherVine;           //used as a condition if player allowed to get, and to not immediately get to another vine
    private bool isBackFromAnotherVine;         //used as a condition if player get back from another vine to check if player hands are off the vine
    private bool canReachVineCloser;            //used as a condition to change DK jr. position to get closer to the vine while player is Dual-Handed
    private bool checkGravityVineExit;
    private bool canGetOffVine;
    #endregion

    private bool isDualHanded;
    private bool canFlip;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isOnVine = false;
        canChangeToReach = false;
        canReachFirstGetOnVine = true;
        canGetToAnotherVine = false;
        isBackFromAnotherVine = false;
        canReachVineCloser = true;
        checkGravityVineExit = false;
        currentState = PlayerState.Idle;
        canFlip = true;
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
        DistanceToReachVineCloser();
        OnVineGravityCheck();
       print("canFlip " + canFlip);
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
        /* if (IsGrounded())*/ //Problem: only get input while on the ground, this makes when not on ground player move on itself because it takes the last input
        if (!IsOnVine() && IsGroundedChecker())
            horizontal = Input.GetAxisRaw("Horizontal"); //--> change to DKHorizontal
        if (IsOnVine() || FoundAnotherVine() && IsTwoHanded() && isTwoHanded && currentState == PlayerState.TwoHanded)
        {
            horizontalOnVine = Input.GetAxis("Horizontal"); //--> change to DKHorizontal
        }
        else
        {
            horizontalOnVine = 0;
        }

        if (isDualHanded || IsOnVine() || FoundAnotherVine())
        {
            horizontalOnVine02 = Input.GetAxis("Horizontal"); //--> change to DKHorizontal
        }
        else
        {
            horizontalOnVine02 = 0;
        }
        
        if (currentState != PlayerState.DualHanded)
        {
            isDualHanded = false;
        }
        else { isDualHanded = true; }


        if (IsOnVine() || FoundAnotherVine())
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
        else if (!FoundAnotherVine() || !isOnVine)
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
        if (IsOnVine())
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
        
        if(IsGroundedChecker() && !IsOnVine())
        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
        else if (IsOnVine())
        {
            horizontal = 0;
            rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);

        }
        /* if(horizontal == 1 || horizontal == -1)
         transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0, 0);*/  //Try not use rb
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X) && IsGroundedChecker()) 
        {
            //moveSpeed = onJumpMoveSpeed;    //for jump straight the get 0 speed
            animator.SetBool("StopJump", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            animator.SetBool("Jump", true);
            //transform.Translate(0, jumpHeight, 0);
        }
    }
    
    
    private void StateManager()
    {
        print(currentState);
        /*if (!isTwoHanded && !canReach && isOnVine && currentState != PlayerState.DualHanded)
        {
            isTwoHanded = true;
            currentState = PlayerState.TwoHanded;
        }*/
        switch (currentState)
        {
            case PlayerState.TwoHanded:
                    isTwoHanded = true;
                animator.SetBool("Jump", false);
                horizontal = 0;
                    TwoHandedState();
                break;
            
            case PlayerState.DualHanded:
                    isDualHanded = true;
                animator.SetBool("Jump", false);
                horizontal = 0;
                DualHandedState();
                break;
        }
    }

    private void TwoHandedState()
    {
        if (isNewState && isTwoHanded)
        {
            StopAllCoroutines();
            WaitToChangeState(currentState, true);
            isNewState = false;
        }


        /*//Check if hands off the vine after GetToAnotherVine
        if (transform.rotation == Quaternion.Euler(0, -180, 0) && isBackFromAnotherVine)   //Hold L
        {
            Vector2 posAfterGetToAnotherVine = transform.position;
            posAfterGetToAnotherVine.x += GetDistanceToHoldVineCloser();
            transform.position = posAfterGetToAnotherVine;
            isBackFromAnotherVine = false;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0) && isBackFromAnotherVine)  //Hold R
        {
            Vector2 posAfterGetToAnotherVine = transform.position;
            posAfterGetToAnotherVine.x += GetDistanceToHoldVineCloser();
            transform.position = posAfterGetToAnotherVine;
            isBackFromAnotherVine = false;
        }*/
        //Flip from the another side
        //used with horizontalOnVine to flip DK jr. from one side to another while OnVine
        //isOnVine will be true after being sure that DK jr hands don't go off
        //Problem: pos doesn't based on offset of vine and player
        if (horizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && !canChangeToReach && canFlip)
        {
            if (IsOnVine() && facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x += 0.4f;
                    transform.position = pos;
                    StartCoroutine(WaitToChangeState(currentState)); //canChangeToReach = true, after 0.5f sec.
                }
                
            }
        }
        else if (horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && !canChangeToReach && canFlip)
        {
            if (IsOnVine() && !facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x -= 0.4f;
                    transform.position = pos;
                    StartCoroutine(WaitToChangeState(currentState)); //canChangeToReach = ture, after 0.5f sec.
                }
            }
        }

         //Used for reaching when get on the vine every first time after get off the ground, 9/14/2023 now used for transit to reach
         if (/*Input.GetKeyDown(KeyCode.LeftArrow)*/horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && canReachFirstGetOnVine && !canChangeToReach )
         {
             if (IsOnVine() && facingRight && isTwoHanded)
             {
                 canReach = true;
                 canChangeToReach = true;
                 canReachFirstGetOnVine = false;
                //TransitState(PlayerState.DualHanded);
                StartCoroutine(WaitAndTransitState());
            }
         }
         else if (/*Input.GetKeyDown(KeyCode.RightArrow)*/horizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && canReachFirstGetOnVine && !canChangeToReach)
         {
             if (IsOnVine() && !facingRight && isTwoHanded)
             {
                 canReach = true;
                 canChangeToReach = true;
                 canReachFirstGetOnVine = false;
                //TransitState(PlayerState.DualHanded);
                StartCoroutine(WaitAndTransitState());
            }
         }

        print(canChangeToReach);

        //use when if don't reach out, canChangeToReach will be false, and Use after get back from dual-handed and want to flip to another side of the vine
        if (horizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && canChangeToReach)
        {
            //StopAllCoroutines();
            canChangeToReach = false;   //false so that player can flip to another side after get back from dual-handed
        }
        else if (horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && canChangeToReach)
        {
            //StopAllCoroutines();
            canChangeToReach = false;   //false so that player can flip to another side after get back from dual-handed
        }

       //Transit to dual-handed
        else if (horizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
            canReach = true;
            StopAllCoroutines();
            StartCoroutine(WaitAndTransitState());
            TransitState(PlayerState.DualHanded);
            //StartCoroutine(WaitState());
        }
        else if (horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
            canReach = true;
            StopAllCoroutines();
            StartCoroutine(WaitAndTransitState());
            TransitState(PlayerState.DualHanded);
            //StartCoroutine(WaitState());
        }
    }
    private void TwoHandedClimb()
    {
        if (IsOnVine() && isOnVine && currentState == PlayerState.TwoHanded)
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

    #region Handle States Methods
    private void TransitState(PlayerState newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            isNewState = true;
        }
    }
    
    private IEnumerator WaitAndTransitState()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerState newState = new PlayerState();
        if (currentState == PlayerState.TwoHanded)
        {
            newState = PlayerState.DualHanded;
            canFlip = false;
           
        }
        else if (currentState == PlayerState.DualHanded)
        {
            isDualHanded = false;
            canChangeToReach = false;
            isTwoHanded = true;
            canReachFirstGetOnVine = true;
            canGetToAnotherVine = false;
            isBackFromAnotherVine = true;
            canReachVineCloser = true;
            canGetOffVine = false;
            canReach = false;
           
            if (isOnVine)
            {
                newState = PlayerState.TwoHanded;
            }
            else if (!isOnVine)
                newState = PlayerState.Idle;

            
        }
        
        TransitState(newState);
    }

    private IEnumerator WaitToChangeState(PlayerState currentState)
    {
        yield return null;
        if (currentState == PlayerState.TwoHanded)
        {
            yield return new WaitForSeconds(0.5f);
            canChangeToReach = true;
        }
        if (currentState == PlayerState.DualHanded)
        {
            yield return new WaitForSeconds(0.5f);
            canGetToAnotherVine = true;
        }
    }
    private IEnumerator WaitToChangeState(PlayerState currentState, bool check)
    {
        yield return new WaitForSeconds(0.5f);
        canFlip = check;
        print("WaitToChangeState called");
        if (this.currentState == PlayerState.DualHanded)
        {
            canGetOffVine = true;
            
        }
        else if (this.currentState == PlayerState.TwoHanded)
        {
            print("canFlip");
            canFlip = true;
            
        }

        //if(currentState == PlayerState.TwoHanded)
        
    }

    #endregion

}


/*Problems
1.Problem: off the ground, player don't stop moving
2.Problem: In original game, after get down from the vine to the ground, can't move for a short time, but now mine can move
3.immediately after get to the ground
4.Problem: if holds horizontalOnVine player will move to that side such as jump to catch the vine from left side which has to hold right arrow,
    player will move to right side, but if only press it carefully, player will get on the left side
5.***Problem: Climbing speed errors, climb up with one hand speed is not equal climbUpSpeed and down is not climbDownSpeed after flipping of Idle on vine
    it takes climbDualHandSpeed instead
6.***Problem: Idle state get to reach state immediately, can't flip left-right of climbIdle animation but get to reach state instead, but after get back to Idle state, can flip.
7.Problem: Don't go down when there's no any vines on another side
    (1 - Transit to two-handed)Problem +=, -= always apply
8.Problem: reaching out doesn't wait the normal flip to finish as stepby step but imdediately reach out 
9.Problem: Fix all the changing DK jr position when get to the vine, for ex, jump under or even from the side of the vine to get on it, sometimes the position is not perfect or even get off the vine
10.***Check pos on Vine, sometime does't on the vine because of player postion
11.***Reach first time only one and can't
12.***GetDistanceToGetCloseVine() and Reach out/back have to be the same values. how much Reach out, Reach back has to be the same
13.Animation fall from height and fall from vine are not the same
14.Get to another vine x,y pos
15.After getting on another vine, if hold the btn, player will move to another side of the vine immediately, for ex, from Two-Handed L holding RightArrow after Dual-Handed R to get to another vine player move to the R of the another vine immediately
//instead of L(only see it in slight time, player can't really tell it's go to L before R) before to R, in the original game player will see L side and longer than this situation
*/

/*Solved
//Solved: gravity after jump, when collide with vine player float up
//Solved: fliping doesn't move sprite to left or right side of the vine, but only flip itself
//--> solved by move position a little, look in FlipInput()
//Solved: if OnGround and hold the x (jump), the jump animation will continue to play, and by getting on vine then get back to the ground
//--> solved by add another bool animation check "StopJump"

//Code in Used for reaching when get on the vine every first time after get off the ground
//Solved: Try to fix after get up on vine don't reach out, when player gets up on the left side and try to reach out to the left side
//player can't reach out --> solved by 2 conditions below
//Solved: reach out immediately when flip to another side 
//solved by usising Input.GetKeyDown instead of horizontalOnVine, becasuse horizontalOnVine will make player reach out immediately after push the btn
//horizontalOnVine == 1 || -1 which meets the condition 

Solved: reach first time R while there's no vine on right side, player doesn't move to R side so that the hand will be on vine, maybe because checker condition only checks
when Raycast hits the vine (Vector2.right -- there's no any vines on Vector2.right, so it doesn't meet condition) 
//--> solved by adding more condition in GetDistanceToGetCloseVine() using hitReachRWithNoVine Raycast
//Solved: -=, += always apply, means position will always change and if they don't + and - each other, the hand position will not be on the vine
//Solved: Reach R checker (GetDistanceToGetCloseVine()) doesn't check

*/

//8/31/2023
//make dk jr's hands are on the vine within the its boundary

//9/14/2023 
//To-do-list
//1.Fall from vine (!FoundAnotherVine() && push btn of the reach side again (L then L again))
//2.Player life
//3.Score

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