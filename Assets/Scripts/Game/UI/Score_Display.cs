using UnityEngine;


namespace ScoreManagement
{
    public class Score_Display : MonoBehaviour 
    {
        private void Update() 
        {
            DisplayerScorePlayer01();
            DisplayHighScore();
            DisplayBonusScore();
            DisplayLapAmount();
        }


        private void DisplayerScorePlayer01()
        {
            #region Player 1
                if(Score_Variables.ScorePlayer01 == 0 )
                {
                    Score_UI.Player01ScoreTextUI.text = $"000000";
                }
                if(Score_Variables.ScorePlayer01 > 0)
                {
                    if(Score_Variables.ScorePlayer01 < 999999 && Score_Variables.ScorePlayer01 >= 10000)
                    {
                        Score_UI.Player01ScoreTextUI.text = $"0{Score_Variables.ScorePlayer01}";
    
                    }
                    else if (Score_Variables.ScorePlayer01 < 10000 && Score_Variables.ScorePlayer01 >= 1000)
                    {
                        Score_UI.Player01ScoreTextUI.text = $"00{Score_Variables.ScorePlayer01}";
                    }
                    else if (Score_Variables.ScorePlayer01 < 1000 && Score_Variables.ScorePlayer01 >= 100)
                    {
                        Score_UI.Player01ScoreTextUI.text = $"000{Score_Variables.ScorePlayer01}";
                    }
                    else if (Score_Variables.ScorePlayer01 < 100)
                    {
                        Score_UI.Player01ScoreTextUI.text = $"000000";
                    }
    
                }
            #endregion

           
            #region Player 2
                if(PlayerPrefs.GetInt("Player_Amount") == 2)
                {
                    Score_UI.Player02ScoreTextUI.gameObject.SetActive(true);
                    Score_UI.Player02TurnUI.gameObject.SetActive(true);
                    if(Score_Variables.ScorePlayer02 == 0 )
                    {
                        Score_UI.Player02ScoreTextUI.text = $"000000";
                    }
                    else if(Score_Variables.ScorePlayer02 > 0)
                    {
                        if(Score_Variables.ScorePlayer02 < 999999 && Score_Variables.ScorePlayer02 >= 10000)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"0{Score_Variables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                        }
                        else if (Score_Variables.ScorePlayer02 < 10000 && Score_Variables.ScorePlayer02 >= 1000)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"00{Score_Variables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (Score_Variables.ScorePlayer02 < 1000 && Score_Variables.ScorePlayer02 >= 100)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"000{Score_Variables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (Score_Variables.ScorePlayer02 < 100)
                        {
                            Score_UI.Player02ScoreTextUI.text = $"000000";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }

                    }
                }
            #endregion
        }

        private void DisplayBonusScore()
        {
            Score_UI.BonusScoreTextUI.text = $"{Score_Variables.bonusScore}";
        }
        
        private void DisplayLapAmount()
        {
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                Score_UI.LapAmountTextUI.text = $"{Score_Variables.lapAmountPlayer01}";
            }
            else
            {
                Score_UI.LapAmountTextUI.text = $"{Score_Variables.lapAmountPlayer02}";
            }
        }

        public void DisplayHighScore()
        {
            if((Score_Variables.ScorePlayer01 > PlayerPrefs.GetInt("HighScore")) || (Score_Variables.ScorePlayer02 > PlayerPrefs.GetInt("HighScore")))   
            {
                //1. check every time ScorePlayer01 > highScore then display with next condition
                //2. check which player shold be on highscore
                if(Score_Variables.ScorePlayer01 > Score_Variables.ScorePlayer02 && PlayerPrefs.GetInt("Current_Player") == 1)
                {
                    PlayerPrefs.SetInt("HighScore", Score_Variables.ScorePlayer01);
                }
                else if(Score_Variables.ScorePlayer02 > Score_Variables.ScorePlayer01 && PlayerPrefs.GetInt("Current_Player") == 2)
                {
                    PlayerPrefs.SetInt("HighScore", Score_Variables.ScorePlayer02);
                }
            }
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
