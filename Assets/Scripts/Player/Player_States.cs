﻿using UnityEngine;  
using System.Collections;

namespace PlayerSpace
{
public partial class Player
{
    private void StateManager()
    {
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
                //problem 40
                // FlipBackFromObstacle();
                break;

            case PlayerState.DualHanded:
                canGetPointFromEnemy = false;
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
            animator.SetBool("TwoHanded", true);
            StopAllCoroutines();
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
        if (horizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && !canChangeToReach && canFlip && !IsThereObstacle())
        {
            if (IsOnVineChecker() && facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    print("Flip to R");
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x += 0.4f;
                    transform.position = pos;
                    /* if(FlipBackFromObstacle())
                    {
                        print("Flip");
                        Flip();
                    } */
                    StartCoroutine(WaitToChangeState(CurrentState)); //canChangeToReach = true, after 0.5f sec.
                }

            }
        }
        else if (horizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && !canChangeToReach && canFlip && !IsThereObstacle())
        {
            if (IsOnVineChecker() && !facingRight && isTwoHanded)
            {
                if (isOnVine)
                {
                    print("Flip to L");
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x -= 0.4f;
                    transform.position = pos;
                    /* if(FlipBackFromObstacle())
                    {
                        print("Flip");
                        Flip();
                    } */
                    StartCoroutine(WaitToChangeState(CurrentState)); //canChangeToReach = ture, after 0.5f sec.
                }
            }
        }

        //Processing
        //Reaching first time
        //Used for reaching when get on the vine every first time after get off the ground, 9/14/2023 now used for transit to reach
        if ( /* Input.GetKeyDown(KeyCode.LeftArrow */ horizontalOnVine < -0.32f   && transform.rotation == Quaternion.Euler(0, -180, 0) && canReachFirstGetOnVine && !canChangeToReach)
        {
            if (IsOnVineChecker() && facingRight && isTwoHanded)
            {
                //print("Reaching first time to dual-handed L");
                    canReach = true;
                    canChangeToReach = true;
                    canReachFirstGetOnVine = false;
                    TransitState(PlayerState.DualHanded);
                    //StartCoroutine(WaitAndTransitState());
                    animator.SetBool("TwoHanded", false);
            }
        }
        else if (/*  Input.GetKeyDown(KeyCode.RightArrow) */ horizontalOnVine > 0.32f  && transform.rotation == Quaternion.Euler(0, 0, 0) && canReachFirstGetOnVine && !canChangeToReach)
        {
            if (IsOnVineChecker() && !facingRight && isTwoHanded)
            {
                //print("Reaching first time to dual-handed R");
                    canReach = true;
                    canChangeToReach = true;
                    canReachFirstGetOnVine = false;
                    TransitState(PlayerState.DualHanded);
                    //StartCoroutine(WaitAndTransitState());
                    animator.SetBool("TwoHanded", false);
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
        else if (/* Input.GetKey(KeyCode.RightArrow) */horizontalOnVine > 0.32f && transform.rotation == Quaternion.Euler(0, 0, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
                canReach = true;
                StopAllCoroutines();
                animator.SetBool("TwoHanded", false);
                //StartCoroutine(WaitAndTransitState());
                TransitState(PlayerState.DualHanded);
        }
        else if (/* Input.GetKey(KeyCode.LeftArrow) */horizontalOnVine < -0.32f && transform.rotation == Quaternion.Euler(0, -180, 0) && !canReach && isTwoHanded && canChangeToReach)
        {
                canReach = true;
                StopAllCoroutines();
                animator.SetBool("TwoHanded", false);
                //StartCoroutine(WaitAndTransitState());    //if transit wait time Dk will get to DualHanded while TwoHanded animation and have to press 2 times (first will get to DualHanded state, another for reaching out) to reach out
                TransitState(PlayerState.DualHanded);       //if transit immediately Dk will sometime DualHanded so quick
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

        #region Reach out
        if (canReach && horizontalOnVine02 > 0.25f && transform.rotation == Quaternion.Euler(0, 0, 0))        //Two-Handed R to Reach R
        {
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
        else if (canReach && horizontalOnVine02 < -0.25f && transform.rotation == Quaternion.Euler(0, -180, 0)) //Two-Handed L to Reach L
        {
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
        if (!canReach && horizontalOnVine02 < 0f && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))        //Reach R to Two-Handed R (and flipfrom R side to L side)
        {
            Flip();
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToReachVineCloser();    //decrease to decrease pos.x += GetDistanceToReachVineCloser(); in Reach out
            transform.position = pos;
            canFallFromVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState());      //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }
        else if (!canReach && horizontalOnVine02 > 0f && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))      //Reach L to Two-Handed L (and floip from L side to R side)
        {
            Flip();
            Vector2 pos = transform.position;
            pos.x += GetDistanceToReachVineCloser();    //plus to decrease pos.x -= GetDistanceToReachVineCloser(); in Reach out
            transform.position = pos;
            canFallFromVine = false;
            canGetToAnotherVine = false;
            StartCoroutine(WaitAndTransitState()); //maybe can use TransitionToState()
            animator.SetBool("DualHand", false);
        }
        #endregion

        #region Get to another vine
        if (Input.GetKey(KeyCode.RightArrow)/* horizontalOnVine02 > 0.2f */ && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)   //Get to another vine R
        {
            animator.SetBool("DualHand", false);
            StartCoroutine(WaitAndTransitState());
        }
        else if (Input.GetKey(KeyCode.LeftArrow)/* horizontalOnVine02 < -0.2f */ && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)      //Get to another vine L
        {
            animator.SetBool("DualHand", false);
            StartCoroutine(WaitAndTransitState());
        }
        #endregion

        #region Get down and Fall from vine conditions
        //DualHanded with no another vine on another side to  two handed to get down
        //Fall from vine, if press L/R arrow again while DualHanded with no another vine on another side DK will fall off the vine
        if (!FoundAnotherVine() && isOnVine &&  animator.GetBool("DualHand"))
        {
            //Get down off the L side and Fall to the R
            if(transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    animator.SetBool("DualHand", false);
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x += GetDistanceToReachVineCloser();
                    transform.position = pos;
                    StartCoroutine(WaitAndTransitState());
                }
                else if (horizontalOnVine02 < -0.2f && canFallFromVine && !canReach) //Get off and Fall from vine
                {
                    print("Fall < -0.5f");
                    canFallFromVine = false;
                    StopAllCoroutines();
                    //if isOnVine is false, MovePosGetOnVine() will work which means player state will be TwoHanded and there will be an unplesant situatuion occur
                    //This is why I used CurrentState = PlayerState.Idle; instead
                    //Why used CurrentState = PlayerState.Idle;? because after pressing L/R arrow again while DualHanded with no another vine on another side
                    //player still be in DualHanded state, the gravity is 0, and player won't fall off ***maybe because of the checkers (look for more details why player won't fall off)***
                    //the vine so I set the state to Idle and set gravity so that player will fall
                    CurrentState = PlayerState.Idle;
                }
            }
            //Get down off the R side and Fall to the L
            else if(transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    animator.SetBool("DualHand", false);
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x -= GetDistanceToReachVineCloser();
                    transform.position = pos;
                    StartCoroutine(WaitAndTransitState());
                }
                else if (horizontalOnVine02 > 0.2f && canFallFromVine && !canReach) //Get off and Fall from vine
                {
                    //canReach is false while TwoHanded makes player can fall while TwoHanded not DualHanded
                    print("Fall > 0.5f");
                    canFallFromVine = false;
                    StopAllCoroutines();
                    //if isOnVine is false, MovePosGetOnVine() will work which means player state will be TwoHanded and there will be an unplesant situatuion occur
                    //This is why I used CurrentState = PlayerState.Idle; instead
                    //Why used CurrentState = PlayerState.Idle;? because after pressing L/R arrow again while DualHanded with no another vine on another side
                    //player still be in DualHanded state, the gravity is 0, and player won't fall off ***maybe because of the checkers (look for more details why player won't fall off)***
                    //the vine so I set the state to Idle and set gravity so that player will fall
                    CurrentState = PlayerState.Idle;
                }
            }
        }
        else if(FoundAnotherVine() && !IsHandOnMainVine() && isOnVine && animator.GetBool("DualHand"))
        {
            if(transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (vertical < 0)
                {
                    animator.SetBool("DualHand", false);
                    Vector2 pos = transform.position;
                    pos.x -= GetDistanceToReachVineCloser();
                    transform.position = pos;
                    StartCoroutine(WaitAndTransitState());
                }
            }
            else if(transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (vertical < 0)
                {
                    animator.SetBool("DualHand", false);
                    Vector2 pos = transform.position;
                    pos.x += GetDistanceToReachVineCloser();
                    transform.position = pos;
                    StartCoroutine(WaitAndTransitState());
                }
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
        if(CurrentState == PlayerState.TwoHanded)
        {
            yield return new WaitForSeconds(0.05f);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }
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
            canFallFromVine = false;
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
        if (CurrentState == PlayerState.DualHanded)
        {
            canFallFromVine = true;
            
        }
        
    }
    #endregion

}
}