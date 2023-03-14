using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerCutscene : MonoBehaviour
{

    [SerializeField] private Material anglerSkin;
    [SerializeField] private Renderer anglerObject;
    [SerializeField] private BoxCollider lightUpCollider;

    private void Start()
    {
        lightUpCollider = GetComponent<BoxCollider>();
        anglerSkin = anglerObject.GetComponent<Renderer>().material;
        anglerSkin.DisableKeyword("_EMISSION");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anglerSkin.EnableKeyword("_EMISSION");
            Debug.Log("Light up!");
        }
    }
}
