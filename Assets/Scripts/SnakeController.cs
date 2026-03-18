using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// PATTERN: Template Method (10 pts)
/// SnakeController defines the invariant skeleton of the snake lifecycle:
///   HandleInput() → [FixedUpdate] SnakeMovement() → CollisionProcessing() → PowerupProcessing()
/// Snake and SnakeTwo override only GetInputStrategy() to supply their
/// control scheme — all other steps are inherited unchanged.
/// This eliminates the near-identical Update/FixedUpdate duplication that
/// existed between Snake.cs and SnakeTwo.cs.
///
/// PATTERN: Object Pool (consumer side, 15 pts — pool itself is in SegmentPool.cs)
/// Grow() calls SegmentPool.Instance.Get() instead of Instantiate().
/// Shrink() calls SegmentPool.Instance.Return() instead of Destroy().

public abstract class SnakeController : MonoBehaviour
{
    // Exposed as property so Command objects can read/write it
    public Vector2 Direction { get; set; }

    [SerializeField] protected SegmentPool segmentPool;

    [SerializeField] protected int playerID;

    [SerializeField] protected int initialPlayerSize;
    [SerializeField] protected ScoreController scoreController;
    [SerializeField] protected LivesController livesController;
    [SerializeField] protected PowerupStatus powerupStatus;
    [SerializeField] protected float powerupEffectTime;
    [SerializeField] protected Vector3 startPosition;

    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;

    protected Vector2 startDirection;
    protected List<Transform> segments = new List<Transform>();

    protected float powerupTimeElapsed = 0f;
    protected int scoreIncrement = 100;
    protected bool shield = false;
    protected bool powerupEnabled = false;
    protected string powerup;

    // ── Strategy (held here, set by concrete subclass) ─────────────────────────
    private IInputStrategy inputStrategy;

    // ── Command queue ──────────────────────────────────────────────────────────
    private IInputCommand pendingCommand;

    // ── Template Method ────────────────────────────────────────────────────────

    /// <summary>Subclasses supply their input strategy once.</summary>
    protected abstract IInputStrategy GetInputStrategy();

    protected abstract Vector2 GetStartDirection();

    protected virtual void Start()
    {
        inputStrategy = GetInputStrategy();
        startDirection = GetStartDirection();
        Direction = startDirection;
        ResetPlayer();
    }

    protected virtual void Update()
    {
        // 1. Read input via Strategy, wrap in Command
        IInputCommand cmd = inputStrategy.ReadInput();
        if (cmd != null) pendingCommand = cmd;

        // 2. Process powerup timers
        PowerupProcessing();

        // 3. Self-disable if dead
        if (livesController.GetLives() == 0)
            enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        // Execute the pending command (Command pattern)
        pendingCommand?.Execute(this);
        pendingCommand = null;

        SnakeMovement();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        CollisionProcessing(collider.tag, collider.gameObject);
    }

    // ── Core snake logic (unchanged algorithm, now shared) ────────────────────

    public void Grow()
    {
        Transform seg = segmentPool.Get(segments[segments.Count - 1].position);
        segments.Add(seg);
    }

    public void Shrink()
    {
        if (!shield)
        {
            if (segments.Count > 1)
            {
                Transform last = segments[segments.Count - 1];
                segments.RemoveAt(segments.Count - 1);
                segmentPool.Return(last);
            }
            else
            {
                KillPlayer();
                ResetPlayer();
            }
        }
        
    }

    protected void ResetPlayer()
    {
        transform.position = startPosition;

        // Return all body segments except head to pool
        for (int i = 1; i < segments.Count; i++)
            segmentPool.Return(segments[i]);

        segments.Clear();
        segments.Add(transform);

        for (int i = 1; i < initialPlayerSize; i++)
            segments.Add(segmentPool.Get(startPosition));

        Direction = startDirection;
    }

    protected void KillPlayer()
    {
       
       SnakeEventBus.PublishLivesChanged(playerID);
        
    }

    protected void SnakeMovement()
    {
        for (int i = segments.Count - 1; i > 0; i--)
            segments[i].position = segments[i - 1].position;

        transform.position = new Vector3(
            Mathf.Round(transform.position.x + Direction.x),
            Mathf.Round(transform.position.y + Direction.y),
            0f);

        if (SceneManager.GetActiveScene().buildIndex != 0)
            ScreenWrap();
    }

    protected void ScreenWrap()
    {
        Vector3 p = transform.position;
        float right = rightWall.transform.position.x - 1f;
        float left = leftWall.transform.position.x + 1f;
        float top = topWall.transform.position.y - 1f;
        float bottom = bottomWall.transform.position.y + 1f;

        if (p.x <= left && Direction == Vector2.left) transform.position = new Vector3(right, p.y, 0);
        if (p.x >= right && Direction == Vector2.right) transform.position = new Vector3(left, p.y, 0);
        if (p.y >= top && Direction == Vector2.up) transform.position = new Vector3(p.x, bottom, 0);
        if (p.y <= bottom && Direction == Vector2.down) transform.position = new Vector3(p.x, top, 0);
    }

    protected void CollisionProcessing(string tag, GameObject obj)
    {
        switch (tag)
        {
            case "Food":               
                SnakeEventBus.PublishScoreChanged(scoreIncrement);
                break;            

            case "ScoreBoost":
                SoundManager.Instance.Play(Sounds.ScoreBoostPickup);
                obj.SetActive(false);
                ActivatePowerup(tag, "Score Boost", () => scoreIncrement = 200);
                break;

            case "SpeedBoost":
                SoundManager.Instance.Play(Sounds.SpeedBoostPickup);
                obj.SetActive(false);
                ActivatePowerup(tag, "Speed Boost", () => Time.fixedDeltaTime = 0.03f);
                break;

            case "Shield":
                SoundManager.Instance.Play(Sounds.ShieldPickup);
                obj.SetActive(false);
                ActivatePowerup(tag, "Shield", () => shield = true);
                break;

            case "SnakeBody" when !shield:
                SoundManager.Instance.Play(Sounds.PlayerKilled);
                KillPlayer();
                ResetPlayer();
                break;
        }
    }

    private void ActivatePowerup(string tag, string displayName, System.Action effect)
    {
        powerup = tag;
        effect();
        powerupStatus.gameObject.SetActive(true);
        powerupStatus.RefreshUI(displayName, "Active");
        powerupEnabled = true;
    }

    protected void PowerupProcessing()
    {
        if (!powerupEnabled) return;
        powerupTimeElapsed += Time.deltaTime;
        if (powerupTimeElapsed >= powerupEffectTime)
        {
            PowerupDisable(powerup);
            powerupTimeElapsed = 0;
        }
    }

    private void PowerupDisable(string p)
    {
        switch (p)
        {
            case "ScoreBoost": scoreIncrement = 100; break;
            case "Shield": shield = false; break;
            case "SpeedBoost": Time.fixedDeltaTime = 0.04f; break;
        }
        powerupStatus.gameObject.SetActive(false);
        powerupEnabled = false;
    }
}