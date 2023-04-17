using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuitChange : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer normalSuit;
    private bool meshChanged = false;
    [SerializeField] Mesh eelSuit;
    [SerializeField] private Material[] eelMat;


    void Update()
    {
        if (GameDataHolder.hasUpgradedSuit && !meshChanged)
        {
            ChangeMesh();
            meshChanged = true;
        }
    }

    public void ChangeMesh()
    {
        normalSuit.sharedMesh = eelSuit;
        normalSuit.sharedMaterials = eelMat;
    }
}
