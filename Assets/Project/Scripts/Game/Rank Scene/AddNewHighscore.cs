using UnityEngine;


namespace ScoreManagement
{
    public class AddNewHighscore : MonoBehaviour 
    {
        private void Awake() 
        {
            LoadAndSaveScore.AddHighScore("", ScoreVariables.ScorePlayer01);
            LoadAndSaveScore.AddHighScore("", ScoreVariables.ScorePlayer02);
        }
    }
}