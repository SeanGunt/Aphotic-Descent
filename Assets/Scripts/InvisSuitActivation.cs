using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisSuitActivation : MonoBehaviour
{   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<InvisibilityMechanic>() != false)
            {
                GameDataHolder.invisibilityAcquired = true;
                GameDataHolder.hasUpgradedSuit = true;
                other.gameObject.GetComponent<InvisibilityMechanic>().SetInvisUIActive();
                Invoke("DestroyMyself", .05f);
            }
        }
    }

    void DestroyMyself()
    {
        Destroy(this.gameObject);
    }
}
