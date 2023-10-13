using UnityEngine;
using System.Collections;


namespace PlayerSpace
{
sealed public partial class Player
{
    #region Checkers
    private bool IsOnVineChecker()
    {
        //check if player jump from the sides of vine
        RaycastHit2D hitOnBodyL = Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, rayDistanceOnBody, vineLayerMask);
        RaycastHit2D hitOnBodyR = Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, 0.4f, vineLayerMask);
        RaycastHit2D hitOnHand = Physics2D.Raycast(vineCheckPosDualHand02.position, -vineCheckPosDualHand02.right, rayDistanceGetCloseToVine, vineLayerMask);
        //Here
        if (hitOnBodyL && hitOnBodyL.collider.gameObject.layer == 7)
        {
            //print("hitOnBodyL Check");
            return true;
        }
        else if(!hitOnBodyL && hitOnBodyR && hitOnBodyR.collider.gameObject.layer == 7 && currentState == PlayerState.TwoHanded)
        {
            //print("hitOnBodyR Check");

            return true;
        }
        if (currentState == PlayerState.DualHanded && hitOnHand && hitOnHand.collider.gameObject.layer == 7)
        {
            //print("IsOnVine2 Check");
            return true;
        }
        return false;
    }
    private bool FoundAnotherVine()
    {
        if (Physics2D.Raycast(vineCheckPosDualHand01.position, vineCheckPosDualHand01.forward, rayDistanceOnHand, vineLayerMask))
        {
            //print("FoundAnotherVine Check");
            return true;
        }
       
        return false;
    }
    private bool IsTwoHanded()
    {
        //Here DualHanded02
        if (Physics2D.OverlapCircle(vineCheckPosDualHand02.position, radiusCheckOnHand, vineLayerMask))
        {
            //print("Check");
            return true;

        }
        return false;
    }

    private bool OnVineGravityChecker()
    {
        RaycastHit2D hitL;
        RaycastHit2D hitR;
        
        hitL = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, 0.6f, vineLayerMask);
        hitR = Physics2D.Raycast(vineCheckPosBody.position, transform.right, 0.6f, vineLayerMask);
        if (hitL && hitL.collider.gameObject.layer == 7)
        {
            //print("Check");//
            return true;
        }
        else if (hitR && hitR.collider.gameObject.layer == 7)
        {
            //print("Check");
            return true;
        }
        return false;
    }
    
    public bool IsGroundedChecker()
    {
        //return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayerMask);
        return Physics2D.OverlapBox(groundCheckPos.position, new Vector3(0.8f, 0f, 0f), 0, groundLayerMask);
    }
    private bool CollideVineOnHead()
    {
        //check if player jump from under of vine
        return Physics2D.OverlapBox(vineCheckPosOnHead.position, new Vector3(0.1f,0,0), 0,vineLayerMask);
    }
    #endregion
    
    #region Checkers to move player closer to the vine
    private float GetDistanceToReachVineCloser()
    {
        //Addition: Find raycast that can ignore DK jr hand so that I can put vineCheckPosDualHand01 on hand and when player is on the highest of vine he still can climb up/down
        RaycastHit2D hitReachL = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceGetCloseToVine,vineLayerMask);
        RaycastHit2D hitReachR = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceGetCloseToVine, vineLayerMask);
        RaycastHit2D hitReachRWithNoVine = Physics2D.Raycast(vineCheckPosDualHand02.position, Vector2.right, rayDistanceGetCloseToVine, vineLayerMask);
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

    private bool DistanceToReachVineCloserChecker()
    {
        RaycastHit2D hitInfo;
        if (currentState == PlayerState.DualHanded)
        {
            hitInfo = Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, rayDistanceGetCloseToVine, vineLayerMask);
            float distance = hitInfo.distance;
            
            if (hitInfo && hitInfo.collider.gameObject.layer == 7)
            {
                if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (distance >= 0.2f && distance < 0.4f)
                    {
                        //print(distance);
                        return true;
                    }
                    
                }
                
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    if (distance >= 0.5f)
                    {
                        //print(distance);
                        return true;
                    }
                }
            }
        }
        return false;
    }


    //Move player pos after get on vine so that the hands won't go off the vine
    private float GetDistanceToGetOnVineCloser()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);
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

    private float GetDistanceToHoldVineCloser()
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
    
    private float GetClose()
    {
        //print("check");//

        RaycastHit2D hitOnBody;
        if (currentState == PlayerState.TwoHanded)
        {
                //Two-Handed L
            if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, rayDistanceOnBody, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBody.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.06)
                    {
                        print("check");
                        return 0.05f;
                    }
                }
            }
                //Two-Handed R
            else if (transform.rotation == Quaternion.Euler(0, 0, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, rayDistanceOnBody, vineLayerMask);
                //hitOnBodyR = Physics2D.Raycast(vineCheckPosBody.position, transform.right, 1f, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosBody.position.x - hitOnBody.collider.transform.position.x;
                    if (distance >= 0.06)
                    {
                        print("check");
                        return 0.05f;
                    }
                }
                /*if (hitOnBodyR && hitOnBodyR.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBodyR.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.05)
                    {
                        return 0.05f;
                    }
                }*/
            }
        }
        else if (currentState == PlayerState.DualHanded)
        {
            if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0)) //Move from  Two-handed R to Dual-Handed R 
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBody.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.2)
                    {
                        return 0.05f;
                    }
                }
            }
            else if (transform.rotation == Quaternion.Euler(0, 0, 0)) //Move from Two-handed L to Dual-Handed L
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, transform.right, rayDistanceGetCloseToVine, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBody.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.2)
                    {
                        return 0.05f;
                    }
                }
            }
        }

        return 0;
    }
    #endregion

    #region Caller of Checkers

    //Change IsOnVine() hitDualhand02 rayDis, GetClose() 2Handede rayDis to 5f
    private void ReachVineCloser()
    {
        //print("check");//
        //Problem: Player always move close to the vine
        //19.get off the vine, get from one shorter to longer and don't get off


        //Get Close to vine check while Two-Handed and Dual-Handed to solve hand is not on the vine
        if (transform.rotation == Quaternion.Euler(0, -180, 0) && isTwoHanded && currentState == PlayerState.TwoHanded && IsOnVineChecker())    //Two-Handed L
        {
            print("back03");

            Vector2 pos = transform.position;
            pos.x += GetClose();
            transform.position = pos;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0) && isTwoHanded && currentState == PlayerState.TwoHanded && IsOnVineChecker())  //Two-Handed R
        {
            print("back04");

            Vector2 pos = transform.position;
            pos.x -= GetClose();
            transform.position = pos;
        }
        else if (DistanceToReachVineCloserChecker() && canReachVineCloser && transform.rotation == Quaternion.Euler(0, 0, 0) && currentState == PlayerState.DualHanded && animator.GetBool("DualHand")) //Dual-Handed L
        {
            print("back01");
            Vector2 pos = transform.position;
            pos.x -= 0.05f;
            transform.position = pos;
            canReachVineCloser = false;
        }
        else if (DistanceToReachVineCloserChecker() && canReachVineCloser && transform.rotation == Quaternion.Euler(0, -180, 0) && currentState == PlayerState.DualHanded && animator.GetBool("DualHand"))  //Dual-Handed R
        {
            print("back02");
            Vector2 pos = transform.position;
            pos.x -= 0.05f;
            transform.position = pos;
            canReachVineCloser = false;
        }
    }

    private void IsGrounded()
    {
        if (IsGroundedChecker())
        {
            animator.SetBool("Jump", false);
            animator.SetBool("DualHand", false);
            animator.SetBool("TwoHanded", false);
            animator.SetBool("StopJump", true);
            animator.SetBool("IsGrounded", true);
            rb.gravityScale = 1;
            vertical = 0;
            canReachFirstGetOnVine = true;
            isOnVine = false;
            isTwoHanded = false;
            rayDistanceGetCloseToVine = 0.1f;
            currentState = PlayerState.Idle;
        }
        if(IsGroundedChecker() && rb.velocity.y <= 0)
        {
            canGetPointFromEnemy = false;
            enemyList.Clear();
        }
    }

    //Q: What is this method for?
    private void IsOnVine()
    {
        if(isOnVine)
        rayDistanceGetCloseToVine = 0.6f;
    }

    private void OnVineGravityCheck()
    {
        if (!OnVineGravityChecker() && checkGravityVineExit)
        {
            rb.gravityScale = 2;
            isOnVine = false;
            checkGravityVineExit = false;
        }
    }
    
    #endregion

  
    #region Collision
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 4)  //Water
        {
            rb.gravityScale = 0;
            horizontal = 0;
            rb.velocity = new Vector2(0, 0);
            collideWithWater = true;
            playerTakeDamage.TakeDamage();
        }

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)  //Vine
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
            rb.AddForce(((new Vector2(facingRight ? 1 : -1, 6)) * jumpPadForce) * Time.deltaTime);
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
        if (horizontal > 0 || horizontal < 0) //if move, player will get out of the first state
            animator.SetTrigger("MoveOut");

        animator.SetFloat("Move", horizontal);
        animator.SetFloat("Climb", vertical);

        //help to correct the jump animation, but maybe problem in future because if not on ground
        //jump animation will always play
        if (!IsGroundedChecker() && !OnVineGravityChecker() && !isOnVine)
        {
            animator.SetBool("TwoHanded", false);
            animator.SetBool("StopJump", false);
            horizontal = 0;
        }

        if (IsOnVineChecker() || CollideVineOnHead())
        {
            rb.gravityScale = 0;
            horizontal = 0;
            animator.SetBool("TwoHanded", true);
            animator.SetBool("Jump", false);
        }
    }

    //Draw cube at raycast box posiotion
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawCube(groundCheckPos.position, new Vector3(0.8f, 0, 0));
        // Gizmos.DrawWireCube(groundCheckPos.position, new Vector3(1f, 1f, 0));
        Gizmos.DrawWireSphere(groundCheckPos.position, 0.7f);
        //Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(vineCheckPosOnHead.position, new Vector3(0.1f, 0, 0));

        Gizmos.DrawRay(vineCheckPosBody.position, vineCheckPosBody.right * 0.3f);
        Gizmos.DrawRay(groundCheckPos.position, -groundCheckPos.up * 2f);
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