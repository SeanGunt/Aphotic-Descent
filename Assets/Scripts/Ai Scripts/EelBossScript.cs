using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EelBossScript : MonoBehaviour
{

    private State state;
    public enum State
    {
        Phase1, Phase2, EB_hiding, EB_attacking
    }


    void Awake()
    {
        //CURRENTLY TESTING:
        //Phase 2
        //be sure to change when testing later

        state = State.Phase2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
