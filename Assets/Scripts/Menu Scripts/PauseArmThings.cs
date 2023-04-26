using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseArmThings : MonoBehaviour
{
    private PauseControls pauseControls;

    private void Awake()
    {
        pauseControls = GetComponentInParent<PauseControls>();
    }

    public void CallPause()
    {
        pauseControls.PauseTheGame();
    }
}
