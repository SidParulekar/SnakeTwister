using System.Collections.Generic;
using UnityEngine;

/// PATTERN: Object Pool (15 pts)
/// Reuses snake body segment GameObjects instead of calling Instantiate/Destroy
/// every time the snake grows or shrinks. 
/// Get() pulls a dormant object from the pool (or creates one if empty).
/// Return() deactivates and returns it for future reuse.

public class SegmentPool : MonoBehaviour
{
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private int initialPoolSize = 20;

    private readonly Queue<Transform> pool = new Queue<Transform>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
            pool.Enqueue(CreateSegment());
    }

    private Transform CreateSegment()
    {
        Transform seg = Instantiate(segmentPrefab);
        seg.gameObject.SetActive(false);
        return seg;
    }

    public Transform Get(Vector3 position)
    {
        Transform seg = pool.Count > 0 ? pool.Dequeue() : CreateSegment();
        seg.position = position;
        seg.gameObject.SetActive(true);
        return seg;
    }

    public void Return(Transform seg)
    {
        seg.gameObject.SetActive(false);
        pool.Enqueue(seg);
    }
}