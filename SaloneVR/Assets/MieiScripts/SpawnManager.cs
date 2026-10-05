using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefabs;
    public float spawnRangeX = 5;
    public float spawnPosZ = 6;
    public float startDelay = 2;
    private float spawnInterval = 3f;
    void Start()
    {
        InvokeRepeating("SpawnZombie", startDelay, spawnInterval);
    }

    
    void SpawnZombie()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);  //Vector3(x, y, z)
        Instantiate(zombiePrefabs, spawnPos, zombiePrefabs.transform.rotation);
    }

    void Update()
    {
        
    }
}
