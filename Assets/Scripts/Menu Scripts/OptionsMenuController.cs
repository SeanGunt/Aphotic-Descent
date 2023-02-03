using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public bool initialized = false;
    public bool screenShake = true;
    public Slider mouseSensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
            Debug.Log("Loaded a sensitivity of " + mouseSensitivitySlider.value);
        }
        else
        {
            mouseSensitivitySlider.value = 0.5f;
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivitySlider.value);
        }

        if (PlayerPrefs.HasKey("screenShake"))
        {
            screenShake = PlayerPrefs.GetInt("screenShake") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("screenShake", screenShake ? 1 : 0);
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

    public void SetScreenShake(bool condition)
    {
        int val = 1;
        if (condition = true) val = 1;
        else if(condition = false) val = 0;
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetInt("screenShake", val);
        Debug.Log("Set screen shake to " + val);
    }

}
