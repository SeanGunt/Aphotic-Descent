using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglerTriggerScr : MonoBehaviour
{
    anglerAi angScr;
    // Start is called before the first frame update
    void Start()
    {
        angScr = GetComponentInParent<anglerAi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("from angler trigger: KILL PLAYER");
            angScr.pHelCon.ChangeHealth(-15.0f);
            angScr.pHelCon.TakeDamage();
        }
    }
}
