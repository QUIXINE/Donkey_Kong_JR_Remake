using System.Collections;
using UnityEngine;
using TakeDamage;

namespace ScoreManagement
{
    public class BonusScoreManager : MonoBehaviour 
    {
        private bool isAudioPlayed;

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
                switch (ScoreVariables.lapAmountPlayer01)
                {
                    case 0:
                        break;
                    case 1:
                        ScoreVariables.bonusScore = 5000;
                        break;
                    case 2:
                        ScoreVariables.bonusScore = 6000;
                        break;
                    case 3:
                        ScoreVariables.bonusScore = 7000;
                        break;
                    default:
                        ScoreVariables.bonusScore = 8000;
                        break;
                }
            }
            else 
            {
                switch (ScoreVariables.lapAmountPlayer02)
                {
                    case 0:
                        break;
                    case 1:
                        ScoreVariables.bonusScore = 5000;
                        break;
                    case 2:
                        ScoreVariables.bonusScore = 6000;
                        break;
                    case 3:
                        ScoreVariables.bonusScore = 7000;
                        break;
                    default:
                        ScoreVariables.bonusScore = 8000;
                        break;
                }
            }
            //DisplayBonusScore();
        }

        
        public void DecreaseBonus()
        {   
            ScoreVariables.bonusScore -= 100;
            if(ScoreVariables.bonusScore <= 0)
            {
                StartCoroutine(ZeroBonus());
                CancelInvoke(nameof(DecreaseBonus));
            }

            else if(ScoreVariables.bonusScore < 1000)
            {
                //Play emergency audio
                if(!isAudioPlayed)
                {
                    AudioReferences.BGSoundObj.SetActive(false);
                    AudioReferences.AlarmSoundObj.SetActive(true);
                    isAudioPlayed = true;
                }
                //Change text color
                ScoreUI.BonusScoreTextUI.color = new Color32(148, 87, 235, 255);
            }
            //DisplayBonusScore();
        }

        public void StopBonusDecreasement()
        {
            CancelInvoke(nameof(DecreaseBonus));
        }

        private IEnumerator ZeroBonus()
        {
            //-1 life after bonus score reaches 0 for 3 seconds
            yield return new WaitForSeconds(3f);
            PlayerTakeDamage playerTakeDamage = FindObjectOfType<PlayerTakeDamage>();
            playerTakeDamage.animator.SetBool("DieOtherCondition", true);
            playerTakeDamage.TakeDamage();
            StopCoroutine(ZeroBonus());
        }
    }
}
