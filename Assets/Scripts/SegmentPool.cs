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
    [SerializeField] private int growAmount = 5; // how many to add when expanding

    private readonly List<Transform> pool = new List<Transform>();
    // List instead of Queue so we can destroy specific items

    // NO pre-population in Awake — pool starts empty, grows on demand

    private Transform CreateSegment()
    {
        Transform seg = Instantiate(segmentPrefab);
        seg.gameObject.SetActive(false);
        return seg;
    }

    public Transform Get(Vector3 position)
    {
        // Find first inactive segment in pool
        foreach (Transform seg in pool)
        {
            if (!seg.gameObject.activeSelf)
            {
                seg.position = position;
                seg.gameObject.SetActive(true);
                return seg;
            }
        }

        // None available — grow the pool
        ExpandPool();
        return Get(position);  // try again after expanding
    }

    /// <summary>Increase pool size by growAmount when demand exceeds supply.</summary>
    private void ExpandPool()
    {
        for (int i = 0; i < growAmount; i++)
            pool.Add(CreateSegment());
    }

    public void Return(Transform seg)
    {
        seg.gameObject.SetActive(false);
        // Object stays in pool list — reusable
    }

    /// <summary>Destroy a specific segment and remove it from the pool entirely.</summary>
    public void Destroy(Transform seg)
    {
        if (pool.Contains(seg))
        {
            pool.Remove(seg);
            GameObject.Destroy(seg.gameObject);
        }
    }

    /// <summary>Destroy all inactive segments — trim pool back down if it grew too large.</summary>
    public void TrimPool()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (Transform seg in pool)
        {
            if (!seg.gameObject.activeSelf)
                toRemove.Add(seg);
        }
        foreach (Transform seg in toRemove)
        {
            pool.Remove(seg);
            GameObject.Destroy(seg.gameObject);
        }
    }
}