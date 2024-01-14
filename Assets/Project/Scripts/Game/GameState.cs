using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using PlayerSpace;
using ScoreManagement;


public class GameState : MonoBehaviour 
{
    public static GameObject GameOverUI;
    [SerializeField] private GameObject gameOverUI;
    public static float gameStartStateTime = 1f;
    private Player player;
    private static bool canChangeScene;

    
    private void Start()
    {
        GameOverUI = gameOverUI;
        canChangeScene = false;
        DisablePlayer();
    }

    private void DisablePlayer()
    {
        //Call after loadind scene 
        if (PlayerPrefs.GetInt("Current_Player") == 1)
        {
            if (Player01Health.Instance.LifeSpriteAmount >= 0)
            {
                //Not allow player to move
                //23.Player only stop first time the game start, after reloading scene player doesn't stop
                player = (Player)FindObjectOfType(typeof(Player));
                player.gameObject.SetActive(false);
                StartCoroutine(EnablePlayer());
            }
        }
        else
        {
            if (Player02Health.Instance.LifeSpriteAmount >= 0)
            {
                //Not allow player to move
                //23.Player only stop first time the game start, after reloading scene player doesn't stop
                player = (Player)FindObjectOfType(typeof(Player));
                player.gameObject.SetActive(false);
                StartCoroutine(EnablePlayer());
            }
        }
    }

    private void Update() 
    {
        if(canChangeScene)
        {
            StartCoroutine(LoadSceneAfterGameOver());
            canChangeScene = false;
        }
    }


    private IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(gameStartStateTime);
        player.gameObject.SetActive(true);
        StopCoroutine(EnablePlayer());
    }


    public static IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4f);
        //inactivate Mario, fruits, enemies, DK jr.
        Fruit[] fruits  =   FindObjectsOfType<Fruit>(); 
        Player  player  =   FindObjectOfType<Player>();
        MarioController mario     =   FindObjectOfType<MarioController>();

        //Destroy enemies in scene
        EnemyScore[] enemies =   FindObjectsOfType<EnemyScore>(); 
        foreach(EnemyScore enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        foreach(Fruit fruit in fruits)
        {
            fruit.gameObject.SetActive(false);
        }
        mario.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        GameOverUI.SetActive(true);
        canChangeScene = true;
    }


    #region Scene Loading
    public static IEnumerator LoadSceneAfterWin()
    {
        BonusScoreManager bonus_Score_Manager = FindObjectOfType<BonusScoreManager>();
        bonus_Score_Manager.StopBonusDecreasement();                       //Stop decreasing bonus when get to dying state (dying animation played)
        int currentScene =  SceneManager.GetActiveScene().buildIndex;

        //Destroy enemies in scene
        MarioController mario_Controller = FindObjectOfType<MarioController>();
        if(mario_Controller != null)
        {
            mario_Controller.enabled = false;
        }
        EnemyScore[] enemyArray = FindObjectsOfType<EnemyScore>();
        foreach(EnemyScore enemy in enemyArray)
        {
            Destroy(enemy.gameObject);
        }
        //There's no need to use PlayerPrefs.GetInt(Player_Amount) as condition because "CurrentPlayer" only change when player dies
        if(PlayerPrefs.GetInt("Current_Player") == 1)
        {
            switch (currentScene)
            {
                default:
                    yield return new WaitForSeconds(10f);                           //How long to wait before changing scene (wait until all changing level animation finish)
                                                                                    //Wait to finish clear stage animation 10 sec
                    ScoreVariables.ScorePlayer01 += ScoreVariables.bonusScore;
                    SceneManager.LoadScene(currentScene + 1);
                    PlayerPrefs.SetInt("Player01_Scene", currentScene + 1);
                    break;
                
                case 6:
                    yield return new WaitForSeconds(16);                            //Wait animation after clear stage level04 to finish
                    ScoreVariables.ScorePlayer01 += ScoreVariables.bonusScore;
                    ScoreVariables.lapAmountPlayer01++;
                    SceneManager.LoadScene(2);                                      //Load scene _Level01 index 2
                    PlayerPrefs.SetInt("Player01_Scene", 2);
                    break;
            }
        }
        else
        {
            switch (currentScene)
            {
                default:
                    yield return new WaitForSeconds(10f);                           //How long to wait before changing scene (wait until all changing level animation finish)
                                                                                    //Wait to finish clear stage animation 10 sec
                    ScoreVariables.ScorePlayer02 += ScoreVariables.bonusScore;   
                    SceneManager.LoadScene(currentScene + 1);
                    PlayerPrefs.SetInt("Player02_Scene", currentScene + 1);
                    break;
                
                case 6:
                    yield return new WaitForSeconds(16);                            //Wait animation after clear stage level04 to finish
                    ScoreVariables.ScorePlayer02 += ScoreVariables.bonusScore;    
                    ScoreVariables.lapAmountPlayer02++;
                    SceneManager.LoadScene(2);                                      //Load scene _Level01 index 2
                    PlayerPrefs.SetInt("Player02_Scene", 2);
                    break;
            }
        }
    }

    public static IEnumerator LoadSceneAfterDead()
    {
        BonusScoreManager bonus_Score_Manager = FindObjectOfType<BonusScoreManager>();
        bonus_Score_Manager.StopBonusDecreasement();                     //Stop decreasing bonus when get to dying state (dying animation played)
        yield return new WaitForSeconds(4f);

        if(PlayerPrefs.GetInt("Player_Amount") == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                if(Player02Health.Instance.Health <= 0 && Player02Health.Instance.gameObject.activeSelf == false)
                {
                    Player02Health.Instance.gameObject.SetActive(false);
                    PlayerPrefs.SetInt("Current_Player", 1);
                    SceneManager.LoadScene(PlayerPrefs.GetInt("Player01_Scene"));
                    yield break;
                }
                Player01Health.Instance.gameObject.SetActive(false);
                PlayerPrefs.SetInt("Current_Player", 2);
                SceneManager.LoadScene(PlayerPrefs.GetInt("Player02_Scene"));
                Player02Health.Instance.gameObject.SetActive(true);
            }
            else 
            {
                if(Player01Health.Instance.Health <= 0 && Player01Health.Instance.gameObject.activeSelf == false)
                {
                    Player01Health.Instance.gameObject.SetActive(false);
                    PlayerPrefs.SetInt("Current_Player", 2);
                    SceneManager.LoadScene(PlayerPrefs.GetInt("Player02_Scene"));
                    yield break;
                }

                Player02Health.Instance.gameObject.SetActive(false);
                PlayerPrefs.SetInt("Current_Player", 1);
                SceneManager.LoadScene(PlayerPrefs.GetInt("Player01_Scene"));
                Player01Health.Instance.gameObject.SetActive(true);

            }
        }

    }
    
    private IEnumerator LoadSceneAfterGameOver()
    {
        BonusScoreManager bonus_Score_Manager = FindObjectOfType<BonusScoreManager>();
        bonus_Score_Manager.StopBonusDecreasement();                     //Stop decreasing bonus when get to dying state (dying animation played)
        yield return new WaitForSeconds(3f);
        //Load to score storing scene if player has score that is more than one of the 5 ranks
        PlayerPrefs.SetInt("Player01_Score", ScoreVariables.ScorePlayer01);
        PlayerPrefs.SetInt("Player02_Score", ScoreVariables.ScorePlayer02);
        
        //if(highScore > Top5) => SceneManager.LoadScene("Rank_Scene");   //maybe use LoadSceneMode.Additive so that the last scene still load and highscore still works then compare the score

        if(PlayerPrefs.GetInt("Player_Amount") == 1)
        {
            for(int i = 0; i < LoadAndSaveScore.player_list.Count; i++)
            {
                if(PlayerPrefs.GetInt("Player01_Score") > LoadAndSaveScore.player_list[i].Score)  
                {
                    SceneManager.LoadScene("Rank_Scene");  
                    yield break;
                }
            }
            //if(highScore < Top5) => SceneManager.LoadScene("Menu_Scene");
            SceneManager.LoadScene("Menu_Scene");  
        }
        else if(PlayerPrefs.GetInt("Player_Amount") == 2)
        {
            //use player health to set condition if player 1/2 dies another player can still play the game
            if(Player01Health.Instance.Health <= 0 && Player02Health.Instance.Health <= 0)
            {
                if(LoadAndSaveScore.player_list.All(player => PlayerPrefs.GetInt("Player01_Score") < player.Score) && LoadAndSaveScore.player_list.Any(player => PlayerPrefs.GetInt("Player02_Score") > player.Score))
                {
                    SceneManager.LoadScene("Rank_Scene");
                    PlayerPrefs.SetInt("CurrentPlayer", 2);
                    yield break;
                }
                else if(LoadAndSaveScore.player_list.All(player => PlayerPrefs.GetInt("Player01_Score") < player.Score) && LoadAndSaveScore.player_list.All(player => PlayerPrefs.GetInt("Player02_Score") < player.Score))
                {
                    //if(highScore < Top5) => SceneManager.LoadScene("Menu_Scene");
                    SceneManager.LoadScene("Menu_Scene");  
                    yield break;
                }
                else
                {
                    PlayerPrefs.SetInt("Current_Player", 1);
                    SceneManager.LoadScene("Rank_Scene");
                    yield break;
                }
            }
            else if (Player01Health.Instance.Health > 0 && Player02Health.Instance.Health <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Player01_Scene"));
                PlayerPrefs.SetInt("Current_Player", 1);
                Player02Health.Instance.gameObject.SetActive(false);
                Player01Health.Instance.gameObject.SetActive(true);
            }
            else if (Player01Health.Instance.Health <= 0 && Player02Health.Instance.Health > 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Player02_Scene"));
                PlayerPrefs.SetInt("Current_Player", 2);
                Player01Health.Instance.gameObject.SetActive(false);
                Player02Health.Instance.gameObject.SetActive(true);
            }
        }


       
    }
    #endregion
}

