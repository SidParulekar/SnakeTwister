using UnityEngine;

/// PATTERN: Strategy (15 pts)
/// Defines a family of interchangeable input-reading algorithms.
/// Snake uses WASDInputStrategy; SnakeTwo uses ArrowKeyInputStrategy.

public interface IInputStrategy
{
    IInputCommand ReadInput();
}

public class WASDInputStrategy : IInputStrategy
{
    public IInputCommand ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) return new MoveUpCommand();
        if (Input.GetKeyDown(KeyCode.S)) return new MoveDownCommand();
        if (Input.GetKeyDown(KeyCode.A)) return new MoveLeftCommand();
        if (Input.GetKeyDown(KeyCode.D)) return new MoveRightCommand();
        return null;
    }
}

public class ArrowKeyInputStrategy : IInputStrategy
{
    public IInputCommand ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return new MoveUpCommand();
        if (Input.GetKeyDown(KeyCode.DownArrow)) return new MoveDownCommand();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return new MoveLeftCommand();
        if (Input.GetKeyDown(KeyCode.RightArrow)) return new MoveRightCommand();
        return null;
    }
}