using PlayerSpace;
using ScoreManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset_Score_Variables : MonoBehaviour 
{
    private void Awake() 
    {
        Score_Variables.lapAmountPlayer01 = 1;
        Score_Variables.lapAmountPlayer02 = 1;
        Score_Variables.ScorePlayer01 = 0;    //if there's a problem w/ coroutine used this in GameState LoadAfterGameOver() or Player_Selection GetInGame()
        Score_Variables.ScorePlayer02 = 0;    //if there's a problem w/ coroutine used this in GameState LoadAfterGameOver() or Player_Selection GetInGame()
    }
}