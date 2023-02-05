using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealHiddenObjects : MonoBehaviour
{
    public GameObject[] objectsToBeRevealed;
    public bool objRevealed, objSpawned, canSpawnObjects;
    private SpawnHiddenObject sHO;
    [SerializeField] private float revealedTime, maxRevealed;

    void Awake()
    {
        objRevealed = false;
    }

    void Start()
    {
        if (this.GetComponent<SpawnHiddenObject>() != false)
        {
            sHO = this.GetComponent<SpawnHiddenObject>();
            canSpawnObjects = true;
        }
        else canSpawnObjects = false;
    }

    void Update()
    {
        revealedTime = Mathf.Clamp(revealedTime, 0f, maxRevealed);
        if (objSpawned == false)
        {
            if (canSpawnObjects)
            {
                RevealObject();
                objSpawned = sHO.objSpawned;
            }
            else
            {
                RevealObject();
            }
        }
    }

    void RevelationTimer()
    {
        revealedTime -= Time.deltaTime;
        if (revealedTime <= 0)
        {
            objRevealed = false;
            revealedTime = maxRevealed;
        }
    }

    void RevealObject()
    {
        if (objRevealed)
        {
            foreach (GameObject objectToBeRevealed in objectsToBeRevealed)
            objectToBeRevealed.gameObject.SetActive(true);
            RevelationTimer();
        }
        if (!objRevealed)
        {
            foreach (GameObject objectToBeRevealed in objectsToBeRevealed)
            objectToBeRevealed.gameObject.SetActive(false);
        }
    }
}
