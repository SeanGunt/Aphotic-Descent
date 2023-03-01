using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuitChange : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer modelYouWantToChange;
    [SerializeField] Mesh modelToChangeTo;
    [SerializeField] private UnityEvent unityEvent;

    void Update()
    {
        if (GameDataHolder.hasUpgradedSuit)
        {
            ChangeMesh();
        }
    }

    public void ChangeMesh()
    {
        modelYouWantToChange.sharedMesh = modelToChangeTo;
    }
}
