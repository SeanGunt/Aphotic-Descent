using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHiddenObject : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToBeSpawned;
    public bool objRevealed, objSpawned;
    void Awake()
    {
        objRevealed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objRevealed && !objSpawned)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        foreach (GameObject objectToBeSpawned in objectsToBeSpawned)
        objectToBeSpawned.gameObject.SetActive(true);
        objRevealed = false;
        objSpawned = true;
    }
}
