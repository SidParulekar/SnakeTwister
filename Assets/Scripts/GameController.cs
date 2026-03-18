using UnityEngine;
using UnityEngine.SceneManagement;

/// Hosts the State machine (State pattern).
/// Fires events on the GameEventBus (Observer pattern).
/// All public members are intentionally minimal — states access only what they need.

public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private LivesController playerOneLivesController;
    [SerializeField] private LivesController playerTwoLivesController;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private ConsumableSpawner foodController;
    [SerializeField] private ConsumableSpawner poisonController;
    [SerializeField] private Powerup powerupController;
    [SerializeField] private Snake snakeOneController;
    [SerializeField] private SnakeTwo snakeTwoController;

    [Header("UI")]
    public GameObject gameOverUI;
    public GameObject pauseGameScreen;

    // ── State Machine ──────────────────────────────────────────────────────────
    private IGameState currentState;

    private void Awake()
    {
        GameEventBus.OnGamePaused += OnGamePaused;
        GameEventBus.OnGameResumed += OnGameResumed;
        GameEventBus.OnGameEnded += OnGameEnded;
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        pauseGameScreen.SetActive(false);
        TransitionTo(new PlayingState());
    }

    private void Update()
    {
        currentState?.Update(this);
    }

    // Store as fields so unsubscription works correctly
    private void OnGamePaused() { SetGameObjectsEnabled(false); TogglePauseGameScreen(true); }
    private void OnGameResumed() { SetGameObjectsEnabled(true); TogglePauseGameScreen(false); }
    private void OnGameEnded() { SetGameObjectsEnabled(false); SaveHighScore(); }

    /// <summary>Drive the state machine forward to a new state.</summary>
    public void TransitionTo(IGameState nextState)
    {
        currentState?.Exit(this);
        currentState = nextState;
        currentState.Enter(this);
    }

    // ── Helpers used by states ─────────────────────────────────────────────────

    public bool AnyPlayerOutOfLives()
    {
        if (playerOneLivesController.GetLives() == 0) return true;
        if (playerTwoLivesController && playerTwoLivesController.GetLives() == 0) return true;
        return false;
    }

    /// <summary>Enable or disable all gameplay objects at once.</summary>
    public void SetGameObjectsEnabled(bool enabled)
    {
        foodController.enabled = enabled;
        poisonController.enabled = enabled;
        powerupController.enabled = enabled;
        snakeOneController.enabled = enabled;
        if (snakeTwoController) snakeTwoController.enabled = enabled;
    }

    public void TogglePauseGameScreen(bool enabled)
    {
        pauseGameScreen.SetActive(enabled);
    }

    public void SaveHighScore()
    {
        if (!playerTwoLivesController)
            HighScoreController.Instance.SetHighScore(scoreController.GetScore());
    }

    // ── Button callbacks ───────────────────────────────────────────────────────

    public void ReplayGame()
    {
        //GameEventBus.PublishReplayed();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        GameEventBus.OnGamePaused -= OnGamePaused;
        GameEventBus.OnGameResumed -= OnGameResumed;
        GameEventBus.OnGameEnded -= OnGameEnded;
    }

    // ResumeGame kept public for UI buttons that call it directly
    public void ResumeGame() => TransitionTo(new PlayingState());
}