using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{   
    //Health
    public static PlayerHealth instance;
    public List<Image> livesImg = new List<Image>();
    [SerializeField] private Sprite  lifeSprite;

    //When player health is decreased anywhere, lifeSpriteAmount also decreased at the same place in code
    public static int lifeSpriteAmount;
    public static int health;
    [SerializeField] private int healthShow;
    [SerializeField] private int lifeSpriteAmountShow;
    
    //Other class
    private Player player;
    private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    static PlayerHealth()
    {
        health = 3;
        lifeSpriteAmount = health;
    }

    private void Awake() 
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
       
    }

    private void Start() 
    {
        health = 3;
        lifeSpriteAmount = health;
        canvas = GetComponent<Canvas>();
        for(int i = 0; i < lifeSpriteAmount; i++)
        {
            livesImg[i].sprite = lifeSprite;
        }
        /*Call after loadind scene 
        if(lifeSpriteAmount >= 0)
        {
            //Not allow player to move
            //23.Player only stop first time the game start, after reloading scene player doesn't stop
            player = (Player)FindObjectOfType(typeof(Player));
            player.enabled = false;
        } */
        StartCoroutine(ChangeLifeSpriteAmount());
    }

    private void Update() 
    {
        mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;

        //Show health, lifeSpriteAmount in inspector
        healthShow = health;
        lifeSpriteAmountShow = lifeSpriteAmount;

        //put player in Update() because it will be missing after reload the scene
        player = (Player)FindObjectOfType(typeof(Player));
        
        
        //Remove img from list and set actiove player life img
        if(lifeSpriteAmount < livesImg.Count)
        {
            livesImg[lifeSpriteAmount].gameObject.SetActive(false);
            livesImg.RemoveAt(lifeSpriteAmount);
        }

        /* //Test
        if(Input.GetKeyDown(KeyCode.E) && lifeSpriteAmount <= 2)
        {

            /* if(health > 0 && health > 1)
            {
                health--;
                lifeSpriteAmount--;
                StartCoroutine(LoadSceneAfterDead());
            }
            else if(health == 1)
            {
                health--;
                StartCoroutine(LoadSceneAfterDead());
            } 
        }


        if(health <= 0)
        {
            animator.SetBool("Die", true);
        }
        */

        DontDestroyOnLoad(this);
    }


    //Enable player and change lifeSpriteAmount
    private IEnumerator ChangeLifeSpriteAmount()
    {
        yield return new WaitForSeconds(3f);
        lifeSpriteAmount = 2;
        player.enabled = true;
        
        StopCoroutine(ChangeLifeSpriteAmount());
    }
}


/*show lifeSprite 
for(int i = 0; i < lifeSpriteAmount; i++)
{
    livesImg[i].sprite = lifeSprite;
}
*/