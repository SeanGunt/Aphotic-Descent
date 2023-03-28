using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    FallingBoulders objectPooler;

    private void Start()
    {
        objectPooler = FallingBoulders.Instance;
    }

    void FixedUpdate()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-415, -245), -50, Random.Range(550, 650));
        objectPooler.SpawnFromPool("Boulders", randomSpawnPosition, Quaternion.identity);
    }
}
