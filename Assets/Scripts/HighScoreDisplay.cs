using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private int highScore;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        DisplayHighScore();
    }

    public void DisplayHighScore()
    {
        highScore = HighScoreController.Instance.GetHighScore();
        RefreshUI();
    }

    private void RefreshUI()
    {
        scoreText.text = "High Score: " + highScore;
    }
}
