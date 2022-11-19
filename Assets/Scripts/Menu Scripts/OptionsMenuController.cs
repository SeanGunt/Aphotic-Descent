using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public bool initialized = false;
    public Slider mouseSensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
            Debug.Log("Loaded a sensitivity of " + mouseSensitivitySlider.value);
        }
        initialized = true;
    }

    //In Progress, do not delete or mess with this.
    public void SetMouseSensitivity(float val)
    {
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetFloat("Sensitivity", val);
        Debug.Log("Set sensitivity to " + val);
        
    }

}
