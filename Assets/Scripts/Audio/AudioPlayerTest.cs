using PlayerSpace;
using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayerTest : MonoBehaviour 
{
    private static AudioSource DefaultAudioPlayer;
    [SerializeField] private  AudioSource defaultAudioPlayer;
    private static AudioSource DieAudioPlayer;
    [SerializeField] private  AudioSource dieAudioPlayer;
    private static Player PlayerType;
    [SerializeField] private Player player;

    private static float playRate = 0.5f;     //can be able to play every 1 sec
    private static float nextPlayTime = 0;

    void Start()
    {
        Assign_Variables();
        playRate = 0.5f;
        nextPlayTime = 0;
    }

    private void Assign_Variables()
    {
        DefaultAudioPlayer  = defaultAudioPlayer;
        DieAudioPlayer      = dieAudioPlayer;
        PlayerType          =   player;
    }

    public static void PlayAudio(AudioClip audioClip)
    {
        
        if (audioClip == AudioReferences.WalkSound || audioClip == AudioReferences.ClimbSound || audioClip == AudioReferences.LandingSound)
        {
            if(Time.time > nextPlayTime)
            {
                //used to not let the sound play if input value (all horizontal and vertical) is not 0
                //used to solve if input value is not 0 and the sound (walk, climp, landing) still play while dying
                if(PlayerType.enabled == true)
                {
                    nextPlayTime = Time.time + playRate;
                    DefaultAudioPlayer.PlayOneShot(audioClip, 0.5f);
                }
            }
        }
        else if(audioClip == AudioReferences.DieSound || audioClip == AudioReferences.FallSound || audioClip == AudioReferences.Fall_ShortSound )
        {
            DieAudioPlayer.PlayOneShot(audioClip, 0.5f);    //there's stack of other walking sound because Player script is disabled and the horizontal or vertical value is at the last value 
            // DefaultAudioPlayer.Stop();   //if others sound but sounds in this condition loop play while dying, use this line
            if(Time.time > nextPlayTime)
            {
                nextPlayTime = Time.time;
            }
            
        }
        else
        {
            DefaultAudioPlayer.PlayOneShot(audioClip, 0.5f);
        }
       
    }

}