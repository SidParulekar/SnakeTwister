using UnityEngine;
//Singleton

public class HighScoreController : MonoBehaviour
{
    private static HighScoreController instance;
    public static HighScoreController Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("HighScoreController not found in scene!");
            return instance;
        }
    }

    private int highScore;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetHighScore(int currentScore)
    {
        if (highScore < currentScore)
            highScore = currentScore;
    }

    public int GetHighScore() => highScore;
}