using PlayerSpace;
using ScoreManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetScoreVariables : MonoBehaviour 
{
    private void Awake() 
    {
        ScoreVariables.lapAmountPlayer01 = 1;
        ScoreVariables.lapAmountPlayer02 = 1;
        ScoreVariables.ScorePlayer01 = 0;    //if there's a problem w/ coroutine used this in GameState LoadAfterGameOver() or Player_Selection GetInGame()
        ScoreVariables.ScorePlayer02 = 0;    //if there's a problem w/ coroutine used this in GameState LoadAfterGameOver() or Player_Selection GetInGame()
    }
}