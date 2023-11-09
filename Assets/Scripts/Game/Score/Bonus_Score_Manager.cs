using System.Collections;
using UnityEngine;
using TakeDamage;

namespace ScoreManagement
{
    public class Bonus_Score_Manager : MonoBehaviour 
    {
        private void Awake() 
        {
            InitializeBonusScore();
        }
        private void Start()
        {
            
            InvokeRepeating(nameof(DecreaseBonus), 4f, 3f); //4f because wait player to become active 1 sec. then decrease bonus every 3 sec.

        }

        private void InitializeBonusScore()
        {
            //bonus score will change with condition depends on Lap amount
            if(PlayerPrefs.GetInt("Current_Player") == 1)
            {
                switch (Score_Variables.lapAmountPlayer01)
                {
                    case 0:
                        break;
                    case 1:
                        Score_Variables.bonusScore = 5000;
                        break;
                    case 2:
                        Score_Variables.bonusScore = 6000;
                        break;
                    case 3:
                        Score_Variables.bonusScore = 7000;
                        break;
                    default:
                        Score_Variables.bonusScore = 8000;
                        break;
                }
            }
            else 
            {
                switch (Score_Variables.lapAmountPlayer02)
                {
                    case 0:
                        break;
                    case 1:
                        Score_Variables.bonusScore = 5000;
                        break;
                    case 2:
                        Score_Variables.bonusScore = 6000;
                        break;
                    case 3:
                        Score_Variables.bonusScore = 7000;
                        break;
                    default:
                        Score_Variables.bonusScore = 8000;
                        break;
                }
            }
            //DisplayBonusScore();
        }

        
        public void DecreaseBonus()
        {   
            Score_Variables.bonusScore -= 100;
            if(Score_Variables.bonusScore <= 0)
            {
                StartCoroutine(ZeroBonus());
                CancelInvoke(nameof(DecreaseBonus));
            }

            if(Score_Variables.bonusScore < 1000)
            {
                //Play emergency audio
                //Change text color
                Score_UI.BonusScoreTextUI.color = new Color32(148, 87, 235, 255);
            }
            //DisplayBonusScore();
        }

        public void StopBonusIncrement()
        {
            CancelInvoke(nameof(DecreaseBonus));
        }

        private IEnumerator ZeroBonus()
        {
            //-1 life after bonus score reaches 0 for 3 seconds
            yield return new WaitForSeconds(3f);
            Player_TakeDamage playerTakeDamage = FindObjectOfType<Player_TakeDamage>();
            playerTakeDamage.animator.SetBool("DieOtherCondition", true);
            playerTakeDamage.TakeDamage();
            StopCoroutine(ZeroBonus());
        }
    }
}
