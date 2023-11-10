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
        Enemy_Moving[] enemies =   FindObjectsOfType<Enemy_Moving>(); 
        Fruit[] fruits  =   FindObjectsOfType<Fruit>(); 
        Player  player  =   FindObjectOfType<Player>();
        Mario mario     =   FindObjectOfType<Mario>();
        foreach(Enemy_Moving enemy in enemies)
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
        Bonus_Score_Manager bonus_Score_Manager = FindObjectOfType<Bonus_Score_Manager>();
        bonus_Score_Manager.StopBonusDecreasement();                       //Stop decreasing bonus when get to dying state (dying animation played)
        int currentScene =  SceneManager.GetActiveScene().buildIndex;

        //There's no need to use PlayerPrefs.GetInt(Player_Amount) as condition because "CurrentPlayer" only change when player dies
        if(PlayerPrefs.GetInt("Current_Player") == 1)
        {
            switch (currentScene)
            {
                default:
                    yield return new WaitForSeconds(10f);                           //How long to wait before changing scene (wait until all changing level animation finish)
                                                                                    //Wait to finish clear stage animation 10 sec
                    Score_Variables.ScorePlayer01 += Score_Variables.bonusScore;
                    SceneManager.LoadScene(currentScene + 1);
                    PlayerPrefs.SetInt("Player01_Scene", currentScene + 1);
                    break;
                
                case 6:
                    yield return new WaitForSeconds(16);                            //Wait animation after clear stage level04 to finish
                    Score_Variables.ScorePlayer01 += Score_Variables.bonusScore;
                    Score_Variables.lapAmountPlayer01++;
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
                    Score_Variables.ScorePlayer02 += Score_Variables.bonusScore;   
                    SceneManager.LoadScene(currentScene + 1);
                    PlayerPrefs.SetInt("Player02_Scene", currentScene + 1);
                    break;
                
                case 6:
                    yield return new WaitForSeconds(16);                            //Wait animation after clear stage level04 to finish
                    Score_Variables.ScorePlayer02 += Score_Variables.bonusScore;    
                    Score_Variables.lapAmountPlayer02++;
                    SceneManager.LoadScene(2);                                      //Load scene _Level01 index 2
                    PlayerPrefs.SetInt("Player02_Scene", 2);
                    break;
            }
        }
    }

    public static IEnumerator LoadSceneAfterDead()
    {
        Bonus_Score_Manager bonus_Score_Manager = FindObjectOfType<Bonus_Score_Manager>();
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
        Bonus_Score_Manager bonus_Score_Manager = FindObjectOfType<Bonus_Score_Manager>();
        bonus_Score_Manager.StopBonusDecreasement();                     //Stop decreasing bonus when get to dying state (dying animation played)
        yield return new WaitForSeconds(3f);
        //Load to score storing scene if player has score that is more than one of the 5 ranks
        PlayerPrefs.SetInt("Player01_Score", Score_Variables.ScorePlayer01);
        PlayerPrefs.SetInt("Player02_Score", Score_Variables.ScorePlayer02);
        
        //if(highScore > Top5) => SceneManager.LoadScene("Rank_Scene");   //maybe use LoadSceneMode.Additive so that the last scene still load and highscore still works then compare the score

        if(PlayerPrefs.GetInt("Player_Amount") == 1)
        {
            for(int i = 0; i < Load_And_Save_Score.player_list.Count; i++)
            {
                if(PlayerPrefs.GetInt("Player01_Score") > Load_And_Save_Score.player_list[i].Score)  
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
                if(Load_And_Save_Score.player_list.All(player => PlayerPrefs.GetInt("Player01_Score") < player.Score) && Load_And_Save_Score.player_list.Any(player => PlayerPrefs.GetInt("Player02_Score") > player.Score))
                {
                    print("player01 all less");
                    SceneManager.LoadScene("Rank_Scene");
                    PlayerPrefs.SetInt("CurrentPlayer", 2);
                    yield break;
                }
                else if(Load_And_Save_Score.player_list.All(player => PlayerPrefs.GetInt("Player01_Score") < player.Score) && Load_And_Save_Score.player_list.All(player => PlayerPrefs.GetInt("Player02_Score") < player.Score))
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

//Add bonus score to ScorePlayer01 (player finish in time gets bonus score)
//use lapAmount as a condition because i don't want bonusScore in Score to be static, if it is it'll be harder to coontrol
//Is there a problem like add over amount of boonus score at a time? - Yes
/* switch (Score.lapAmount)
        {
            case 1:
                bonusScore = 5000;
                break;
            case 2:
                bonusScore = 6000;
                break;
            case 3:
                bonusScore = 7000;
                break;
            default:
                bonusScore = 8000;
                break;
        } */
        

/* public static IEnumerator LoadSceneAfterWin()
{
print("change scene");
Score score = FindObjectOfType<Score>();
score.StopBonusIncrement();                     //Stop decreasing bonus when get to dying state (dying animation played)
yield return new WaitForSeconds(4f);            //How long to wait before changing scene (wait until all changing level animation finish)
int currentScene =  SceneManager.GetActiveScene().buildIndex;

switch(currentScene)
{
    default:
        Score.ScorePlayer01 += Score.bonusScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        break;
    /* case 1: //for testing
        Score.ScorePlayer01 += bonusScore;
        Score.lapAmount++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        break;
    case 4:
        Score.ScorePlayer01 += Score.bonusScore;
        Score.lapAmount++;
        SceneManager.LoadScene(1);
        break;
}   
*/