using UnityEngine;

public class Poison : Consumable
{
    protected override void OnConsumed(SnakeController snake)
    {
        SoundManager.Instance.Play(Sounds.PoisonConsume);
        snake.Shrink();
    }
}