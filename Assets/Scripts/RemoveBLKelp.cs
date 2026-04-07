using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBLKelp : MonoBehaviour
{
    [SerializeField] private GameObject kelpHolder;
    
    void Update()
    {
        if (GameDataHolder.eelIsDead)
        {
            Destroy(kelpHolder);
        }
    }
}
