using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisPickup : MonoBehaviour
{
    [SerializeField]bool isInactive;
    [SerializeField]GameObject Myself, pickupParticle;
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        InvisibilityMechanic controller = col.GetComponent<InvisibilityMechanic>();

        if(controller != null)
        {
            if(!isInactive)
            {
                CreateParticle();
                isInactive = true;
                Destroy(Myself);
            }
        }
    }

    void CreateParticle()
    {
        Instantiate(pickupParticle, transform.position, transform.rotation);
    }
}
