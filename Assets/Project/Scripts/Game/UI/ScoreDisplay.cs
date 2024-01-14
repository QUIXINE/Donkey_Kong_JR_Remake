using UnityEngine;


namespace ScoreManagement
{
    public class ScoreDisplay : MonoBehaviour 
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
                if(ScoreVariables.ScorePlayer01 == 0 )
                {
                    ScoreUI.Player01ScoreTextUI.text = $"000000";
                }
                if(ScoreVariables.ScorePlayer01 > 0)
                {
                    if(ScoreVariables.ScorePlayer01 < 999999 && ScoreVariables.ScorePlayer01 >= 10000)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"0{ScoreVariables.ScorePlayer01}";
    
                    }
                    else if (ScoreVariables.ScorePlayer01 < 10000 && ScoreVariables.ScorePlayer01 >= 1000)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"00{ScoreVariables.ScorePlayer01}";
                    }
                    else if (ScoreVariables.ScorePlayer01 < 1000 && ScoreVariables.ScorePlayer01 >= 100)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"000{ScoreVariables.ScorePlayer01}";
                    }
                    else if (ScoreVariables.ScorePlayer01 < 100)
                    {
                        ScoreUI.Player01ScoreTextUI.text = $"000000";
                    }
    
                }
            #endregion

           
            #region Player 2
                if(PlayerPrefs.GetInt("Player_Amount") == 2)
                {
                    ScoreUI.Player02ScoreTextUI.gameObject.SetActive(true);
                    ScoreUI.Player02TurnUI.gameObject.SetActive(true);
                    if(ScoreVariables.ScorePlayer02 == 0 )
                    {
                        ScoreUI.Player02ScoreTextUI.text = $"000000";
                    }
                    else if(ScoreVariables.ScorePlayer02 > 0)
                    {
                        if(ScoreVariables.ScorePlayer02 < 999999 && ScoreVariables.ScorePlayer02 >= 10000)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"0{ScoreVariables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                        }
                        else if (ScoreVariables.ScorePlayer02 < 10000 && ScoreVariables.ScorePlayer02 >= 1000)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"00{ScoreVariables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (ScoreVariables.ScorePlayer02 < 1000 && ScoreVariables.ScorePlayer02 >= 100)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"000{ScoreVariables.ScorePlayer02}";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }
                        else if (ScoreVariables.ScorePlayer02 < 100)
                        {
                            ScoreUI.Player02ScoreTextUI.text = $"000000";
                            // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                        }

                    }
                }
            #endregion
        }

        private void DisplayBonusScore()
        {
            ScoreUI.BonusScoreTextUI.text = $"{ScoreVariables.bonusScore}";
        }
        
        private void DisplayLapAmount()
        {
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                ScoreUI.LapAmountTextUI.text = $"{ScoreVariables.lapAmountPlayer01}";
            }
            else
            {
                ScoreUI.LapAmountTextUI.text = $"{ScoreVariables.lapAmountPlayer02}";
            }
        }

        public void DisplayHighScore()
        {
            if((ScoreVariables.ScorePlayer01 > PlayerPrefs.GetInt("HighScore")) || (ScoreVariables.ScorePlayer02 > PlayerPrefs.GetInt("HighScore")))   
            {
                //1. check every time ScorePlayer01 > highScore then display with next condition
                //2. check which player shold be on highscore
                if(ScoreVariables.ScorePlayer01 > ScoreVariables.ScorePlayer02 && PlayerPrefs.GetInt("Current_Player") == 1)
                {
                    PlayerPrefs.SetInt("HighScore", ScoreVariables.ScorePlayer01);
                }
                else if(ScoreVariables.ScorePlayer02 > ScoreVariables.ScorePlayer01 && PlayerPrefs.GetInt("Current_Player") == 2)
                {
                    PlayerPrefs.SetInt("HighScore", ScoreVariables.ScorePlayer02);
                }
            }
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
