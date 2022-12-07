using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatorScript : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] private int genHealth;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Knife" && isOn == true && (genHealth > 0))
        {
            Debug.Log("generatorHit");
            genHealth -= 1;

            if(genHealth <= 0)
            {
                Debug.Log("generator broke");
                isOn = false;
            }
        }
    }
}
