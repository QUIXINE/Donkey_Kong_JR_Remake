using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace
{
public class PlayerHealth : MonoBehaviour
{   
    //Singleton
    private static PlayerHealth instance;

    public static PlayerHealth Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerHealth();
            }
            return instance;
        }
    }
    
    //Health
    public List<Image> livesImg = new List<Image>();
    [SerializeField] private Sprite  lifeSprite;

    //When player health is decreased anywhere, lifeSpriteAmount also decreased at the same place in code
    public int lifeSpriteAmount;
    public int health;
    [SerializeField] private int healthShow;
    [SerializeField] private int lifeSpriteAmountShow;
    
    //Other class
    private Player player;
    private Canvas canvas;
    [SerializeField] private Camera mainCamera;

    private void Awake() 
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            print("Destrot");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
       
    }

    private void Start() 
    {
        
        Instance.health = 3;
        Instance.lifeSpriteAmount = Instance.health;
        canvas = GetComponent<Canvas>();
        /* for(int i = 0; i < Instance.lifeSpriteAmount; i++)
        {
            livesImg[i].sprite = lifeSprite;
        } */
        // StartCoroutine(ChangeLifeSpriteAmount());
    }

    private void Update() 
    {
        mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;

        //Show health, lifeSpriteAmount in inspector
        healthShow = Instance.health;
        lifeSpriteAmountShow = Instance.lifeSpriteAmount;

        //Remove img from list and set actiove player life img
        if(Instance.lifeSpriteAmount < livesImg.Count)
        {
            livesImg[Instance.lifeSpriteAmount].gameObject.SetActive(false);
            // livesImg.RemoveAt(Instance.lifeSpriteAmount);
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

    }


    //Enable player and change lifeSpriteAmount
    public IEnumerator ChangeLifeSpriteAmount()
    {
        yield return new WaitForSeconds(GameState.gameStartStateTime);
        Instance.lifeSpriteAmount = 2;
        
    }
}
}

/*show lifeSprite 
for(int i = 0; i < lifeSpriteAmount; i++)
{
    livesImg[i].sprite = lifeSprite;
}
*/