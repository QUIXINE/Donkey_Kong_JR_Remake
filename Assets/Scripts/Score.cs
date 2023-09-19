﻿using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class Score : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public static int TotalScore;

        private void Start()
        {
            TotalScore = 0;
        }
        private void Update()
        {
            ScoreText.text = $"Score : {TotalScore}";
        }

    }
}