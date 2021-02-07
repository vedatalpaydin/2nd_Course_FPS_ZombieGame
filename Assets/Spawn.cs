using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int number;
    public float spawnRadius;
    void Start()
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
