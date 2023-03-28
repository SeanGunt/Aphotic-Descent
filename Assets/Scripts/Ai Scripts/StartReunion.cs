using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReunion : MonoBehaviour
{
    [SerializeField] private PistolShrimpInMarsh pistolShrimpInMarsh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pistolShrimpInMarsh.startedEncounter = true;
        }
    }
}
