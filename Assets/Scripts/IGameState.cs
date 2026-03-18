/// PATTERN: State (20 pts)
/// GameController delegates all behaviour to a current IGameState.
/// Transitioning from Playing → Paused → GameOver is just a field swap.
/// Each state class encapsulates exactly what should happen on entry (Enter),
/// every frame (Update), and on exit (Exit), eliminating the tangled
/// if/else chains in the original GameController.Update().

public interface IGameState
{
    void Enter(GameController ctx);
    void Update(GameController ctx);
    void Exit(GameController ctx);
}