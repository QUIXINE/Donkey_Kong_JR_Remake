using System.Collections;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    public static int TotalScore;

    private void Start()
    {
        TotalScore = 0;
        ScoreText = scoreText;
    }

}
