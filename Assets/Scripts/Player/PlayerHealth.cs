using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
public class PlayerHealth : SingletonTest<PlayerHealth>
{   
    
    /* 
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
    } */
    
    //Health
    // public List<Image> livesImg = new List<Image>();
    //[SerializeField] private Sprite  lifeSprite;


    //UI
    Player_Health_UI player_Health_UI;

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
        /* //Singleton
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            print("Destrot");
            Destroy(this.gameObject);
        } */
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        Instance.health = 3;
        Instance.lifeSpriteAmount = Instance.health;
        
        // canvas = GetComponent<Canvas>();
        /* for(int i = 0; i < Instance.lifeSpriteAmount; i++)
        {
            livesImg[i].sprite = lifeSprite;
        } */
        StartCoroutine(ChangeLifeSpriteAmount());
    }

    private void Update() 
    {
        //Destroy when reach menu scene
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject newObject = new GameObject("Player Health");
            transform.parent = newObject.transform;
            Destroy(newObject);
        }

        //Find type that has array of health UI
        player_Health_UI = FindObjectOfType<Player_Health_UI>();

        //Show health, lifeSpriteAmount in inspector
        healthShow = Instance.health;
        lifeSpriteAmountShow = Instance.lifeSpriteAmount;

        

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
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Menu_Scene");
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