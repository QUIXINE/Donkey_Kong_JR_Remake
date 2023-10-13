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
        switch (currentState)
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
            if (IsOnVineChecker() && facingRight && isTwoHanded)
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
            if (IsOnVineChecker() && !facingRight && isTwoHanded)
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
        animator.SetBool("IsGrounded", false);
        isTwoHanded = false;
        isDualHanded = true;

        if (isNewState)
        {
            StopAllCoroutines();
            isNewState = false;
        }

        //Reach out
        if (canReach && horizontalOnVine02 > 0 && transform.rotation == Quaternion.Euler(0, 0, 0))        //Two-Handed R to Reach R
        {
            Flip();
            animator.SetBool("DualHand", true);
            animator.SetBool("TwoHanded", false);
            Vector2 pos = transform.position;
            pos.x += GetDistanceToReachVineCloser();
            transform.position = pos;
            canReach = false;
            StartCoroutine(WaitToChangeState(currentState, true));
            StartCoroutine(WaitToChangeState(currentState));
        }
        else if (canReach && horizontalOnVine02 < 0 && transform.rotation == Quaternion.Euler(0, -180, 0)) //Two-Handed L to Reach L
        {
            Flip();
            animator.SetBool("DualHand", true);
            animator.SetBool("TwoHanded", false);
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToReachVineCloser();
            transform.position = pos;
            canReach = false;
            StartCoroutine(WaitToChangeState(currentState, true));
            StartCoroutine(WaitToChangeState(currentState));
        }

        //check if dual-handed and try to get back to two-handed
        //Transit to Two-Handed
        if (!canReach && horizontalOnVine02 < 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))        //Reach R to Two-Handed R (and flipfrom R side to L side)
        {
            Flip();
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToReachVineCloser();
            transform.position = pos;
            canReachVineCloser = false;
            canGetOffVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState()); //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }
        else if (!canReach && horizontalOnVine02 > 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))      //Reach L to Two-Handed L (and floip from L side to R side)
        {
            Flip();
            Vector2 pos = transform.position;
            pos.x += GetDistanceToReachVineCloser();
            transform.position = pos;
            canReachVineCloser = false;
            canGetOffVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState()); //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }

        //Get to another vine
        if (/*Input.GetKey(KeyCode.RightArrow)*/horizontalOnVine02 > 0 && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)   //Get to another vine R
        {
            animator.SetBool("DualHand", false);
            canReachVineCloser = false;
            StartCoroutine(WaitAndTransitState());
        }
        else if (/*Input.GetKey(KeyCode.LeftArrow)*/horizontalOnVine02 < 0 && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)      //Get to another vine L
        {
            animator.SetBool("DualHand", false);
            canReachVineCloser = false;
            StartCoroutine(WaitAndTransitState());
        }


        //Dual with no another vine on another side to  two handed to get down
        //Get down off the R side
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
                pos.x += 0.2f;
                transform.position = pos;
                canGetOffVine = false;
                StopAllCoroutines();
            }
        }
            
        //Get down off the L side
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
                pos.x -= 0.2f;
                transform.position = pos;
                canGetOffVine = false;
                StopAllCoroutines();
            }
        }


    }

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
        if (this.currentState == PlayerState.DualHanded)
        {
            canGetOffVine = true;
            
        }
        else if (this.currentState == PlayerState.TwoHanded)
        {
            print("canFlip");
            //canFlip = true;
            
        }

        //if(currentState == PlayerState.TwoHanded)
        
    }

    #endregion

}
}