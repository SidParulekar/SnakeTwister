using TMPro;
using UnityEngine;

public class LivesController : MonoBehaviour
{
    private TextMeshProUGUI livesText;
    [SerializeField] private int playerLives = 5;
    [SerializeField] private int playerID;

    private void Awake()
    {
        livesText = GetComponent<TextMeshProUGUI>();
        RefreshUI();
    }

    private void OnEnable()
    {
        SnakeEventBus.OnLivesChanged += OnLivesChanged;
    }

    private void OnDisable()
    {
        SnakeEventBus.OnLivesChanged -= OnLivesChanged;
    }

    private void OnLivesChanged(int ID)
    {
        if (ID != playerID) return;

        if (playerLives > 0)
        {
            playerLives--;
            RefreshUI();
        }      
    }

    public int GetLives() => playerLives;

    private void RefreshUI()
    {
        livesText.text = "Lives: " + playerLives;
    }
}