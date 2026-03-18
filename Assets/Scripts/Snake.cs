using UnityEngine;

/// Concrete snake for Player 1.
/// Only responsibilities: supply starting direction and WASD input strategy.
/// All movement, collision, and powerup logic lives in SnakeController (Template Method).

public class Snake : SnakeController
{
    protected override Vector2 GetStartDirection()
        => Vector2.right;

    /// PATTERN: Template Method hook — returns the Strategy for this player.
    protected override IInputStrategy GetInputStrategy()
        => new WASDInputStrategy();
}