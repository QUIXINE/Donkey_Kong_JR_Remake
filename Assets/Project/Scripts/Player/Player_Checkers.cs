using UnityEngine;
using UnityEngine.SceneManagement;


namespace PlayerSpace
{
sealed public partial class Player
{
    #region Checkers

    private bool IsOnVineChecker()
    {
        //check if player jump from the sides of vine to get on vine
        //Solved 16 - use IsOnPlatformCloseVine() for checking if player is on platform that is close to the vine, if so then change ray distance to shorter so that player won't get on vine from too far  
        RaycastHit2D hitOnBodyL = Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, IsGroundedChecker()? 0.2f : rayDistanceToCheckIsOnVine, vineLayerMask);
        //used when DK hands are off because immediately flip to the wrong side after jump from the platform, or while two handed
        RaycastHit2D hitOnBodyR = Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, rayDistanceToCheckIsOnVine, vineLayerMask);                    
        //RaycastHit2D hitOnHead  = Physics2D.Raycast(vineCheckPosOnHead02.position, -vineCheckPosOnHead02.right, 0.5f, vineLayerMask);           //maybe used to check when body is off but hands are still on the vine, to be able to move
        RaycastHit2D hitOnHand  = Physics2D.Raycast(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right, rayDistanceGetCloseToVine, vineLayerMask);
        //RaycastHit2D hitOnHand02  = Physics2D.Raycast(vineCheckPosDualHand01.position, -vineCheckPosDualHand02.right, 1.5f, vineLayerMask);   //used because DK's hands are off while DualHanded

        if (hitOnBodyL && hitOnBodyL.collider.gameObject.layer == 7)
        {
            // print("hitOnBodyL");
            return true;
        }
        //used when DK hands are off because immediately flip to the wrong side after jump from the platform, or while two handed
        else if(!hitOnBodyL && hitOnBodyR && hitOnBodyR.collider.gameObject.layer == 7 && CurrentState == PlayerState.TwoHanded)
        {
            // print("hitOnBodyR");
            return true;
        }
        //Problem 34: if there's On head Playuer won't be able to reach out animation but the state is changed to DualHanded as coded
        //maybe used to check when body is off but hands are still on the vine, to be able to move
       /*  else if(hitOnHead && !hitOnBodyL && !hitOnBodyR && hitOnHead.collider.gameObject.layer == 7 && CurrentState == PlayerState.TwoHanded)
        {
            // print("hitOnHead TwoHanded");
            return true;
        } */
        else if(hitOnHand && hitOnHand.collider.gameObject.layer == 7 && CurrentState == PlayerState.DualHanded) //used to get input horizontalOnVine02 so that player can get back to and fall from vine
        {
            // print("hitOnHand");
            return true;
        }
       /*  else if(hitOnHand02 && !hitOnHand && hitOnHand02.collider.gameObject.layer == 7 && CurrentState == PlayerState.DualHanded) 
        {
            // print("hitOnHand");
            return true;
        } */
        /* //Problem 34: if there's On head Playuer won't be able to reach out animation but the state is changed to DualHanded as coded
        else if(hitOnHead && !hitOnBodyL && !hitOnBodyR && !hitOnHand &&hitOnHead.collider.gameObject.layer == 7 && CurrentState == PlayerState.DualHanded)
        {
            // print("hitOnHead DualHanded");
            return true;
        } */
        return false;
    }

    private bool FoundAnotherVine()
    {
        //print("FoundAnotherVine Check");
        return Physics2D.Raycast(vineCheckPosDualHand01.position, vineCheckPosDualHand01.forward, rayDistanceOnHand, vineLayerMask);
    }

    //use to check if FoundAnotherVine() but another hand is off the vine, to fix receiving inout of vertical
    private bool IsHandOnMainVine()
    {
        return Physics2D.Raycast(vineCheckPosDualHand02.position, vineCheckPosDualHand02.forward, rayDistanceOnHand, vineLayerMask);
    }

    private bool OnVineGravityChecker()
    {
        RaycastHit2D hitL;
        RaycastHit2D hitR;
        
        hitL = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, 0.6f, vineLayerMask);
        hitR = Physics2D.Raycast(vineCheckPosBody.position, transform.right, 0.6f, vineLayerMask);
        if (hitL && hitL.collider.gameObject.layer == 7)
        {
            return true;
        }
        else if (hitR && hitR.collider.gameObject.layer == 7)
        {
            return true;
        }
        return false;
    }
    
    public bool IsGroundedChecker()
    {
        return Physics2D.OverlapBox(groundCheckPos01.position, new Vector2(xLengthGroundCheck, yLengthGroundCheck), 0, groundLayerMask);
    }
    
    //For what?
    private bool IsThereGroundInRange()
    {
        if(Physics2D.Raycast(groundCheckPos01.position, -groundCheckPos01.up, 1f, groundLayerMask))
        {
            return true;
        }
        else if(Physics2D.Raycast(groundCheckPos02.position, -groundCheckPos02.up, 1f, groundLayerMask))
        {
            return true;
        }
        return false;
    }
    private bool IsThereWaterInRange()
    {
        if(Physics2D.Raycast(groundCheckPos01.position, -groundCheckPos01.up, 0.1f, waterLayerMask))      //layer 4 = water
        {
            return true;
        }
        return false;
    }
    private bool IsThereGroundAsObstacle()
    {
        Collider2D collideGround = Physics2D.OverlapBox(obstacleCheckPos01.position , new Vector2(xLengthObstacleCheck, yLengthObstacleCheck), 0, groundLayerMask);
        if(collideGround)
        {
            return true;
        }
        return false;
    }

    private bool IsThereBoundaryAsObstacle()
    {
        Collider2D collideBoundaryOnFront = Physics2D.OverlapBox(obstacleCheckPos01.position , new Vector2(xLengthObstacleCheck, yLengthObstacleCheck), 0, boundaryLayerMask);
        //used w/ _Level04 while on vine and there is boundary on another side, and is TwoHanded
        if (collideBoundaryOnFront && SceneManager.GetActiveScene().buildIndex == 6)
        {
            return true;
        }
        return false;
    }

    private bool IsThereKeyAsObstacle()
    {
        Collider2D collideKeyOnBack = Physics2D.OverlapBox(obstacleCheckPos02.position , new Vector2(xLengthObstacleCheck, yLengthObstacleCheck), 0, itemLayerMask);
        //used w/ _Level04 while on vine and there is a key behind, prevent player to get to that side
        if (collideKeyOnBack && collideKeyOnBack.gameObject.CompareTag("Key") && SceneManager.GetActiveScene().buildIndex == 6)
        {
            print("Key is there");
            return true;
        }
        return false;
    }

    private void FlipBackFromObstacle()
    {
        if(CurrentState == PlayerState.TwoHanded && animator.GetBool("TwoHanded") && !IsGroundedChecker() && !CollideGroundOnHead())
        {
            //used to check on the back if flip and colldie w/ ground
            if (Physics2D.OverlapBox(obstacleCheckPos02.position , new Vector2(xLengthObstacleCheck, yLengthObstacleCheck), 0, groundLayerMask) && IsOnVine)
            {
                Flip();
            }
            //used w/ _Level04
            else if (Physics2D.OverlapBox(obstacleCheckPos02.position , new Vector2(xLengthObstacleCheck, yLengthObstacleCheck), 0, boundaryLayerMask) && IsOnVine && SceneManager.GetActiveScene().buildIndex == 6)
            {
                Flip();
            }
        }
        
    }

    private bool CollideVineOnHead()
    {
        //check if player jump from under of vine
        return Physics2D.OverlapBox(vineCheckPosOnHead.position, new Vector3(xLengthCheckOnHead,0,0), 0,vineLayerMask);
    }

    private bool CollideGroundOnHead()  //Used to check ground on head which associate w/ FlipBackFromObstacle() to prevent automaticaaly flipping
    {
        //check if player jump from under of vine
        return Physics2D.OverlapBox(groundCheckPosOnHead.position, new Vector3(0.6f,0.05f,0), 0,groundLayerMask);
    }
    #endregion
    
    #region Checkers to move player closer to the vine
    //Used to provide float for reaching the vine closer, used with DualHanded state
    //used w/ DualHanded State
    private float GetDistanceToReachVineCloser()
    {
        //Addition: Find raycast that can ignore DK jr hand so that I can put vineCheckPosDualHand01 on hand and when player is on the highest of vine he still can climb up/down
        RaycastHit2D hitReachL = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceToReachVineCloser,vineLayerMask);
        RaycastHit2D hitReachR = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceToReachVineCloser, vineLayerMask);
        RaycastHit2D hitReachRWithNoVine = Physics2D.Raycast(vineCheckPosDualHand02.position, Vector2.right, rayDistanceToReachVineCloser, vineLayerMask);
        //Here
        //float distanceL = hitReachL.collider.transform.position.x - vineCheckPosDualHand01.position.x;
        //layer 7 = vine
        //move player to L side
        //(0,0,0) is L side (because flip from Two-handed-L to reach L in Reach L code)
        if (transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand"))     //Two-Handed L to Reach L
        {
            if (hitReachL && hitReachL.collider.gameObject.layer == 7)     
            {

                float distance = hitReachL.collider.transform.position.x - vineCheckPosDualHand01.position.x;
                if (distance > 0.5f && distance <= 0.6)
                {

                    reachToLTriggered01 = true;
                    return reachToL01;
                }
                else if (distance > 0.6f)
                {

                    reachToLTriggered02 = true;
                    return reachToL02;
                }
                else if (distance < 0 && distance >= -0.08f)
                {

                    reachToLTriggered01 = true;
                    return reachToL01;
                }
                else if (distance > 0.001f && distance <= 0.2f)
                {

                    reachToLTriggered01 = true;
                    return reachToL01;
                }
            }
        }
        //move player to R side
        //(0,-180,0) is R side (because flip in reach-R), same reason as move player to L side
        else if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0) && animator.GetBool("DualHand"))  //Two-Handed R to Reach R
        {
            if (hitReachR && hitReachR.collider.gameObject.layer == 7 )  
            {

                float distance = hitReachR.collider.transform.position.x - vineCheckPosDualHand01.position.x;
                if (distance >= 0.27f)
                {

                    reachToRTriggered03 = true;
                    return reachToR03;
                }
                else if (distance < 0.27f)
                {
                    reachToRTriggered01 = true;
                    return reachToR01;
                }
            }
            else if (hitReachRWithNoVine && hitReachRWithNoVine.collider.gameObject.layer == 7)
            {
                float distance = hitReachRWithNoVine.collider.transform.position.x - vineCheckPosDualHand02.position.x;
                if (distance >= 0.25f && distance <= 0.3f)
                {
                    reachToRTriggered01 = true;
                    return reachToR01;
                }
                //move player to L because hand out to R
                else if (distance > 0.3f)
                {
                    reachToRTriggered02 = true;
                    return reachToR02; //move L
                }
            }
            
            
        }
        
        //move player back to two-handed-L with the same pos value as reach L using moveToL
        else if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0) && !animator.GetBool("DualHand"))
        {
            if(reachToLTriggered01)
            {
                reachToLTriggered01 = false;
                return reachToL01;
            }
            else if (reachToLTriggered02)
            {
                reachToLTriggered02 = false;
                return reachToL02;
            }
            return 0;
        }
        //move player back to two-handed-R with the same pos value as reach R using moveToR
        else if (transform.rotation == Quaternion.Euler(0, 0, 0) && !animator.GetBool("DualHand"))
        {
            if (reachToRTriggered01)
            {
                reachToRTriggered01 = false;
                return reachToR01;
            }
            else if (reachToRTriggered02)
            {
                reachToRTriggered02 = false;
                return reachToR02;
            }
            else if (reachToRTriggered03)
            {
                reachToRTriggered03 = false;
                return reachToR03;
            }
        }
        return 0;
    }


    //Used to check while player is DualHanded, check if the ditance between player and the vine is not valid(DK hands are off the vine)
    //Used w/ ReachVineCloser() which is called in Update()
    private float GetDistanceToAlwaysReachVineCloser()
    {
        //change direction from  vineCheckPosBody.right to  -vineCheckPosBody.right
        RaycastHit2D hitInfo;
        if (CurrentState == PlayerState.DualHanded)
        {
            hitInfo = Physics2D.Raycast(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right, 1f, vineLayerMask);
            float distance02 = hitInfo.distance;
            //print($"distance : {distance02}");
            //Used when there's no any vine on another side
            if (!FoundAnotherVine() && hitInfo && hitInfo.collider.gameObject.layer == 7)
            {
                //print("!Found");
                if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (distance02 >= 0f && distance02 < 0.7f)
                    {
                        if(distance02 <= 0.001)
                        {
                            //canReachVineCloser = false;
                            return 0;
                        }
                        return 0.02f;
                    }
                }
                
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    if (distance02 >= 0 && distance02 < 0.7f)
                    {
                        if(distance02 <= 0.001)
                        {
                            //canReachVineCloser = false;
                            return 0;
                        }
                        return 0.02f;
                    }
                }
            }
            //Used when there's a vine on another side
            else if (FoundAnotherVine() && hitInfo && hitInfo.collider.gameObject.layer == 7)
            {
                //print("Found");

                if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (distance02 >= 0f && distance02 < 0.7f)
                    {
                        if(distance02 <= 0.001)
                        {
                            //canReachVineCloser = false;
                            return 0;
                        }
                        return 0.01f;
                    }
                }
                
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    if (distance02 >= 0 && distance02 < 0.7f)
                    {
                        if(distance02 <= 0.001)
                        {
                            //canReachVineCloser = false;
                            return 0;
                        }
                        return 0.01f;
                    }
                }
            }
        }
        return 0;
    }


    private float GetDistanceToHoldVineCloser()
    {
        //print("check");//
        //33.Player-Movement: Player doesn't get on the vine when the vine is ahead of PosBody while still in range of ability to get on vine 

        RaycastHit2D hitOnHead;
        RaycastHit2D hitOnBodyL;

        //Why used hitOnBodyR --> at first there's only hitOnBodyL, but there's a problem. Problem: Player hands are off with the wrong side sprite
        //I solved the problem - check the Solved 14. in Problems.md 
        RaycastHit2D hitOnBodyR;
        if (CurrentState == PlayerState.TwoHanded)
        {
            
            //Two-Handed L
            if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBodyL  =   Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, rayDistanceToHoldVineCloser, vineLayerMask);
                if (hitOnBodyL && hitOnBodyL.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBodyL.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.06)
                    {
                        // print("hitOnBodyL 180 degree");
                        return 0.05f;
                    }
                }

                //used when vineCheckPosBody is lower than the vine while DK hands are at the height that can be on the vine
                hitOnHead   =   Physics2D.Raycast(vineCheckPosOnHead02.position, -vineCheckPosOnHead02.right, rayDistanceOnHead, vineLayerMask);
                if (hitOnHead && !hitOnBodyL && hitOnHead.collider.gameObject.layer == 7)
                {
                    float distance = hitOnHead.collider.transform.position.x - vineCheckPosOnHead02.position.x;
                    if (distance >= 0.06)
                    {
                        print("hitOnHead 180 degree L");
                        return 0.05f;
                    }
                } 

                //used when DK hands are off because immediately flip to the wrong side after jump from the platform, or while two handed
                hitOnBodyR  =   Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, rayDistanceToHoldVineCloser, vineLayerMask);
                if (hitOnBodyR/*  && !hitOnHead */ &&  !hitOnBodyL && hitOnBodyR.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosBody.position.x - hitOnBodyR.collider.transform.position.x;
                    if (distance >= 0.06)
                    {
                        // print($"hitOnHead -180 degree distance: {distance}");
                        return -0.05f;
                    }
                }


                //used when DK hands are off which hitOnHead w/ -vineCheckPosOnHead02.right can't detect the vine
                //maybe need to put in //Two-Handed R as well
                hitOnHead   =   Physics2D.Raycast(vineCheckPosOnHead02.position, vineCheckPosOnHead02.right, rayDistanceOnHead, vineLayerMask);
                if (hitOnHead && !hitOnBodyL && !hitOnBodyR && hitOnHead.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosOnHead02.position.x - hitOnHead.collider.transform.position.x;
                    if (distance >= 0.06)
                    {
                        print("hitOnHead 180 degree R");
                        return -0.05f;
                    }
                } 
                
            }
            //Two-Handed R
            else if (transform.rotation == Quaternion.Euler(0, 0, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBodyL  =   Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, rayDistanceToHoldVineCloser, vineLayerMask);
                if (hitOnBodyL && hitOnBodyL.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosBody.position.x - hitOnBodyL.collider.transform.position.x;
                    if (distance >= 0.06)
                    {
                        // print("hitOnBodyL 0 degree");
                        return 0.05f;
                    }
                }

                //used when vineCheckPosBody is lower than the vine while DK hands are at the height that can be on the vine
                hitOnHead   =   Physics2D.Raycast(vineCheckPosOnHead02.position, -vineCheckPosOnHead02.right, rayDistanceOnHead, vineLayerMask);
                if (hitOnHead && !hitOnBodyL && hitOnHead.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosOnHead02.position.x - hitOnHead.collider.transform.position.x;
                    if (distance >= 0.06)
                    {
                        // print("hitOnHead 0 degree");
                        return 0.05f;
                    }
                }
                //used when DK hands are off because immediately flip to the wrong side after jump from the platform
                hitOnBodyR  =   Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, rayDistanceToHoldVineCloser, vineLayerMask);
                if (hitOnBodyR /* && !hitOnHead */ && !hitOnBodyL && hitOnBodyR.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBodyR.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.06)
                    {
                        // print("hitOnBodyR 0 degree");
                        return -0.05f;
                    }
                }

                
            }
        }
     
        return 0;
    }
    #endregion

    #region Caller of Checkers

    //Change IsOnVine() hitDualhand02 rayDis, GetClose() 2Handed rayDis to 5f
    private void ReachVineCloser()
    {
        //Problem: Player always move close to the vine
        //19.get off the vine, get from one shorter to longer and don't get off

        //Get Close to vine check while Two-Handed and Dual-Handed to solve hand is not on the vine
        if (CurrentState == PlayerState.TwoHanded && transform.rotation == Quaternion.Euler(0, -180, 0) && isTwoHanded && IsOnVineChecker())    //Two-Handed L
        {
            Vector2 pos = transform.position;
            pos.x += GetDistanceToHoldVineCloser();
            transform.position = pos;
        }
        else if (CurrentState == PlayerState.TwoHanded && transform.rotation == Quaternion.Euler(0, 0, 0) && isTwoHanded && IsOnVineChecker())  //Two-Handed R
        {
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToHoldVineCloser();
            transform.position = pos;
        }
        else if (CurrentState == PlayerState.DualHanded/*  && DistanceToReachVineCloserChecker() */ /* && canReachVineCloser */ && transform.rotation == Quaternion.Euler(0, 0, 0) && animator.GetBool("DualHand")) //Dual-Handed L
        {
            print("back01");
            Vector2 pos = transform.position;
            pos.x -= GetDistanceToAlwaysReachVineCloser();    //0.156
            transform.position = pos;
            // canReachVineCloser = false;
        }
        else if (CurrentState == PlayerState.DualHanded/*  && DistanceToReachVineCloserChecker() */ /* && canReachVineCloser */ && transform.rotation == Quaternion.Euler(0, -180, 0) && animator.GetBool("DualHand"))  //Dual-Handed R
        {
            print("back02");
            Vector2 pos = transform.position;
            pos.x += GetDistanceToAlwaysReachVineCloser();    //0.056
            transform.position = pos; 
            // canReachVineCloser = false;
        }
    }

    private void IsGrounded()
    {
        if (IsGroundedChecker())
        {
            animator.SetBool("Jump", false);
            animator.SetBool("StopJump", true);
            rb.gravityScale = 1;
            Vertical = 0;
            canReachFirstGetOnVine = true;
            
            //!IsOnVineChecker() --> benefit when on vine and collide w/ platform, player will be able to move
            if(!IsOnVineChecker())
            {
                animator.SetBool("IsGrounded", true);
                animator.SetBool("DualHand", false);
                animator.SetBool("TwoHanded", false);
                IsOnVine = false;
                isTwoHanded = false;
                CurrentState = PlayerState.Idle;
                xLengthCheckOnHead = 0.6f;  //set 0.1f so that player can get to the vine collide w/ head
            }
            
            IsJumped = false;
            rayDistanceGetCloseToVine = 0.1f;
        }
        if(IsGroundedChecker() && rb.velocity.y <= 0)   //What is this for? --> to stop get point from enemy when is back to the ground
        {
            canGetPointFromEnemy = false;
            enemyList.Clear();
        }
    }

    //Q: What is this method for?
    private void IfOnVine()
    {
        if(IsOnVine && !FoundAnotherVine())
            rayDistanceGetCloseToVine = 0.6f; //What is this for?
        if(IsOnVine)
        {
            xLengthCheckOnHead = 0; //set 0 so that when getting down the ray won't interupt
        }
        else
        {
            canFallFromVine = false;    //used because after get down with DualHanded straight to the ground, it won't
                                        //get into change WaitToChangeState() on PlayerStates script which means canFallFromVine will be true when get to the ground
                                        //and get to the TwoHanded w/ true canFallFromVine which makes player can fall from vine while TwoHanded which is not right,
                                        //so this line of code is used if player is not on vine then set canFallFromVine to false
        }
    }
    
    private void HandleCollider()
    {
        if(IsThereGroundInRange() || IsOnVine)
        {
            dkCol.enabled = true;
        }
        else if (IsThereWaterInRange() && !IsThereGroundInRange())  //used this instead of IsThereGroundOrWaterInRange() which is its old version to fix when it should fall into water 
                                                                    //but collider of the ground and the water collides each other which makes player won't fall into water and jolting, fro example, the first platform in level 1 
        {
            dkCol.enabled = true;
            dkCol.isTrigger = true;
        }
        else if(!IsThereGroundInRange())
        {
            //maybe add groundCheckPos03 and check IsThereGroundOrWaterInRange() w/ groundCheckPos03 not groundCheckPos01 because DK's sprite still on the ground and fall too soon 
            if(!IsJumped)   //if jump and is closer to the edge of ground collider, player 
            {
                Horizontal = 0;
            }
            dkCol.enabled = false;
        }
    }

    private void HandleGravity()
    {
        //Set gravity Fall from vine after "Fall from vine" condition in DualHandedState() works
        //why rb.gravityScale = 2;   --> to fall fast 
        if (!OnVineGravityChecker() && checkGravityVineExit)    //this condition relates with DualHandedState()
        {
            rb.gravityScale = 2;
            IsOnVine = false;
            checkGravityVineExit = false;
        }

        //used this for setting the gravity if player press L/R arrow again after already is DualHanded (reach out)
        //What the reason? because after press L/R arrow again after reach out gravity is 0, this makes player won't fall off the vine
        //Setting condition as below --> I used this condition only for falling off the vine while isOnVine = true and not on ground
        //if isOnVine is false, MovePosGetOnVine() will work which means player state will be TwoHanded and there will be an unplesant situatuion occur
        else if(CurrentState == PlayerState.Idle && !IsGroundedChecker() && IsOnVine)   //this condition relates with DualHandedState()       
        {
            rb.gravityScale = 2;
        }
    }
    #endregion

  
    #region Collision
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 4)  //Water, why can't set w/ waterLayerMask
        {
            rb.gravityScale = 0;
            Horizontal = 0;
            rb.velocity = new Vector2(0, 0);
            collideWithWater = true;

            Vector3 shortenScale = new Vector3(0.6f, 0.6f, 0.6f);
            waterSplashParticle.transform.localScale = shortenScale;
            Instantiate(waterSplashParticle, vineCheckPosBody.position, vineCheckPosBody.rotation);  //call water splash VFX
            
            animator.SetBool("DieOtherCondition", true);
            playerTakeDamage.TakeDamage();
        }

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)  //Vine, why can't set w/ vineLayerMask
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Jump", true);
            animator.SetBool("TwoHanded", false);
            animator.SetBool("StopJump", false);
            checkGravityVineExit = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //stomp Jump pad
        if (col.gameObject.CompareTag("JumpPad"))
        {
            Vector3 anglePoint = new Vector2(facingRight ? 1 : -1, 1).normalized;
            //set jump animation like this because while on jump and not on ground player will always play jump animation
            //but it's maybe a problem in future, yep --> player won't always play jump animation while not on ground player

            Animator jumpPadAnimator = col.gameObject.GetComponent<Animator>();
            jumpPadAnimator.SetBool("Stomped", true);
            animator.SetBool("StopJump", true);
            animator.SetBool("Jump", false);
            //Vector2 value depends on height of moving platform in scene 2
            rb.AddForce(new Vector2(facingRight ? 2 : -2, 6) * (Input.GetKey(KeyCode.X) && facingRight? jumpPadForceSpecial : jumpPadForceDefault));    //if hold X will push higher
            /*//rb.AddForce(((Vector2.up * jumpPadForce)) * Time.deltaTime , ForceMode2D.Impulse) ;
            //rb.AddForce((anglePoint * jumpPadForce) * Time.deltaTime, ForceMode2D.Impulse);
            //rb.AddForce(((anglePoint * jumpPadForce)) * Time.deltaTime , ForceMode2D.Impulse);
            */
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        //get off Jump pad
        if (col.gameObject.CompareTag("JumpPad"))
        {
            animator.SetBool("Jump", true);
            Animator jumpPadAnimator = col.gameObject.GetComponent<Animator>();
            jumpPadAnimator.SetBool("Stomped", false);

        }
    }
    #endregion

    private void HandleAnimation()
    {
        if (Horizontal > 0 || Horizontal < 0) //if move, player will get out of the first state
            animator.SetTrigger("MoveOut");

        animator.SetFloat("Move", Horizontal);
        animator.SetFloat("Climb", Vertical);

        //help to correct the jump animation, but maybe problem in future because if not on ground
        //jump animation will always play
        if (!IsGroundedChecker() && !OnVineGravityChecker() && !IsOnVine)
        {
            animator.SetBool("TwoHanded", false);
            animator.SetBool("StopJump", false);
            Horizontal = 0;
        }

        if (IsOnVineChecker() || CollideVineOnHead())
        {
            rb.gravityScale = 0;
            Horizontal = 0;
            if(CurrentState == PlayerState.TwoHanded)
            {
                animator.SetBool("TwoHanded", true);
            }
            animator.SetBool("Jump", false);
        }

          
    }

    //Draw cube at raycast box posiotion
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)

        Gizmos.DrawWireCube(groundCheckPos01.position, new Vector3(xLengthGroundCheck, yLengthGroundCheck, 0));
        //Gizmos.DrawRay(groundCheckPos01.position, -groundCheckPos01.up *  2f);

        //Obstacle Check
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(obstacleCheckPos01.position, new Vector3(xLengthObstacleCheck, yLengthObstacleCheck, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(obstacleCheckPos02.position, new Vector3(xLengthObstacleCheck, yLengthObstacleCheck, 0));

        
        //Enemy check to get score
        //Gizmos.DrawWireSphere(groundCheckPos01.position, CircleRadius);
        Gizmos.DrawWireCube(groundCheckPos01.position, new Vector3(sizeX, sizeY, 0f));
        //Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);

        //Drak on Head
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(vineCheckPosOnHead.position, new Vector3(xLengthCheckOnHead, 0, 0));
        Gizmos.DrawWireCube(groundCheckPosOnHead.position, new Vector3(xLengthCheckOnHead, 0.05f, 0));
        //Gizmos.DrawRay(vineCheckPosOnHead.position, -vineCheckPosOnHead.right * rayDistanceOnHead);

        //Draw to detect the vine to reach DualHanded closer
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawRay(vineCheckPosDualHand01.position, -vineCheckPosDualHand01.right * rayDistanceToReachCloserOnBody);
        //Gizmos.DrawRay(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right * 0.4f);
        
        //Draw on Body
        //Gizmos.DrawRay(vineCheckPosBody.position, vineCheckPosBody.right * rayDistanceOnBody);
        //Gizmos.DrawRay(groundCheckPos.position, -groundCheckPos.up * 2f);
    }
}
}
/*I test this code with jump pad

private void IsPushed()
{
    if (isPushed)
    {
        print(isPushed);
        Vector3 anglePoint = new Vector2(facingRight ? 1 : -1, 1).normalized;
        //Vector3 angle = anglePoint.normalized;
        rb.AddForce(((new Vector2(facingRight ? 1 : -1, 6)) * jumpPadForce) * Time.deltaTime);
        isPushed = false;
    }
}
private IEnumerator JumpPadPush(bool isPushed)
    {
        yield return new WaitForSeconds(0.25f);
        isPushed = true;
        StopCoroutine(JumpPadPush(isPushed));


    }
            
*/
/* private float GetDistanceToHoldVineCloser()
    {
        RaycastHit2D hitOnBody;

        float moveToR01Local = 0.45f;
        float moveToR02Local = 0.55f;
        
        float moveToL01Local = 0.45f;
        float moveToL02Local = 0.55f;

        if (transform.rotation == Quaternion.Euler(0, 0, 0)) //Move from Dual-Handed L to Two-handed R
        {
            hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);
            if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
            {
                float distance = vineCheckPosBody.position.x - hitOnBody.collider.transform.position.x;
                if (distance >= 0.2f && distance < 0.4f)
                {
                    float move = -moveToR01Local + reachToL01;
                    return move;
                }
                else if (distance >= 0.4f)
                {
                    float move = -moveToR02Local + reachToL02; //-x, plus reachToL02 because Dual-Handed L minus postion with it
                    return move;
                }
            }
        }
        else if (transform.rotation == Quaternion.Euler(0, -180, 0)) //Move from Dual-Handed R to Two-handed L
        {
            hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, transform.right, rayDistanceGetCloseToVine, vineLayerMask);
            if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
            {
                float distance = vineCheckPosBody.position.x - hitOnBody.collider.transform.position.x;
                if (distance >= 0.2f && distance < 0.4f)
                {
                    float move = moveToL01Local - reachToR01;
                    return move;
                }
                else if (distance >= 0.4f)
                {
                    float move = moveToL02Local - reachToR03;  //+x, minus reachToR03 because Dual-Handed R plus postion with it
                    return move;
                }
            }
        }
        
        return 0;
    }
     */

/*GetDistanceToAlwaysReachVineCloser ver. 01
private bool DistanceToReachVineCloserChecker()
{
    //change direction from  vineCheckPosBody.right to  -vineCheckPosBody.right
    RaycastHit2D hitInfo;
    RaycastHit2D hitDual02;
    if (CurrentState == PlayerState.DualHanded)
    {
        hitInfo = Physics2D.Raycast(vineCheckPosDualHand01.position, -vineCheckPosDualHand01.right, rayDistanceToReachCloserOnBody, vineLayerMask);
        hitDual02 = Physics2D.Raycast(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right, rayDistanceToReachCloserOnBody, vineLayerMask);
        float distance = hitInfo.distance;
        float distance02 = hitDual02.distance;
        if (hitInfo && hitInfo.collider.gameObject.layer == 7)
        {
            if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (distance >= 0.01f && distance < 0.7f)
                {
                    print("hitDual01 Check 0 degree");
                    print("hitDual01 Check 0 distance " + distance);
                    return true;
                }
                
            }
            
            if (transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (distance >= 0.01f && distance < 0.7f)
                {
                    print("hitDual01 Check -180 degree");
                    print("hitDual01 Check -180 distance " + distance);
                    return true;
                }
            }
        }

        if (hitDual02 && hitDual02.collider.gameObject.layer == 7)
        {
            if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (distance02 >= 0.1f && distance02 < 0.7f)
                {
                    print("hitDual01 Check 0 degree");
                    print("hitDual02 Check 0 distance " + distance02);
                    return true;
                }
            }
            
            if (transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (distance02 >= 0.1f && distance02 < 0.7f)
                {
                    print("hitDual01 Check -180 degree");
                    print("hitDual02 Check -180 distance " + distance02);
                    return true;
                }
            }
        }
    
    }

    
    return false;
}
*/
/*GetDistanceToAlwaysReachVineCloser ver. 02
private float GetDistanceToAlwaysReachVineCloser()
{
    //change direction from  vineCheckPosBody.right to  -vineCheckPosBody.right
    RaycastHit2D hitDual01;
    RaycastHit2D hitDual02;
    if (CurrentState == PlayerState.DualHanded)
    {
        hitDual01 = Physics2D.Raycast(vineCheckPosDualHand01.position, -vineCheckPosDualHand01.right, rayDistanceToReachCloserOnBody, vineLayerMask);
        hitDual02 = Physics2D.Raycast(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right, 0.5f, vineLayerMask);
        float distance = hitDual01.distance;
        float distance02 = hitDual02.distance;
        /*  if(!FoundAnotherVine())
        {
            print("set ray to 0.2f");
            rayDistanceToReachCloserOnBody = 0f;
        }

        if (hitDual01 && hitDual01.collider.gameObject.layer == 7)
        {

            if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (distance >= 0 && distance < 0.7f)
                {
                    if(distance <= 0)
                    {
                        print($"hitInfo 0 degree distance: {distance}");
                        canReachVineCloser = false;
                        return 0;
                    }
                    return 0.15f;
                }
                
            }
            
            if (transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (distance >= 0f && distance < 0.7f)
                {
                        if(distance <= 0f)
                    {
                        print($"hitInfo -180 degree distance: {distance}");
                        canReachVineCloser = false;
                        return 0;
                    }
                    return 0.15f;
                }
            }
        }
        else if(!hitDual01 && !hitDual02)
        {
            canChangeToReach = false;
        }

        //Used when there's no any vine on another side
        if (!FoundAnotherVine() && !hitDual01 && hitDual02 && hitDual02.collider.gameObject.layer == 7)
        {
            if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                if (distance02 >= 0f && distance02 < 0.7f)
                {
                    if(distance02 <= 0.05)
                    {
                        print("hitDual02 0 degree");
                        canReachVineCloser = false;
                        return 0;
                    }
                    return 0.05f;
                }
            }
            
            if (transform.rotation == Quaternion.Euler(0, -180, 0))
            {
                if (distance02 >= 0 && distance02 < 0.7f)
                {
                    if(distance02 <= 0.05)
                    {
                        print("hitDual02 -180 degree");
                        canReachVineCloser = false;
                        return 0;
                    }
                    return 0.05f;
                }
            }
        }
    
    }
    
    return 0;
} 
*/

 //Move player pos after get on vine the first time so that the hands won't go off the vine
    /* private float GetDistanceToGetOnVine()
    {
        //Used hand instead of body
        RaycastHit2D hitInfo = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetOnVine, vineLayerMask);
        if(hitInfo)
        {
            print("GetDistanceToGetOnVine() hit");
        }
        
        if (transform.rotation == Quaternion.Euler(0, -180, 0))     //Get on from L side
        {
            if (hitInfo.distance >= 0.07f && hitInfo.distance < 0.09f)
            {
                return 0;
            }
            else if (hitInfo.distance >= 0.09f)
            {
                return 0.1f;
            }
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0))   //Get on from R side
        {
            if (hitInfo.distance >= 0.07f && hitInfo.distance < 0.09f)
            {
                return 0.01f;
            }
            else if (hitInfo.distance >= 0.09f)
            {
                return 0.1f;
            }
        }
        
        return 0;
    }
 */
    //overwhelmed by ReachVineCloser()