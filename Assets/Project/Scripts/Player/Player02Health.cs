using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScoreManagement;

namespace PlayerSpace
{
    public class Player02Health : Singleton<Player02Health>
    {   
        
        //When player health is decreased anywhere, lifeSpriteAmount also decreased at the same place in code
        public int LifeSpriteAmount;
        public int Health;
        [SerializeField] private int healthShow;
        [SerializeField] private int lifeSpriteAmountShow;
        public bool canIncrease = true;
        public bool canDestroyOther = true;

        private void Awake() 
        {
            DontDestroyOnLoad(this);
        }

        private void Start() 
        {
            Instance.Health = 3; //used for testing
            //Instance.Health = 3;
            Instance.LifeSpriteAmount = Instance.Health;
            
            StartCoroutine(ChangeLifeSpriteAmount());
        }

        private void Update()
        {
            //Show health, lifeSpriteAmount in inspector
            healthShow = Instance.Health;
            lifeSpriteAmountShow = Instance.LifeSpriteAmount;

            IncreaseHealth();
            DestroySelf();
        }

        //Destroy when reach rank scene
        private void DestroySelf()
        {
            if (SceneManager.GetActiveScene().buildIndex == 7)  //Rank scene index 7
            {
                if (Player01Health.Instance != null)
                {
                    if(!Player01Health.Instance.gameObject.activeInHierarchy)
                    Player01Health.Instance.gameObject.SetActive(true);     //used to destroy Player01Health beacause it will be hidden if not show it
                }
                GameObject newObject = new GameObject("Player Health");
                transform.parent = newObject.transform;
                Destroy(newObject);
            }
        }

        //Increase health if score reach 10000
        private void IncreaseHealth()
        {

            if(PlayerPrefs.GetInt("Current_Player") == 2 && canIncrease)
            {
                if(Score_Variables.ScorePlayer02 >= 10000)
                {
                    Health++;
                    if(LifeSpriteAmount < 3)
                    {
                        LifeSpriteAmount++;
                    }
                    canIncrease = false;
                }
            }
        }

        //Enable player and change lifeSpriteAmount
        public IEnumerator ChangeLifeSpriteAmount()
        {
            yield return new WaitForSeconds(GameState.gameStartStateTime);
            Instance.LifeSpriteAmount = 2;
            
        }
    }
}
