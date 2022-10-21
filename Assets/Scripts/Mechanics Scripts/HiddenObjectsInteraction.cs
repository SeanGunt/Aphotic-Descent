using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectsInteraction : MonoBehaviour
{   
    public Material[] hMat;
    [SerializeField] Renderer hRend;
    public bool objRevealed;
    [SerializeField] private float revealedTime, maxRevealed;

    void Awake()
    {
        hRend = this.GetComponent<Renderer>();
        hRend.enabled = true;
        hRend.sharedMaterial = hMat[0];
        objRevealed = false;
    }

    void Update()
    {
        revealedTime = Mathf.Clamp(revealedTime, 0f, maxRevealed);
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

    void RevelationTimer()
    {
        revealedTime -= Time.deltaTime;
        if (revealedTime <= 0)
        {
            objRevealed = false;
            revealedTime = maxRevealed;
        }
    }
}
