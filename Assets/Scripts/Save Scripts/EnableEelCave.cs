using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEelCave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameDataHolder.inPsShrimpCave = false;
            GameDataHolder.inEelCave = true;
        }
    }
}
