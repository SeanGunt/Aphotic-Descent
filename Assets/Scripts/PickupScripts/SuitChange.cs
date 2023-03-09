using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuitChange : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer normalSuit;
    [SerializeField] Mesh eelSuit;

    void Update()
    {
        if (GameDataHolder.hasUpgradedSuit)
        {
            ChangeMesh();
        }
    }

    public void ChangeMesh()
    {
        normalSuit.sharedMesh = eelSuit;
    }
}
