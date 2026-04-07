using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    public void ChangeResolution()
    {
        switch (_resolutionDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(1920, 1080, false);
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(1920, 1080, false);
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(1280, 720, false);

                break;
            default:
                break;
        }
    }
}
