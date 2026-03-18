using UnityEngine;
using UnityEngine.SceneManagement;

// ── Playing State ─────────────────────────────────────────────────────────────
public class PlayingState : IGameState
{
    public void Enter(GameController ctx)
    {        
        GameEventBus.PublishResumed();
    }

    public void Update(GameController ctx)
    {
        if (ctx.AnyPlayerOutOfLives())
            ctx.TransitionTo(new GameOverState());

        if (Input.GetKeyDown(KeyCode.Escape))
            ctx.TransitionTo(new PausedState());
    }

    public void Exit(GameController ctx) { }
}

// ── Paused State ──────────────────────────────────────────────────────────────
public class PausedState : IGameState
{
    public void Enter(GameController ctx)
    {       
        GameEventBus.PublishPaused();
    }

    public void Update(GameController ctx)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ctx.TransitionTo(new PlayingState());
    }

    public void Exit(GameController ctx)
    {
        ctx.pauseGameScreen.SetActive(false);
    }
}

// ── Game Over State ───────────────────────────────────────────────────────────
public class GameOverState : IGameState
{
    public void Enter(GameController ctx)
    {       
        GameEventBus.PublishEnded();
        ctx.gameOverUI.SetActive(true);       
    }

    public void Update(GameController ctx) { }

    public void Exit(GameController ctx)
    {
        ctx.gameOverUI.SetActive(false);
    }
}
