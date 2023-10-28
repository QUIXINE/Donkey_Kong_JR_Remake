using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using PlayerSpace;
using ScoreManagement;
using Unity.VisualScripting;


public class GameState : MonoBehaviour 
{
    public static GameObject GameOverUI;
    [SerializeField] private GameObject gameOverUI;
    public static float gameStartStateTime = 1f;
    private Player player;
    private static bool canChangeScene;

    
    private void Start() 
    {
       /*  PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if(playerHealth != null)
        {
            print("enable playerhealth");
            playerHealth.gameObject.SetActive(true);
            if(PlayerHealth.Instance.lifeSpriteAmount == 0)
            StartCoroutine(PlayerHealth.Instance.ChangeLifeSpriteAmount());
        } */
        GameOverUI = gameOverUI;
        canChangeScene = false;
        //Call after loadind scene 
        if(PlayerHealth.Instance.lifeSpriteAmount >= 0)
        {
            //Not allow player to move
            //23.Player only stop first time the game start, after reloading scene player doesn't stop
            player = (Player)FindObjectOfType(typeof(Player));
            player.gameObject.SetActive(false);
            StartCoroutine(EnablePlayer());
        }
        
    }

    private void Update() 
    {
        if(canChangeScene)
        {
            StartCoroutine(LoadAfterGameOver());
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

    public static IEnumerator LoadSceneAfterWin()
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
                break; */
            case 4:
                Score.ScorePlayer01 += Score.bonusScore;
                Score.lapAmount++;
                SceneManager.LoadScene(1);
                break;
        }
    }

    public static IEnumerator LoadSceneAfterDead()
    {
        Score score = FindObjectOfType<Score>();
        score.StopBonusIncrement();                     //Stop decreasing bonus when get to dying state (dying animation played)
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        /*if there's black scene the below line will wait 3 sec.
        yield return new WaitForSeconds(3f);
        //put black scene here
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        */
    }
    
    private IEnumerator LoadAfterGameOver()
    {
        Score score = FindObjectOfType<Score>();
        score.StopBonusIncrement();                     //Stop decreasing bonus when get to dying state (dying animation played)
        yield return new WaitForSeconds(3f);
        //Load to score storing scene if player has score that is more than one of the 5 ranks
        PlayerPrefs.SetInt("Player01_Score", Score.ScorePlayer01);
        
        //if(highScore > Top5) => SceneManager.LoadScene("Rank_Scene");   //maybe use LoadSceneMode.Additive so that the last scene still load and highscore still works then compare the score
        for(int i = 0; i < /* Score_Singleton.player_list.Count */Load_And_Save_Json.player_list.Count; i++)
        {
            if(PlayerPrefs.GetInt("Player01_Score") > Load_And_Save_Json.player_list[i].Score)  
            {
                SceneManager.LoadScene("Rank_Scene");  
                yield break;
            }
        }

        //if(highScore < Top5) => SceneManager.LoadScene("Menu_Scene");
        SceneManager.LoadScene("Menu_Scene");  
    }
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
        