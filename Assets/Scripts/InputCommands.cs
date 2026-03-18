using UnityEngine;

/// PATTERN: Command (15 pts)
/// Each directional input is encapsulated as an object implementing IInputCommand.
/// SnakeController stores the pending command and executes it on FixedUpdate,
/// cleanly decoupling input polling (Update) from physics movement (FixedUpdate).
/// Makes replay, undo, or AI control trivially easy to add later.

public interface IInputCommand
{
    bool Execute(SnakeController snake);
}

public class MoveUpCommand : IInputCommand
{
    public bool Execute(SnakeController snake)
    {
        if (snake.Direction != Vector2.down) { snake.Direction = Vector2.up; return true; }
        return false;
    }
}

public class MoveDownCommand : IInputCommand
{
    public bool Execute(SnakeController snake)
    {
        if (snake.Direction != Vector2.up) { snake.Direction = Vector2.down; return true; }
        return false;
    }
}

public class MoveLeftCommand : IInputCommand
{
    public bool Execute(SnakeController snake)
    {
        if (snake.Direction != Vector2.right) { snake.Direction = Vector2.left; return true; }
        return false;
    }
}

public class MoveRightCommand : IInputCommand
{
    public bool Execute(SnakeController snake)
    {
        if (snake.Direction != Vector2.left) { snake.Direction = Vector2.right; return true; }
        return false;
    }
}