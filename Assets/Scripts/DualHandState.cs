using UnityEngine;   
public partial class Player
{

        private void DualHandedState()
        {
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
                print("To the another vine");
                animator.SetBool("DualHand", false);
                canReachVineCloser = false;
                StartCoroutine(WaitAndTransitState());
            }
            else if (/*Input.GetKey(KeyCode.LeftArrow)*/horizontalOnVine02 < 0 && FoundAnotherVine() && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand") && !canReach && canGetToAnotherVine)      //Get to another vine L
            {
                print("To the another vine");
                animator.SetBool("DualHand", false);
                canReachVineCloser = false;
                StartCoroutine(WaitAndTransitState());
            }

            print("canGetToAnotherVine " + canGetToAnotherVine);

            //Dual with no another vine on another side to  two handed to get down
            //Get down off the R side
            if (!FoundAnotherVine() && isOnVine && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))
            {
                print("Get off prepare");
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    animator.SetBool("DualHand", false);
                    print("Get Down R");
                    Flip();
                    Vector2 pos = transform.position;
                    pos.x -= GetDistanceToReachVineCloser();
                    transform.position = pos;
                    canReachVineCloser = false;
                    StartCoroutine(WaitAndTransitState());
                }
                  else if (horizontalOnVine02 > 0 && canGetOffVine && !canReach) //Get off and Fall from vine
                  {
                      print("R arrow");
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
                print("Get off prepare");
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    print("Get down L");
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
                     print("L arrow");
                     Vector2 pos = transform.position;
                     pos.x -= 0.2f;
                     transform.position = pos;
                     canGetOffVine = false;
                     StopAllCoroutines();
                 }
            }


        }

}