using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiolampBox : MonoBehaviour
{
    [SerializeField] private PShrimpBlacklightEvent pShrimpBlacklightEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Destroyer")
        {
            pShrimpBlacklightEvent.canBeBlacklighted = true;
            Destroy(this.gameObject);
        }
    }
}
