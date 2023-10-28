using UnityEngine;


namespace ScoreManagement
{
    public class Add_New_Highscore : MonoBehaviour 
    {
        private void Start() 
        {
            Load_And_Save_Json.AddHighScore("", Score.ScorePlayer01);
            print(Score.ScorePlayer01);
        }
    }
}