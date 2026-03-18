using System;

/// PATTERN: Observer (12 pts)
/// A lightweight publish/subscribe event bus. GameController (publisher) fires
/// events like OnGamePaused and OnGameEnded. Any number of subscribers
/// (UI panels, audio, analytics) register callbacks without the publisher
/// needing a direct reference to any of them.
/// This removes the rigid coupling where GameController manually toggled
/// every component — new listeners can subscribe without touching GameController.

public static class GameEventBus
{
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameEnded;
    

    public static void PublishPaused() => OnGamePaused?.Invoke();
    public static void PublishResumed() => OnGameResumed?.Invoke();
    public static void PublishEnded() => OnGameEnded?.Invoke();
    
}