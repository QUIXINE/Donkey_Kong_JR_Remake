using System.Collections.Generic;
using ScoreManagement;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace
{
    public class Player_Health_UI : MonoBehaviour 
    {
        public List<Image> LivesImg = new List<Image>();

        private void Update()
        {
            HealthDisplay();

            //used for testing
            if(Input.GetKeyDown(KeyCode.S))
            {
                if(PlayerPrefs.GetInt("Current_Player") == 1)
                Score_Variables.ScorePlayer01 += 9000;
                if(PlayerPrefs.GetInt("Current_Player") == 2)
                Score_Variables.ScorePlayer02 += 9000;
            }
            //print(PlayerPrefs.GetInt("Current_Player"));
        }

        private void HealthDisplay()
        {
            //Remove img from list and set actiove player life img
            //Hide health UI
            if (PlayerPrefs.GetInt("Current_Player") == 1)
            {
                if (Player01Health.Instance.LifeSpriteAmount < Player01Health.Instance.Health && Player01Health.Instance.LifeSpriteAmount < 3)
                {
                    for (int i = 3; i > Player01Health.Instance.LifeSpriteAmount; i--)
                    {
                        LivesImg[i - 1].gameObject.SetActive(false);
                    }
                    /* if(PlayerHealth.Instance.lifeSpriteAmount < livesImg.Count)
                    {
                        print(PlayerHealth.Instance.lifeSpriteAmount);
                        livesImg[PlayerHealth.Instance.lifeSpriteAmount].gameObject.SetActive(false);
                        // livesImg.RemoveAt(Instance.lifeSpriteAmount);
                    } */
                }
                else if (Player01Health.Instance.LifeSpriteAmount == 3)
                {
                    foreach (Image i in LivesImg)
                    {
                        i.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (Player02Health.Instance.LifeSpriteAmount < Player02Health.Instance.Health && Player02Health.Instance.LifeSpriteAmount < 3)
                {
                    for (int i = 3; i > Player02Health.Instance.LifeSpriteAmount; i--)
                    {
                        LivesImg[i - 1].gameObject.SetActive(false);
                    }
                }
                else if (Player02Health.Instance.LifeSpriteAmount == 3)
                {
                    foreach (Image i in LivesImg)
                    {
                        i.gameObject.SetActive(true);
                    }
                }
            }
            //Show health UI
            /* for(int i = 0; i < PlayerHealth.Instance.lifeSpriteAmount; i++)
            {
                livesImg[PlayerHealth.Instance.lifeSpriteAmount].gameObject.SetActive(true);
            } */
        }
    }
}