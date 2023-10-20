using ScoreManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SetScore : MonoBehaviour {
    public int SetScorePlayer;
    public int SetScoreHigh;
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
        {
        Score.ScorePlayer01 = SetScorePlayer;
        PlayerPrefs.SetInt("HighScore", SetScoreHigh);

        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}