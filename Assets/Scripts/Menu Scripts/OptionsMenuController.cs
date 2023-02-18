using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public bool initialized = false;
    //public bool screenShake = true;
    public Slider mouseSensitivitySlider;
    public Toggle screenShakeToggle;
    public Toggle headBobToggle;
    public Toggle flashingLightsToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            mouseSensitivitySlider.value = 0.5f;
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivitySlider.value);
        }

        if (PlayerPrefs.HasKey("screenShake"))
        {
            screenShakeToggle.isOn = PlayerPrefs.GetInt("screenShake") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("screenShake", screenShakeToggle.isOn ? 1 : 0);
        }

        if (PlayerPrefs.HasKey("headBob"))
        {
            headBobToggle.isOn = PlayerPrefs.GetInt("headBob") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("headBob", headBobToggle.isOn ? 1 : 0);
        }

        if (PlayerPrefs.HasKey("flashingLights"))
        {
            flashingLightsToggle.isOn = PlayerPrefs.GetInt("flashingLights") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("flashingLights", flashingLightsToggle.isOn ? 1 : 0);
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

    public void SetScreenShake()
    {
        bool condition = screenShakeToggle.isOn;
        int val = 1;
        if (condition == true) val = 1;
        else if (condition == false) val = 0;
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetInt("screenShake", val);
        PlayerPrefs.Save();
        Debug.Log("Set screen shake to " + val);
    }

    public void SetHeadbob()
    {
        bool condition2 = headBobToggle.isOn;
        int val2 = 1;
        if (condition2 == true)
        {
            val2 = 1;
        }
        else if (condition2 == false)
        {
            val2 = 0;
        }
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetInt("headBob", val2);
        PlayerPrefs.Save();
        Debug.Log("Set headbob to " + val2);
    }

    public void SetFlashingLights()
    {
        bool condition3 = flashingLightsToggle.isOn;
        int val3 = 1;
        if (condition3 == true)
        {
            val3 = 1;
        }
        else if (condition3 == false)
        {
            val3 = 0;
        }
        if (!initialized) return;
        if (!Application.isPlaying) return;

        PlayerPrefs.SetInt("flashingLights", val3);
        PlayerPrefs.Save();
        Debug.Log("Set flashingLights to " + val3);
    }
}
