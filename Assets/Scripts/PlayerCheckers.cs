using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

sealed public partial class Player
{
    #region Checkers
    private bool IsOnVine()
    {
        //check if player jump from the sides of vine
        return Physics2D.Raycast(vineCheckPosBody.position, -vineCheckPosBody.right, vineCheckRadius, vineLayerMask);
    }
    private bool IsTwoHanded()
    {
        if (Physics2D.OverlapCircle(vineCheckPosDualHand02.position, radiusCheckOnHand, vineLayerMask))
        {
            return true;

        }
        return false;
    }

    private bool OnVineGravityChecker()
    {
        RaycastHit2D hitL;
        RaycastHit2D hitR;
        
        hitL = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, 1f, vineLayerMask);
        hitR = Physics2D.Raycast(vineCheckPosBody.position, transform.right, 1f, vineLayerMask);
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
    private bool CollideVineOnHead()
    {
        //check if player jump from under of vine
        return Physics2D.Raycast(vineCheckPosOnHead.position, vineCheckPosBody.up, vineCheckRadius, vineLayerMask);
    }
    private bool IsGroundedChecker()
    {
        //return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayerMask);
        return Physics2D.OverlapBox(groundCheckPos.position, new Vector2(0.8f, 0f), 0, groundLayerMask);
    }
    private bool FoundAnotherVine()
    {
        if (Physics2D.Raycast(vineCheckPosDualHand01.position, vineCheckPosDualHand01.forward, distanceCheckOnHand, vineLayerMask))
        {
            return true;
        }
       
        return false;
    }


    private float GetDistanceToReachVineCloser()
    {
        //Addition: Find raycast that can ignore DK jr hand so that I can put vineCheckPosDualHand01 on hand and when player is on the highest of vine he still can climb up/down
        RaycastHit2D hitReachL = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceGetCloseToVine,vineLayerMask);
        RaycastHit2D hitReachR = Physics2D.Raycast(vineCheckPosDualHand01.position, Vector2.right, rayDistanceGetCloseToVine, vineLayerMask);
        RaycastHit2D hitReachRWithNoVine = Physics2D.Raycast(vineCheckPosDualHand02.position, Vector2.right, rayDistanceGetCloseToVine, vineLayerMask);
       
        /*if (hitReachR || hitReachRWithNoVine)
        {
            print("Other checker words");
            if(hitReachR)
            print(hitReachR.collider.name);
            if(hitReachRWithNoVine)
            print(hitReachRWithNoVine.collider.name);
        }*/

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
                    return moveToL01;
                }
                if (distance > 0.6f)
                {
                    reachToLTriggered02 = true;
                    return moveToL02;
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
                    return moveToR03;
                }
                else if (distance < 0.27f)
                {
                    reachToRTriggered01 = true;
                    return moveToR01;
                }
            }
            else if (hitReachRWithNoVine && hitReachRWithNoVine.collider.gameObject.layer == 7)
            {
                float distance = hitReachRWithNoVine.collider.transform.position.x - vineCheckPosDualHand02.position.x;
                if (distance >= 0.25f && distance <= 0.3f)
                {
                    reachToRTriggered01 = true;
                    return moveToR01;
                }
                //move player to L because hand out to R
                else if (distance > 0.3f)
                {
                    reachToRTriggered02 = true;
                    return moveToR02; //move L
                }
            }
            
            
        }
        
        //move player back to two-handed-L with the same pos value as reach L using moveToL
        else if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0) && !animator.GetBool("DualHand"))
        {
            if(reachToLTriggered01)
            {
                reachToLTriggered01 = false;
                return moveToL01;
            }
            else if (reachToLTriggered02)
            {
                reachToLTriggered02 = false;
                return moveToL02;
            }
            return 0;
        }
        //move player back to two-handed-R with the same pos value as reach R using moveToR
        else if (transform.rotation == Quaternion.Euler(0, 0, 0) && !animator.GetBool("DualHand"))
        {
            if (reachToRTriggered01)
            {
                reachToRTriggered01 = false;
                return moveToR01;
            }
            else if (reachToRTriggered02)
            {
                reachToRTriggered02 = false;
                return moveToR02;
            }
            else if (reachToRTriggered03)
            {
                reachToRTriggered03 = false;
                return moveToR03;
            }
        }
        return 0;
    }

    private bool DistanceToReachVineCloserChecker()
    {
        RaycastHit2D hitInfo;
        if (currentState == PlayerState.DualHanded)
        {
            hitInfo = Physics2D.Raycast(vineCheckPosBody.position, vineCheckPosBody.right, 0.5f, vineLayerMask);
            float distance = hitInfo.distance;
            
            if (hitInfo && hitInfo.collider.gameObject.layer == 7)
            {
                if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (distance >= 0.2f && distance < 0.35f)
                    {
                        print(distance);
                        return true;
                    }
                }
                
                if (transform.rotation == Quaternion.Euler(0, -180, 0))
                {
                    if (distance >= 0.5f)
                    {
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
                    holdToRTriggered01 = true;
                    float move = -moveToR01Local + moveToL01;
                    return move;
                }
                else if (distance >= 0.4f)
                {
                    holdToRTriggered02 = true;
                    float move = -moveToR02Local + moveToL02; //-x
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
                    holdToRTriggered01 = true;
                    float move = moveToL01Local - moveToR01;
                    return move;
                }
                else if (distance >= 0.4f)
                {
                    holdToRTriggered02 = true;
                    float move = moveToL02Local - moveToR03;  //+x
                    return move;
                }
            }
            RaycastHit2D hitOnHeadL = Physics2D.Raycast(vineCheckPosOnHead.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);


            //move Two-handed R
            /*if (hitOnHeadR && hitOnHeadR.collider.gameObject.layer == 7)
            {
                float distance = vineCheckPosOnHead.position.x - hitOnHeadR.collider.transform.position.x;
                print(distance);
                if (distance >= 0.3f && distance < 0.4f)
                {
                    print("Move to point R");
                    holdToRTriggered01 = true;
                    return moveToR01;
                }
                else if (distance >= 0.4f)
                {
                    print("Move to point R");
                    holdToRTriggered02 = true;
                    return moveToR02;
                }
                else if (distance <= -0.3f && distance > -0.5f)
                {
                    print("Move to point L");
                    if (holdToRTriggered01)
                    {
                        print("holdToRTriggered01");
                        holdToRTriggered01 = false;
                        return moveToR01;
                    }
                    if (holdToRTriggered02)
                    {
                        print("holdToRTriggered02");
                        holdToRTriggered02 = false;
                        return moveToR02;
                    }
                }
                else if (distance <= -0.5)
                {
                    print("Move to point L");
                    if (holdToRTriggered01)
                    {
                        print("holdToRTriggered01");
                        holdToRTriggered01 = false;
                        return -moveToR01;
                    }
                    if (holdToRTriggered02)
                    {
                        print("holdToRTriggered02");
                        holdToRTriggered02 = false;
                        return -moveToR02;
                    }
                }
                else if (distance <= -0.7f && distance > -0.75f)
                {
                    print("Move to point L");
                    return 0.02f;
                }
                else if (distance <= -0.75f && distance > -0.85f)
                {
                    print("Move to point L");
                    return 0.005f;
                }
                else if (distance <= -0.85f)
                {
                    print("Move to point L");
                    return -0.005f;
                }

            }*/
        }
        
        return 0;
    }
    
    private float GetClose()
    {
        RaycastHit2D hitOnBody;
        if (currentState == PlayerState.TwoHanded)
        {
            if (transform.rotation == Quaternion.Euler(0, -180, 0) || transform.rotation == Quaternion.Euler(0, 180, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = hitOnBody.collider.transform.position.x - vineCheckPosBody.position.x;
                    if (distance >= 0.05)
                    {
                        return 0.05f;
                    }
                }
            }
            else if (transform.rotation == Quaternion.Euler(0, 0, 0)) //Move from Dual-Handed L to Two-handed R
            {
                hitOnBody = Physics2D.Raycast(vineCheckPosBody.position, -transform.right, rayDistanceGetCloseToVine, vineLayerMask);
                if (hitOnBody && hitOnBody.collider.gameObject.layer == 7)
                {
                    float distance = vineCheckPosBody.position.x - hitOnBody.collider.transform.position.x;
                    if (distance >= 0.05)
                    {
                        return 0.05f;
                    }
                }
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
    


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 4)  //Water
        {
            animator.SetBool("Die", true);
            rb.gravityScale = 0;
            horizontal = 0;
            rb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 1f);
        }
        
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)  //Vine
        {
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

    //Draw cube at raycast box posiotion
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(groundCheckPos.position, transform.localScale);
    }
    #endregion

    #region Caller of Checkers
    private void DistanceToReachVineCloser()
    {
        //Get Close to vine check while Two-Handed to solve hand is not on the vine
        if (transform.rotation == Quaternion.Euler(0, -180, 0) && isTwoHanded)    //Two-Handed L
        {
            Vector2 pos = transform.position;
            pos.x += GetClose();
            transform.position = pos;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0) && isTwoHanded)  //Two-Handed R
        {
            Vector2 pos = transform.position;
            pos.x -= GetClose();
            transform.position = pos;
        }
        else if (DistanceToReachVineCloserChecker() && checkDistanceToReachVineCloser && transform.rotation == Quaternion.Euler(0, 0, 0) && currentState == PlayerState.DualHanded)  //Two-Handed R
        {
            Vector2 pos = transform.position;
            pos.x -= 0.15f;
            transform.position = pos;
            checkDistanceToReachVineCloser = false;
        }
        else if (DistanceToReachVineCloserChecker() && checkDistanceToReachVineCloser && transform.rotation == Quaternion.Euler(0, -180, 0) && currentState == PlayerState.DualHanded)  //Two-Handed R
        {
            Vector2 pos = transform.position;
            pos.x += 0.15f;
            transform.position = pos;
            checkDistanceToReachVineCloser = false;
        }
    }

    private void IsGrounded()
    {
        if (IsGroundedChecker())
        {
            animator.SetBool("StopJump", true);
            animator.SetBool("Jump", false);
            animator.SetBool("DualHand", false);
            animator.SetBool("TwoHanded", false);
            rb.gravityScale = 1;
            vertical = 0;
            allowReachFirstGetOnVine = true;
            isOnVine = false;
            isTwoHanded = false;
        }
    }

    private void HandleAnimation()
    {
        if (horizontal > 0 || horizontal < 0) //if move, player will get out of the first state
            animator.SetTrigger("MoveOut");

        animator.SetFloat("Move", horizontal);
        animator.SetFloat("Climb", vertical);

        //help to correct the jump animation, but maybe problem in future because if not on ground
        //jump animation will always play
        if (!IsGroundedChecker())
        {
            animator.SetBool("Jump", true);
            animator.SetBool("StopJump", false);
        }

        if (IsOnVine() || CollideVineOnHead())
        {
            rb.gravityScale = 0;
            horizontal = 0;
            animator.SetBool("TwoHanded", true);
            animator.SetBool("Jump", false);
        }
    }

    private void OnVineGravityCheck()
    {
        if (!OnVineGravityChecker() && checkGravityVineExit)
        {
            rb.gravityScale = 20;
            checkGravityVineExit = false;
        }
    }

    #endregion
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