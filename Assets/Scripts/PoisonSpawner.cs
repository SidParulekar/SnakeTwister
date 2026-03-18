using UnityEngine;

public class PoisonSpawner : ConsumableSpawner
{
    [SerializeField] private GameObject poisonPrefab;

    protected override GameObject CreateConsumable()
    {
        return Instantiate(poisonPrefab);
    }    
}