using UnityEngine;

public class Food : Consumable
{
    protected override void OnConsumed(SnakeController snake)
    {
        SoundManager.Instance.Play(Sounds.FoodConsume);
        snake.Grow();
    }
}