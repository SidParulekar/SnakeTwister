using UnityEngine;

public class FoodSpawner : ConsumableSpawner
{
    [SerializeField] private GameObject foodPrefab;

    protected override GameObject CreateConsumable()
    {
        return Instantiate(foodPrefab);
    }
   
}