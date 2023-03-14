using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglerTriggerScr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("KILL PLAYER");
            //pHelCon.ChangeHealth(-15.0f);
            //pHelCon.TakeDamage();
        }
    }
}
