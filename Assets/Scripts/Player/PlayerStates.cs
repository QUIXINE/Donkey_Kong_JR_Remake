using UnityEngine;  
using System.Collections;

namespace PlayerSpace
{
public partial class Player
{
    private void StateManager()
    {
        print("Get in to first state");
        /*if (!isTwoHanded && !canReach && isOnVine && currentState != PlayerState.DualHanded)
        {
            isTwoHanded = true;
            currentState = PlayerState.TwoHanded;
        }*/
        switch (CurrentState)
        {
            case PlayerState.TwoHanded:
                canGetPointFromEnemy = false;
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
        animator.SetBool("IsGrounded", false);
        isDualHanded = false;
        if (isNewState && isTwoHanded)
        {
            StopAllCoroutines();
            WaitToChangeState(CurrentState, true);
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
            if (IsOnVineChecker() && facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x += 0.4f;
                    transform.position = pos;
                    StartCoroutine(WaitToChangeState(CurrentState)); //canChangeToReach = true, after 0.5f sec.
                }

            }
        }
        else if (horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && !canChangeToReach && canFlip)
        {
            if (IsOnVineChecker() && !facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x -= 0.4f;
                    transform.position = pos;
                    StartCoroutine(WaitToChangeState(CurrentState)); //canChangeToReach = ture, after 0.5f sec.
                }
            }
        }

        //Reaching first time
        //Used for reaching when get on the vine every first time after get off the ground, 9/14/2023 now used for transit to reach
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.rotation == Quaternion.Euler(0, -180, 0) && canReachFirstGetOnVine && !canChangeToReach)
        {
            if (IsOnVineChecker() && facingRight && isTwoHanded)
            {
                canReach = true;
                canChangeToReach = true;
                canReachFirstGetOnVine = false;
                TransitState(PlayerState.DualHanded);
                animator.SetBool("TwoHanded", false);
                //StartCoroutine(WaitAndTransitState());
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.rotation == Quaternion.Euler(0, 0, 0) && canReachFirstGetOnVine && !canChangeToReach)
        {
            if (IsOnVineChecker() && !facingRight && isTwoHanded)
            {
                canReach = true;
                canChangeToReach = true;
                canReachFirstGetOnVine = false;
                TransitState(PlayerState.DualHanded);
                animator.SetBool("TwoHanded", false);
                //StartCoroutine(WaitAndTransitState());
            }
        }


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
        //Solved: Player has to press two time to get to Dual-Handed (problem: wait to transit state), so I think I'll use isBackFromAnotherVine so if they back from Dual-Handed they can use another condition
        //and this one isBackFromAnotherVine == false
        //--> solved by using TransitState();
        else if (Input.GetKey(KeyCode.RightArrow)/*horizontalOnVine > 0*/ && transform.rotation == Quaternion.Euler(0, 0, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
            canReach = true;
            StopAllCoroutines();
            animator.SetBool("TwoHanded", false);
            //StartCoroutine(WaitAndTransitState());
            TransitState(PlayerState.DualHanded);
            //StartCoroutine(WaitState());
        }
        else if (Input.GetKey(KeyCode.LeftArrow)/*horizontalOnVine < 0*/ && transform.rotation == Quaternion.Euler(0, -180, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
            canReach = true;
            StopAllCoroutines();
            animator.SetBool("TwoHanded", false);
            //StartCoroutine(WaitAndTransitState());
            TransitState(PlayerState.DualHanded);
            //StartCoroutine(WaitState());
        }
    }

    private void DualHandedState()
    {
        print(canReach);
        animator.SetBool("IsGrounded", false);
        isTwoHanded = false;
        isDualHanded = true;

        if (isNewState)
        {
            StopAllCoroutines();
            isNewState = false;
        }

        #region Reach out
        if (canReach && horizontalOnVine02 > 0 && transform.rotation == Quaternion.Euler(0, 0, 0))        //Two-Handed R to Reach R
        {
            print("Reach");
            Flip();
            animator.SetBool("DualHand", true);
            animator.SetBool("TwoHanded", false);
            Vector2 pos = transform.position;
            pos.x += GetDistanceToReachVineCloser();
            transform.position = pos;
            canReach = false;
            StartCoroutine(WaitToChangeState(CurrentState, true));
            StartCoroutine(WaitToChangeState(CurrentState));
        }
        else if (canReach && horizontalOnVine02 < 0 && transform.rotation == Quaternion.Euler(0, -180, 0)) //Two-Handed L to Reach L
        {
            print("Reach");
            Flip();
            animator.SetBool("DualHand", true);
            animator.SetBool("TwoHanded", false);
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToReachVineCloser();
            transform.position = pos;
            canReach = false;
            StartCoroutine(WaitToChangeState(CurrentState, true));
            StartCoroutine(WaitToChangeState(CurrentState));
        }
        #endregion

        //check if dual-handed and try to get back to two-handed
        #region Transit to Two-Handed
        if (!canReach && horizontalOnVine02 < 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))        //Reach R to Two-Handed R (and flipfrom R side to L side)
        {
            print("Transit");
            Flip();
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToReachVineCloser();    //decrease to decrease pos.x += GetDistanceToReachVineCloser(); in Reach out
            transform.position = pos;
            canReachVineCloser = false;
            canGetOffVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState());      //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }
        else if (!canReach && horizontalOnVine02 > 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))      //Reach L to Two-Handed L (and floip from L side to R side)
        {
            print("Transit");
            Flip();
            Vector2 pos = transform.position;
            pos.x += GetDistanceToReachVineCloser();    //plus to decrease pos.x -= GetDistanceToReachVineCloser(); in Reach out
            transform.position = pos;
            canReachVineCloser = false;
            canGetOffVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState()); //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }
        #endregion

        #region Get to another vine
        if (/*Input.GetKey(KeyCode.RightArrow)*/horizontalOnVine02 > 0 && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)   //Get to another vine R
        {
            print("GetToAnotherVine");
            animator.SetBool("DualHand", false);
            canReachVineCloser = false;
            StartCoroutine(WaitAndTransitState());
        }
        else if (/*Input.GetKey(KeyCode.LeftArrow)*/horizontalOnVine02 < 0 && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)      //Get to another vine L
        {
            print("GetToAnotherVine");
            animator.SetBool("DualHand", false);
            canReachVineCloser = false;
            StartCoroutine(WaitAndTransitState());
        }
        #endregion

        #region Get down and Fall from vine conditions
        //DualHanded with no another vine on another side to  two handed to get down
        //Fall from vine, if press L/R arrow again while DualHanded with no another vine on another side DK will fall off the vine
        //Get down off the R side and Fall to the L
        if (!FoundAnotherVine() && isOnVine && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                animator.SetBool("DualHand", false);
                Flip();
                Vector2 pos = transform.position;
                pos.x -= GetDistanceToReachVineCloser();
                transform.position = pos;
                canReachVineCloser = false;
                StartCoroutine(WaitAndTransitState());
            }
            else if (horizontalOnVine02 > 0 && canGetOffVine && !canReach) //Get off and Fall from vine
            {
                Vector2 pos = transform.position;
                // pos.x += 0.2f;           //used this to move DK hands off the vine so that it won't be on vine and fall off thei vine
                transform.position = pos;
                canGetOffVine = false;
                StopAllCoroutines();
                //if isOnVine is false, MovePosGetOnVine() will work which means player state will be TwoHanded and there will be an unplesant situatuion occur
                //This is why I used CurrentState = PlayerState.Idle; instead
                //Why used CurrentState = PlayerState.Idle;? because after pressing L/R arrow again while DualHanded with no another vine on another side
                //player still be in DualHanded state, the gravity is 0, and player won't fall off ***maybe because of the checkers (look for more details why player won't fall off)***
                //the vine so I set the state to Idle and set gravity so that player will fall
                CurrentState = PlayerState.Idle;
            }
        }
            
        //Get down off the L side and Fall to the R
        else if (!FoundAnotherVine() && isOnVine && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                animator.SetBool("DualHand", false);
                Flip();
                Vector2 pos = transform.position;
                pos.x += GetDistanceToReachVineCloser();
                transform.position = pos;
                canReachVineCloser = false;
                StartCoroutine(WaitAndTransitState());
            }
            else if (horizontalOnVine02 < 0 && canGetOffVine && !canReach) //Get off and Fall from vine
            {
                Vector2 pos = transform.position;
                // pos.x -= 0.2f;           //used this to move DK hands off the vine so that it won't be on vine and fall off thei vine
                transform.position = pos;
                canGetOffVine = false;
                StopAllCoroutines();
                //if isOnVine is false, MovePosGetOnVine() will work which means player state will be TwoHanded and there will be an unplesant situatuion occur
                //This is why I used CurrentState = PlayerState.Idle; instead
                //Why used CurrentState = PlayerState.Idle;? because after pressing L/R arrow again while DualHanded with no another vine on another side
                //player still be in DualHanded state, the gravity is 0, and player won't fall off ***maybe because of the checkers (look for more details why player won't fall off)***
                //the vine so I set the state to Idle and set gravity so that player will fall
                CurrentState = PlayerState.Idle;
            }
        }
        #endregion

    }

    #region Handle States Methods
    private void TransitState(PlayerState newState)
    {
        if (newState != CurrentState)
        {
            CurrentState = newState;
            isNewState = true;
        }
    }
    
    private IEnumerator WaitAndTransitState()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerState newState = new PlayerState();
        if (CurrentState == PlayerState.TwoHanded)
        {
            newState = PlayerState.DualHanded;
            canFlip = false;
           
        }
        else if (CurrentState == PlayerState.DualHanded)
        {
            isDualHanded = false;
            canChangeToReach = false;
            isTwoHanded = true;
            canReachFirstGetOnVine = true;
            canGetToAnotherVine = false;
            //isBackFromAnotherVine = true;
            canReachVineCloser = true;
            canGetOffVine = false;
            canReach = false;
            canFlip = true;
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
        //canFlip = check;
        if (this.CurrentState == PlayerState.DualHanded)
        {
            canGetOffVine = true;
            
        }
        else if (this.CurrentState == PlayerState.TwoHanded)
        {
            print("canFlip");
            //canFlip = true;
            
        }

        //if(currentState == PlayerState.TwoHanded)
        
    }

    #endregion

}
}