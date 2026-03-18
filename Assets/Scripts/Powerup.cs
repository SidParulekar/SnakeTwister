using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] BoxCollider2D playableArea;

    [SerializeField] private GameObject Shield;
    [SerializeField] private GameObject scoreBoost;
    [SerializeField] private GameObject speedBoost;

    private List<GameObject> powerups = new List<GameObject>();

    private float timeElapsed = 0f;

    private bool spawned = false;

    private int powerupIndex;

    [SerializeField] private float timeStay = 3f;
    [SerializeField] private float timeInterval = 5f;


    private void Start()
    {
        powerups.Add(Shield);
        powerups.Add(speedBoost);
        if(scoreBoost)
        {
            powerups.Add(scoreBoost);
        }  
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeInterval)
        {
            spawned = true;
            powerupIndex = Random.Range(0, powerups.Count);
            SpawnPowerup(powerupIndex);

            timeElapsed = 0;
        }

        else if(timeElapsed>=timeStay && spawned) 
        {
            powerups[powerupIndex].SetActive(false);
            timeElapsed = 0;
            spawned = false;
        }
    }

    private void SpawnPowerup(int index)
    {  
        Bounds bounds = this.playableArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        powerups[index].transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

        powerups[index].SetActive(true);
    }

}
