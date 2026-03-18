using UnityEngine;
//Factory Method
public abstract class ConsumableSpawner : MonoBehaviour
{
    [SerializeField] protected BoxCollider2D playableArea;
    [SerializeField] private float consumableTimeInterval = 5f;

    private float consumableTime = 0f;
    protected GameObject currentConsumable;

    private void Start()
    {
        SpawnConsumable();
    }

    private void Update()
    {
        consumableTime += Time.deltaTime;

        if (consumableTime >= consumableTimeInterval)
        {
            SpawnConsumable();
            consumableTime = 0;
        }
    }

    private void SpawnConsumable()
    {
        if (currentConsumable != null)
            Destroy(currentConsumable);

        currentConsumable = CreateConsumable();

        // Initialize with spawner reference
        Consumable consumable = currentConsumable.GetComponent<Consumable>();
        consumable.Initialize(this);

        Bounds bounds = playableArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        currentConsumable.transform.position =
            new Vector3(Mathf.Round(x), Mathf.Round(y), 0f);
    }

    public void HandleConsumed(Consumable consumable, SnakeController snake)
    {
        

        Destroy(consumable.gameObject);

        SpawnConsumable();
        consumableTime = 0;
    }

    protected abstract GameObject CreateConsumable();
    
}