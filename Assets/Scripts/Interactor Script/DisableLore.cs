using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLore : MonoBehaviour
{
    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (Input.GetButtonDown("Interact"))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
