using UnityEngine;  
using System.Collections;

namespace PlayerSpace
{
    public partial class Player
    {
        private void StateManager()
        {
            switch (CurrentState)
            {
                case PlayerState.TwoHanded:
                    canGetPointFromEnemy = false;
                    isTwoHanded = true;
                    animator.SetBool("Jump", false);
                    Horizontal = 0;
                    TwoHandedState();
                    //problem 40
                    // FlipBackFromObstacle();
                    break;

                case PlayerState.DualHanded:
                    canGetPointFromEnemy = false;
                    isDualHanded = true;
                    animator.SetBool("Jump", false);
                    Horizontal = 0;
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


            if (HorizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && !canChangeToReach && canFlip && !IsThereBoundaryAsObstacle() && !IsThereGroundAsObstacle())
            {
                if (IsOnVineChecker() && facingRight && isTwoHanded)
                {
                    if (IsOnVine)
                    {
                        Flip();
                        Vector2 pos = transform.position;
                        pos.x += 0.4f;
                        transform.position = pos;
                        StartCoroutine(WaitToChangeState(CurrentState)); //canChangeToReach = true, after 0.5f sec.
                    }

                }
            }
            else if (HorizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && !canChangeToReach && canFlip && !IsThereBoundaryAsObstacle() && !IsThereGroundAsObstacle())
            {
                if (IsOnVineChecker() && !facingRight && isTwoHanded)
                {
                    if (IsOnVine)
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
            if (HorizontalOnVine < -0.32f   && transform.rotation == Quaternion.Euler(0, -180, 0) && canReachFirstGetOnVine && !canChangeToReach)
            {
                if(IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    if (IsOnVineChecker() && facingRight && isTwoHanded)
                    {
                        canReach = true;
                        canChangeToReach = true;
                        canReachFirstGetOnVine = false;
                        TransitState(PlayerState.DualHanded);
                        animator.SetBool("TwoHanded", false);
                    }
                }
                else if(!IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())  
                {
                    if (IsOnVineChecker() && facingRight && isTwoHanded)
                    {
                        canReach = true;
                        canChangeToReach = true;
                        canReachFirstGetOnVine = false;
                        TransitState(PlayerState.DualHanded);
                        animator.SetBool("TwoHanded", false);
                    }
                }
                
            }
            else if (/*  Input.GetKeyDown(KeyCode.RightArrow) */ HorizontalOnVine > 0.32f  && transform.rotation == Quaternion.Euler(0, 0, 0) && canReachFirstGetOnVine && !canChangeToReach && !IsThereGroundAsObstacle())
            {
                if(IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    if (IsOnVineChecker() && !facingRight && isTwoHanded)
                    {
                        canReach = true;
                        canChangeToReach = true;
                        canReachFirstGetOnVine = false;
                        TransitState(PlayerState.DualHanded);
                        animator.SetBool("TwoHanded", false);
                    }
                }
                else if(!IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())  
                {
                    if (IsOnVineChecker() && !facingRight && isTwoHanded)
                    {
                        canReach = true;
                        canChangeToReach = true;
                        canReachFirstGetOnVine = false;
                        TransitState(PlayerState.DualHanded);
                        animator.SetBool("TwoHanded", false);
                    }
                }
            
            }


            //use when if don't reach out, canChangeToReach will be false, and Use after get back from dual-handed and want to flip to another side of the vine
            if (HorizontalOnVine > 0 && transform.rotation == Quaternion.Euler(0, -180, 0) && canChangeToReach)
            {
                canChangeToReach = false;   //false so that player can flip to another side after get back from dual-handed
            }
            else if (HorizontalOnVine < 0 && transform.rotation == Quaternion.Euler(0, 0, 0) && canChangeToReach)
            {
                canChangeToReach = false;   //false so that player can flip to another side after get back from dual-handed
            }

            //Transit to dual-handed
            //Solved: Player has to press two time to get to Dual-Handed (problem: wait to transit state), so I think I'll use isBackFromAnotherVine so if they back from Dual-Handed they can use another condition
            //and this one isBackFromAnotherVine == false
            //--> solved by using TransitState();
            else if (/* Input.GetKey(KeyCode.RightArrow) */HorizontalOnVine > 0.32f && transform.rotation == Quaternion.Euler(0, 0, 0) && !canReach && isTwoHanded && canChangeToReach && !IsThereGroundAsObstacle())
            {
                if(IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    canReach = true;
                    StopAllCoroutines();
                    animator.SetBool("TwoHanded", false);
                    TransitState(PlayerState.DualHanded);
                }
                else if(!IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    canReach = true;
                    StopAllCoroutines();
                    animator.SetBool("TwoHanded", false);
                    TransitState(PlayerState.DualHanded);
                }
            }
            else if (HorizontalOnVine < -0.32f && transform.rotation == Quaternion.Euler(0, -180, 0) && !canReach && isTwoHanded && canChangeToReach && !IsThereGroundAsObstacle())
            {
                if(IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    canReach = true;
                    StopAllCoroutines();
                    animator.SetBool("TwoHanded", false);
                    TransitState(PlayerState.DualHanded);       //if transit immediately Dk will sometime DualHanded so quick
                }
                else if(!IsThereBoundaryAsObstacle() && !IsThereKeyAsObstacle())
                {
                    canReach = true;
                    StopAllCoroutines();
                    animator.SetBool("TwoHanded", false);
                    TransitState(PlayerState.DualHanded);       //if transit immediately Dk will sometime DualHanded so quick
                }
            }
        }

        private void DualHandedState()
        {
            animator.SetBool("IsGrounded", false);
            animator.SetBool("Jump", false);
            isTwoHanded = false;
            isDualHanded = true;

            if (isNewState)
            {
                StopAllCoroutines();
                isNewState = false;
            }

            #region Reach out
            if (canReach && HorizontalOnVine02 > 0.25f && transform.rotation == Quaternion.Euler(0, 0, 0))        //Two-Handed R to Reach R
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
            else if (canReach && HorizontalOnVine02 < -0.25f && transform.rotation == Quaternion.Euler(0, -180, 0)) //Two-Handed L to Reach L
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
            if (!canReach && HorizontalOnVine02 < 0f && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))        //Reach R to Two-Handed R (and flipfrom R side to L side)
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
            else if (!canReach && HorizontalOnVine02 > 0f && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))      //Reach L to Two-Handed L (and floip from L side to R side)
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
            if (!FoundAnotherVine() && IsOnVine &&  animator.GetBool("DualHand"))
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
                    else if (HorizontalOnVine02 < -0.3f && canFallFromVine && !canReach) //Get off and Fall from vine
                    {
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
                    else if (HorizontalOnVine02 > 0.3f && canFallFromVine && !canReach) //Get off and Fall from vine
                    {
                        //canReach is false while TwoHanded makes player can fall while TwoHanded not DualHanded
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
            //Check sth. (used)
            else if(FoundAnotherVine() && !IsHandOnMainVine() && IsOnVine && animator.GetBool("DualHand"))
            {
                if(transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (Vertical < 0)
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
                    if (Vertical < 0)
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
                if (IsOnVine)
                {
                    newState = PlayerState.TwoHanded;
                }
                else if (!IsOnVine)
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