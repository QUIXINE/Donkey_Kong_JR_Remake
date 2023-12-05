using System.Linq;
using UnityEngine;

namespace ScoreManagement
{
    public class Menu_Player_Score_Display : MonoBehaviour 
    {
        //Used for testing 
       /*  [Tooltip("used only for testing")]
        [SerializeField] private int HighScoreOfPlayerPrefs;    //used to change PlayerPrefs("HighScore") value to test faster 
        [Tooltip("used only for testing")]
        [SerializeField] private int Player01ScoreOfPlayerPrefs;    //used to change PlayerPrefs("HighScore") value to test faster  */
        
        private void Start() 
        {
            PlayerPrefs.GetInt("Player01_Score", 0);
            PlayerPrefs.GetInt("Player02_Score", 0);

            //Set highscore as the top 1
            int highestScore = Load_And_Save_Score.player_list.Max(player => player.Score);
            PlayerPrefs.SetInt("HighScore", highestScore);

            //PlayerPrefs.SetInt("Player01_Score", Player01ScoreOfPlayerPrefs);     //used for testing
            //PlayerPrefs.GetInt("HighScore");                                      //used for testing
            //PlayerPrefs.SetInt("HighScore", HighScoreOfPlayerPrefs);              //used for testing

        }
        private void Update() 
        {
            DisplayHighScore();
            DisplayPlayerScore();
        }

        private void DisplayPlayerScore()
        {
            //use PlayerPrefs.GetInt("Player01_Score") because Menu_Scene will show last score player can do
            //this will work together with LoadAfterGameOver() in GameState by setting --> PlayerPrefs.SetInt("Player01_Score", Score.ScorePlayer01);
            #region Player 1
                if(PlayerPrefs.GetInt("Player01_Score") == 0 )
                {
                    Score_UI.Player01ScoreTextUI.text = $"000000";
                }
                if(PlayerPrefs.GetInt("Player01_Score") > 0)
                {
                    if(PlayerPrefs.GetInt("Player01_Score") < 999999 && PlayerPrefs.GetInt("Player01_Score") >= 10000)
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
            #endregion

            #region Player 2
                if(Option_Selection.Player_Amount == 2)
                {
                    Score_UI.Player02ScoreTextUI.gameObject.SetActive(true);
                    Score_UI.Player02TurnUI.gameObject.SetActive(true);
                    if(PlayerPrefs.GetInt("Player02_Score") == 0 )
                    {
                        Score_UI.Player02ScoreTextUI.text = $"000000";
                    }
                    else if(PlayerPrefs.GetInt("Player02_Score") > 0)
                    {
                        if(PlayerPrefs.GetInt("Player02_Score") < 999999 && PlayerPrefs.GetInt("Player02_Score") >= 10000)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"0{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 10000 && PlayerPrefs.GetInt("Player02_Score") >= 1000)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"00{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 1000 && PlayerPrefs.GetInt("Player02_Score") >= 100)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"000{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 100)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"000000";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }

                    }
                }
            #endregion
        }

        private void DisplayHighScore()
        {
            if(PlayerPrefs.GetInt("HighScore") < 999999 && PlayerPrefs.GetInt("HighScore") >= 10000)
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
}