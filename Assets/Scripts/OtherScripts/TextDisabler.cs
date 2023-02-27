using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisabler : MonoBehaviour
{
    private float disableTimer = 3.0f;
    private bool disabling;

    private void Update()
    {
        if (disabling)
        {
            disableTimer -= Time.deltaTime;
        }

        if (disableTimer <= 0f)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        disabling = true;
    }

    private void OnDisable()
    {
        disableTimer = 3.0f;
        disabling = false;
    }

}
