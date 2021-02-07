using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int number;
    public float spawnRadius;
    public bool spawnOnStart = true;

    void Start()
    {
        if(spawnOnStart)
            SpawnAll();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!spawnOnStart && other.tag=="Player")
            SpawnAll();
    }

    void SpawnAll()
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
                Instantiate(zombiePrefab, hit.position, Quaternion.identity);
            else
                i--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
