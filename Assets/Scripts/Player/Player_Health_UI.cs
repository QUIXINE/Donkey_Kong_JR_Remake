using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace
{
    public class Player_Health_UI : MonoBehaviour 
    {
        public List<Image> livesImg = new List<Image>();

        private void Update() 
        {
            //Remove img from list and set actiove player life img
            //Hide health UI
            if(PlayerHealth.Instance.lifeSpriteAmount < PlayerHealth.Instance.health)
            {
                for(int i = 3; i > PlayerHealth.Instance.lifeSpriteAmount; i--)
                {
                    livesImg[i - 1].gameObject.SetActive(false);
                }
                /* if(PlayerHealth.Instance.lifeSpriteAmount < livesImg.Count)
                {
                    print(PlayerHealth.Instance.lifeSpriteAmount);
                    livesImg[PlayerHealth.Instance.lifeSpriteAmount].gameObject.SetActive(false);
                    // livesImg.RemoveAt(Instance.lifeSpriteAmount);
                } */
            }
            //Show health UI
            /* for(int i = 0; i < PlayerHealth.Instance.lifeSpriteAmount; i++)
            {
                livesImg[PlayerHealth.Instance.lifeSpriteAmount].gameObject.SetActive(true);
            } */

        }
    }
}