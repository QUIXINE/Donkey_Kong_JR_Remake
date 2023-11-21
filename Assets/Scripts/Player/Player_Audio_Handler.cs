using PlayerSpace;
using UnityEngine;

public class Player_Audio_Handler : MonoBehaviour 
{
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDying, isFalling, canLanding;

    private void Start() 
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update() 
    {
        HandleAudio();
    }

    private void HandleAudio()
    {
        if(player.Horizontal != 0 && player.enabled)
        {
            AudioPlayerTest.PlayAudio(AudioReferences.WalkSound);
        }

        if(player.Vertical != 0 && player.enabled)
        {
            AudioPlayerTest.PlayAudio(AudioReferences.ClimbSound);
        }

        if(player.HorizontalOnVine != 0 || player.HorizontalOnVine02 != 0)
        {
            AudioPlayerTest.PlayAudio(AudioReferences.ClimbSound);
        }

        if(Input.GetKeyDown(KeyCode.X) && player.IsGroundedChecker() && player.CurrentState == Player.PlayerState.Idle)
        {
            print("jumped");
            AudioPlayerTest.PlayAudio(AudioReferences.JumpSound);
        }

        //after jump, get on vine can play landing
        if(player.CurrentState != Player.PlayerState.Idle || rb.velocity.y <= -1f)
        {
            canLanding = true;
        }

        if(player.IsGroundedChecker() && canLanding && !player.IsOnVine)
        {
            AudioPlayerTest.PlayAudio(AudioReferences.LandingSound);
            canLanding = false;
        }

        #region Dying
            if(animator.GetBool("Die") && !isDying)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.DieSound);
                
                //will the game obj belongs to class, if this turns false will it get to it's first state after this
                //--> Yes, it will turn to its first state
                AudioReferences.BGSoundObj.SetActive(false);
                AudioReferences.AlarmSoundObj.SetActive(false);
                isDying = true;
            }
    
            if(rb.velocity.y <= -5f &&  rb.velocity.y >= -6f && !isFalling)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.Fall_ShortSound);
                isFalling = true;
            }
            else if(rb.velocity.y <= -7f && !isFalling)
            {
                AudioPlayerTest.PlayAudio(AudioReferences.FallSound);
                isFalling = true;
            }
        #endregion


    }
}