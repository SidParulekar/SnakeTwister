using System;

/// PATTERN: Observer (12 pts)
/// Snake publishes events. UI controllers subscribe and react.
/// Snake has zero knowledge of ScoreController, LivesController

public static class SnakeEventBus
{
    public static event Action<int> OnScoreChanged;      // score increment
    public static event Action<int> OnLivesChanged;      // lives remaining    

    public static void PublishScoreChanged(int increment) => OnScoreChanged?.Invoke(increment);
    public static void PublishLivesChanged(int playerID) => OnLivesChanged?.Invoke(playerID);
    
}