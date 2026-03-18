using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SnakeEventBus.OnScoreChanged += IncreaseScore;
    }

    private void OnDisable()
    {
        SnakeEventBus.OnScoreChanged -= IncreaseScore;
    }

    private void IncreaseScore(int increment)
    {
        score += increment;
        scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
}