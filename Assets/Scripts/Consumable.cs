using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    protected ConsumableSpawner spawner;

    public void Initialize(ConsumableSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeController snake = collision.GetComponent<SnakeController>();
        if (snake == null) return;

        OnConsumed(snake);

        // Notify spawner
        spawner.HandleConsumed(this, snake);
    }

    protected abstract void OnConsumed(SnakeController snake);
}