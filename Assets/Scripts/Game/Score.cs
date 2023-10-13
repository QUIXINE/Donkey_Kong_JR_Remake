using System.Collections;
using UnityEngine;
using TMPro;
using TakeDamage;

namespace ScoreManagement
{
    public class Score : MonoBehaviour
    {
        public static int ScorePlayer01;
        public static int lapAmount;
        public static TextMeshProUGUI ScoreText;
        [SerializeField] private int bonusScore;
        [SerializeField] private int highScore;

        private void Start()
        {
            InitializeBonusScore();
            
            InvokeRepeating(nameof(DecreaseBonus), 4f, 3f); //4f because wait player to become active 1 sec. then decrease bonus every 3 sec.
            highScore = PlayerPrefs.GetInt("HighScore", 0);

        }

        private void InitializeBonusScore()
        {
            //bonus score will change with condition depends on Lap amount
            switch (lapAmount)
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
            }
            DisplayBonusScore();
        }

        private void Update() 
        {
            DisplayerScorePlayer01();
            DisplayHighScore();
            DisplayBonusScore();
            DisplayLapAmount();
        }


        private void DisplayerScorePlayer01()
        {
            if(ScorePlayer01 == 0 )
            {
                Score_Singleton.Player01ScoreTextUI.text = $"000000";
            }
            if(ScorePlayer01 > 0)
            {
                if(ScorePlayer01 < 100000 && ScorePlayer01 >= 10000)
                {
                    Score_Singleton.Player01ScoreTextUI.text = $"0{ScorePlayer01}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";

                }
                else if (ScorePlayer01 < 10000 && ScorePlayer01 >= 1000)
                {
                    Score_Singleton.Player01ScoreTextUI.text = $"00{ScorePlayer01}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }
                else if (ScorePlayer01 < 1000 && ScorePlayer01 >= 100)
                {
                    Score_Singleton.Player01ScoreTextUI.text = $"000{ScorePlayer01}";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }
                else if (ScorePlayer01 < 100)
                {
                    Score_Singleton.Player01ScoreTextUI.text = $"000000";
                    // Score_Singleton.HighScoreTextUI.text = $"0{ScorePlayer01}";
                }

            }
        }

        private void DisplayBonusScore()
        {
            Score_Singleton.BonusScoreTextUI.text = $"{bonusScore}";
        }
        
        private void DisplayLapAmount()
        {
             Score_Singleton.LapAmountTextUI.text = $"{lapAmount}";
        }

        public void DisplayHighScore()
        {
            if(ScorePlayer01 > highScore)   
            {
                //1. check every time ScorePlayer01 > highScore then display with next condition
                //2.can use only PlayerPrefs.SetInt("HighScore", highScore);
                highScore = ScorePlayer01;
                PlayerPrefs.SetInt("HighScore", highScore);
            }
            if(PlayerPrefs.GetInt("HighScore") < 100000 && PlayerPrefs.GetInt("HighScore") >= 10000)
            {
                Score_Singleton.HighScoreTextUI.text = $"0{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 10000 && PlayerPrefs.GetInt("HighScore") >= 1000)
            {
                Score_Singleton.HighScoreTextUI.text = $"00{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 1000 && PlayerPrefs.GetInt("HighScore") >= 100)
            {
                Score_Singleton.HighScoreTextUI.text = $"000{PlayerPrefs.GetInt("HighScore")}";
            }
            else if (PlayerPrefs.GetInt("HighScore") < 100)
            {
                Score_Singleton.HighScoreTextUI.text = $"000000";
            }
        }
        
        public void DecreaseBonus()
        {   
            bonusScore -= 100;
            if(bonusScore <= 0)
            {
                StartCoroutine(ZeroBonus());
                CancelInvoke(nameof(DecreaseBonus));
            }

            if(bonusScore < 1000)
            {
                //Play emergency audio
                //Change text color
                Score_Singleton.BonusScoreTextUI.color = new Color32(148, 87, 235, 255);
            }
            DisplayBonusScore();
        }

        public void StopBonusIncrement()
        {
            CancelInvoke(nameof(DecreaseBonus));
        }

        private IEnumerator ZeroBonus()
        {
            //-1 life after bonus score reaches 0 for 3 seconds
            yield return new WaitForSeconds(3f);
            PlayerTakeDamage playerTakeDamage = FindObjectOfType<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage();
            StopCoroutine(ZeroBonus());
        }
    }
}
