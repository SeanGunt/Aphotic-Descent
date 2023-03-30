using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDespawner : MonoBehaviour
{
    void Awake()
    {
        Destroy(this.gameObject, 5f);
    }
}
