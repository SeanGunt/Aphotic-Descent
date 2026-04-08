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
				Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                
                break;
            case 1:
				Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
				//Screen.fullScreenMode = FullScreenMode.Windowed;
                
                break;
            case 2:
				Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
				//Screen.fullScreenMode = FullScreenMode.Windowed;
                

                break;
            default:
                break;
        }
    }
}
