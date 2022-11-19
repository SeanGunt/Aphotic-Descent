using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //In Progress, do not delete or mess with this.
    public void SetMouseSensitivity(float val)
    {
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("Sensitivity", val);
        Debug.Log("Set sensitivity to " + val);
        
    }

}
