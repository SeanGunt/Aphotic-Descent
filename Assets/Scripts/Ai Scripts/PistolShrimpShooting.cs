using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShooting : MonoBehaviour
{
    private RaycastHit hit;
    string nameOfHit;
    [SerializeField] private LayerMask ignoreLayers;
    private void Update()
    {
        if(Physics.Raycast(this.transform.position, Vector3.up, out hit, 25f, ~ignoreLayers))
        {
            nameOfHit = hit.collider.name;
            Debug.Log(nameOfHit);
        }
    }
}
