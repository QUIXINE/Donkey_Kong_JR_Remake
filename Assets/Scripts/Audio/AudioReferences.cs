using UnityEngine;

public class AudioReferences : MonoBehaviour 
{
    //Player
    public static AudioClip WalkSound, ClimbSound, JumpSound, FallSound, Fall_ShortSound, LandingSound, DieSound;   
    [SerializeField] private AudioClip walkSound, climbSound, jumpSound, fallSound, fall_ShortSound, landingSound, dieSound;   
    
    //Enemy
    public static AudioClip BiteSound;
    [SerializeField] private AudioClip biteSound;

    //Fruit
    public static AudioClip EatSound;
    [SerializeField] private AudioClip eatSound;

    //Score
    public static AudioClip FruitBumpSound, EnemyClearSound, UnlockSound;
    [SerializeField] private AudioClip fruitBumpSound, enemyClearSound, unlockSound;
    public static GameObject AlarmSoundObj;
    [SerializeField] private GameObject alarmSoundObj;

    //Level Scene
    public static GameObject BGSoundObj;
    [SerializeField] private GameObject bgSoundObj;

    //Menu Scene
    public static AudioClip CoinInsertSound;
    [SerializeField] private AudioClip coinInsertSound;

    private void Start()
    {
        AssignSounds();
    }

    private void AssignSounds()
    {
        //Player
        WalkSound   = walkSound;
        ClimbSound  = climbSound;
        JumpSound   = jumpSound;
        FallSound   = fallSound;
        Fall_ShortSound     =   fall_ShortSound;
        LandingSound        = landingSound;
        DieSound    = dieSound;

        //Enemy
        BiteSound   = biteSound;

        //Fruit
        EatSound    =   eatSound;

        //Score
        AlarmSoundObj      =   alarmSoundObj;
        FruitBumpSound  =   fruitBumpSound;
        EnemyClearSound =   enemyClearSound;
        UnlockSound     =   unlockSound;
        
        //Level Scene
        BGSoundObj     =   bgSoundObj; 

        //Menu Scene
        CoinInsertSound =   coinInsertSound;

    }
}