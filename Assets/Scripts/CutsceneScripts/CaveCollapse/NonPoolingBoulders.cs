using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPoolingBoulders : MonoBehaviour
{
    public GameObject[] boulderPrefab;
    private int randomInt;
    public static bool enabledBoulders = true;

    void FixedUpdate()
    {
        randomInt = Random.Range(0, 2);
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-415, -245), -70, Random.Range(550, 650));
        Instantiate(boulderPrefab[randomInt], randomSpawnPosition, Quaternion.identity);
    }

    private void Update()
    {
        if (enabledBoulders == false)
        {
            this.gameObject.SetActive(false);
        }
    }
}
