using System.Linq;
using UnityEngine;

namespace ScoreManagement
{
    public class MenuPlayerScoreDisplay : MonoBehaviour 
    {
        private void Start() 
        {
            PlayerPrefs.GetInt("Player01_Score", 0);
            PlayerPrefs.GetInt("Player02_Score", 0);

            //Set highscore as the top 1
            int highestScore = LoadAndSaveScore.player_list.Max(player => player.Score);
            PlayerPrefs.SetInt("HighScore", highestScore);
        }
        private void Update() 
        {
            DisplayHighScore();
            DisplayPlayerScore();
        }

        private void DisplayPlayerScore()
        {
            //use PlayerPrefs.GetInt("Player01_Score") because MenuScene will show last score player can do
            //this will work together with LoadAfterGameOver() in GameState by setting --> PlayerPrefs.SetInt("Player01_Score", Score.ScorePlayer01);
            #region Player 1
                if(PlayerPrefs.GetInt("Player01_Score") == 0 )
                {
                    ScoreUI.Player01ScoreTextUI.text = $"000000";
                }
                if(PlayerPrefs.GetInt("Player01_Score") > 0)
                {
                    if(PlayerPrefs.GetInt("Player01_Score") < 999999 && PlayerPrefs.GetInt("Player01_Score") >= 10000)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"0{PlayerPrefs.GetInt("Player01_Score")}";
                        // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                    }
                    else if (PlayerPrefs.GetInt("Player01_Score") < 10000 && PlayerPrefs.GetInt("Player01_Score") >= 1000)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"00{PlayerPrefs.GetInt("Player01_Score")}";
                        // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                    }
                    else if (PlayerPrefs.GetInt("Player01_Score") < 1000 && PlayerPrefs.GetInt("Player01_Score") >= 100)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"000{PlayerPrefs.GetInt("Player01_Score")}";
                        // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                    }
                    else if (PlayerPrefs.GetInt("Player01_Score") < 100)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"000000";
                        // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                    }

                }
            #endregion

            #region Player 2
                if(OptionSelection.Player_Amount == 2)
                {
                    ScoreUI.Player02ScoreTextUI.gameObject.SetActive(true);
                    ScoreUI.Player02TurnUI.gameObject.SetActive(true);
                    if(PlayerPrefs.GetInt("Player02_Score") == 0 )
                    {
                        ScoreUI.Player02ScoreTextUI.text = $"000000";
                    }
                    else if(PlayerPrefs.GetInt("Player02_Score") > 0)
                    {
                        if(PlayerPrefs.GetInt("Player02_Score") < 999999 && PlayerPrefs.GetInt("Player02_Score") >= 10000)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"0{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 10000 && PlayerPrefs.GetInt("Player02_Score") >= 1000)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"00{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 1000 && PlayerPrefs.GetInt("Player02_Score") >= 100)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"000{PlayerPrefs.GetInt("Player02_Score")}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (PlayerPrefs.GetInt("Player02_Score") < 100)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"000000";
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
                ScoreUI.HighScoreTextUI.text = $"0{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 10000 && PlayerPrefs.GetInt("HighScore") >= 1000)
            {
                ScoreUI.HighScoreTextUI.text = $"00{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 1000 && PlayerPrefs.GetInt("HighScore") >= 100)
            {
                ScoreUI.HighScoreTextUI.text = $"000{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 100)
            {
                ScoreUI.HighScoreTextUI.text = $"000000";
            }
        }

    }
}