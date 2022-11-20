using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectsInteraction : MonoBehaviour
{   
    public Material[] hMat;
    [SerializeField] Renderer hRend;
    public bool objRevealed, objSpawned, canSpawnObjects;
    [SerializeField] private float revealedTime, maxRevealed;
    private SpawnHiddenObject sHO;

    void Awake()
    {
        hRend = this.GetComponent<Renderer>();
        hRend.enabled = true;
        hRend.sharedMaterial = hMat[0];
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
                MaterialChange();
                objSpawned = sHO.objSpawned;
            }
            else
            {
                MaterialChange();
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

    void MaterialChange()
    {
        if (objRevealed)
        {
            hRend.sharedMaterial = hMat[1];
            RevelationTimer();
        }
        if (!objRevealed)
        {
            hRend.sharedMaterial = hMat[0];
        }
    }
}
