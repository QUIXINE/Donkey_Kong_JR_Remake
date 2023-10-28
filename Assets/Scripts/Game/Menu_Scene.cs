using Microsoft.Unity.VisualStudio.Editor;
using PlayerSpace;
using ScoreManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Scene : MonoBehaviour 
{
    //Used for testing 
    [Tooltip("used only for testing")]
    [SerializeField] private int HighScoreOfPlayerPrefs;    //used to change PlayerPrefs("HighScore") value to test faster 
    [Tooltip("used only for testing")]
    [SerializeField] private int Player01ScoreOfPlayerPrefs;    //used to change PlayerPrefs("HighScore") value to test faster 
    
    
    private void Awake() 
    {
        Score.lapAmount = 1;
        PlayerPrefs.GetInt("Player01_Score");
    }
    private void Start() 
    {
        //PlayerPrefs.SetInt("Player01_Score", Player01ScoreOfPlayerPrefs);  //used for testing
        //PlayerPrefs.SetInt("HighScore", HighScoreOfPlayerPrefs);            //used for testing */

    }
    private void Update() 
    {
       
        //used for testing
        if(Input.GetKeyDown(KeyCode.E))     //used as the selecting amount of player because now I havn't done selecting amount of player script
        {
            Score.ScorePlayer01 = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        DisplayHighScore();
        DisplayScorePlayer01();
    }

    private void DisplayScorePlayer01()
    {
            //use PlayerPrefs.GetInt("Player01_Score") because Menu_Scene will show last score player can do
            //this will work together with LoadAfterGameOver() in GameState by setting --> PlayerPrefs.SetInt("Player01_Score", Score.ScorePlayer01);
            if(PlayerPrefs.GetInt("Player01_Score") == 0 )
            {
                Score_UI.Player01ScoreTextUI.text = $"000000";
            }
            if(PlayerPrefs.GetInt("Player01_Score") > 0)
            {
                if(PlayerPrefs.GetInt("Player01_Score") < 100000 && PlayerPrefs.GetInt("Player01_Score") >= 10000)
                {
                    Score_UI.Player01ScoreTextUI.text = $"0{PlayerPrefs.GetInt("Player01_Score")}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                }
                else if (PlayerPrefs.GetInt("Player01_Score") < 10000 && PlayerPrefs.GetInt("Player01_Score") >= 1000)
                {
                    Score_UI.Player01ScoreTextUI.text = $"00{PlayerPrefs.GetInt("Player01_Score")}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }
                else if (PlayerPrefs.GetInt("Player01_Score") < 1000 && PlayerPrefs.GetInt("Player01_Score") >= 100)
                {
                    Score_UI.Player01ScoreTextUI.text = $"000{PlayerPrefs.GetInt("Player01_Score")}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }
                else if (PlayerPrefs.GetInt("Player01_Score") < 100)
                {
                    Score_UI.Player01ScoreTextUI.text = $"000000";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }

            }
    }

    private void DisplayHighScore()
    {
        if(PlayerPrefs.GetInt("HighScore") < 100000 && PlayerPrefs.GetInt("HighScore") >= 10000)
        {
            Score_UI.HighScoreTextUI.text = $"0{PlayerPrefs.GetInt("HighScore")}";
        }
        else if (PlayerPrefs.GetInt("HighScore") < 10000 && PlayerPrefs.GetInt("HighScore") >= 1000)
        {
            Score_UI.HighScoreTextUI.text = $"00{PlayerPrefs.GetInt("HighScore")}";
        }
        else if (PlayerPrefs.GetInt("HighScore") < 1000 && PlayerPrefs.GetInt("HighScore") >= 100)
        {
            Score_UI.HighScoreTextUI.text = $"000{PlayerPrefs.GetInt("HighScore")}";
        }
        else if (PlayerPrefs.GetInt("HighScore") < 100)
        {
            Score_UI.HighScoreTextUI.text = $"000000";
        }
    }

}