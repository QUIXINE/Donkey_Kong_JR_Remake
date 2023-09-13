using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    /*private void OnCollisionEnter2D(Collision2D col)
    {
        Vector3 anglePoint = new Vector2(facingRight ? 1 : -1, 1).normalized;
        print(anglePoint);
        //Vector3 angle = anglePoint.normalized;
        if (col.gameObject.CompareTag("JumpPad"))
        {
            //set jump animation like this because while on jump and not on ground player will always play jump animation
            //but it's maybe a problem in future
            animator.SetBool("Jump", true);
            animator.SetBool("StopJump", false);
            //rb.AddForce(((Vector2.up * jumpPadForce)) * Time.deltaTime , ForceMode2D.Impulse) ;
            //rb.AddForce((anglePoint * jumpPadForce) * Time.deltaTime, ForceMode2D.Impulse);
            //rb.AddForce(((anglePoint * jumpPadForce)) * Time.deltaTime , ForceMode2D.Impulse);
            rb.AddForce(((new Vector2(facingRight ? 1 : -1, 6)) * jumpPadForce) * Time.deltaTime);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("JumpPad"))
        {
            animator.SetBool("Jump", true);
        }
    }*/
}
