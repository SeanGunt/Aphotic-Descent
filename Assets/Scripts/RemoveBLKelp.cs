using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBLKelp : MonoBehaviour
{
    [SerializeField] private GameObject kelpHolder;
    [SerializeField] private MeshCollider generator;
    
    void Update()
    {
        if (GameDataHolder.eelIsDead)
        {
            Destroy(kelpHolder);
            generator.enabled = true;
        }
    }
}
