using UnityEngine;


namespace ScoreManagement
{
    public class Add_New_Highscore : MonoBehaviour 
    {
        private void Awake() 
        {
            Load_And_Save_Score.AddHighScore("", Score_Variables.ScorePlayer01);
            Load_And_Save_Score.AddHighScore("", Score_Variables.ScorePlayer02);
        }
    }
}