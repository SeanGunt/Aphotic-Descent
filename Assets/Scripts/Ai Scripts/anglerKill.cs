using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglerKill : MonoBehaviour
{
    public bool turnOffAngler = false;
    public GameObject anglerFish;
    anglerAi aAi;

    // Start is called before the first frame update
    void Start()
    {
        aAi = anglerFish.GetComponent<anglerAi>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turnOffAngler)
        {
            Debug.Log("turned off angler");
            aAi.isAlive = false;
            aAi.state = anglerAi.State.anglerDead;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            turnOffAngler = true;
        }
    }
}
