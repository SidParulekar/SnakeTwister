using UnityEngine;

/// Concrete snake for Player 2.
/// Only responsibilities: supply starting direction and Arrow-key input strategy.

public class SnakeTwo : SnakeController
{
    protected override Vector2 GetStartDirection()
        => Vector2.left;

    /// PATTERN: Template Method hook — returns the Strategy for this player.
    protected override IInputStrategy GetInputStrategy()
        => new ArrowKeyInputStrategy();
}