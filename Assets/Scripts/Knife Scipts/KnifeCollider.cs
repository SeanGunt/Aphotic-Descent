using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider bc;

    public void EnableCollider()
    {
        bc.enabled = true;
        Debug.Log("Enabled");
    }

    public void DisableCollider()
    {
        bc.enabled = false;
        Debug.Log("Disabled");
    }
}
