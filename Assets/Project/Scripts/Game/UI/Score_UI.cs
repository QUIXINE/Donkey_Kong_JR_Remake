using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;


namespace ScoreManagement
{
    //Don't destroyed
    public class Score_UI : MonoBehaviour 
    {

        #region Variables
        [SerializeField] private Canvas score_Count_Canvas;
        [SerializeField] private Canvas bonus_Count_Canvas;
        [SerializeField] private TextMeshProUGUI player01ScoreTextUI;
        [SerializeField] private TextMeshProUGUI player02ScoreTextUI;
        [SerializeField] private GameObject player02TurnUI;
        [SerializeField] private TextMeshProUGUI bonusScoreTextUI;
        [SerializeField] private TextMeshProUGUI lapAmountTextUI;
        [SerializeField] private TextMeshProUGUI highScoreTextUI;
        public static TextMeshProUGUI Player01ScoreTextUI;
        public static TextMeshProUGUI Player02ScoreTextUI;
        public static GameObject Player02TurnUI;
        public static TextMeshProUGUI BonusScoreTextUI;
        public static TextMeshProUGUI LapAmountTextUI;
        public static TextMeshProUGUI HighScoreTextUI;
        #endregion


        private void Awake() 
        {
            Assign_UI_Variables();
        }

        private void Update()
        {
            Canvas_Check();
        }

        private void Canvas_Check()
        {
           
            //Turn off bonus UI
            if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 7)  //Rank scene index 7
            {
                bonus_Count_Canvas.gameObject.SetActive(false);
            }
            else
            {
                bonus_Count_Canvas.gameObject.SetActive(true);
            }
        }

        private void Assign_UI_Variables()
        {
            
            Player01ScoreTextUI =   player01ScoreTextUI;
            Player02ScoreTextUI =   player02ScoreTextUI;
            Player02TurnUI      =   player02TurnUI;
            BonusScoreTextUI    =   bonusScoreTextUI;
            HighScoreTextUI     =   highScoreTextUI;
            LapAmountTextUI     =   lapAmountTextUI;
        }

    }
}