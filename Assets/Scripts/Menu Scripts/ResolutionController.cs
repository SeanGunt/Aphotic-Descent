using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using Unity.Mathematics;
//using JetBrains.Annotations;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _borderlessToggle;
    private int _resLengthToCompare;
    private int _resolutionsLength;
    //private Toggle _fullscreenToggle;

    private List<Resolution> _validResolutions = new List<Resolution>();
    private bool _isBorderless;

    private FullScreenMode _currentScreenMode;
    //private bool _isFullScreen;

    private int _selectedResolutionIndex;

    private List<Resolution> _selectedResolutionsList = new List<Resolution>();

	private void Start()
    {
        _resLengthToCompare = _resolutionsLength;
        _isBorderless = true;
        _currentScreenMode = FullScreenMode.FullScreenWindow;
        //_isFullScreen = false;

        _validResolutions = Screen.resolutions.ToList();

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution res in _validResolutions)
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            float trueWidth = res.width;
            float trueHeight = res.height;
			float aRatio = trueWidth / trueHeight;
			//Debug.Log($"{trueWidth / trueHeight}");
            if (!resolutionStringList.Contains(newRes) && aRatio > 1.7f && aRatio < 1.8f)
            {
                resolutionStringList.Add(newRes);
                _selectedResolutionsList.Add(res);
            }
        }

        _resolutionDropdown.AddOptions(resolutionStringList);
        
		if (PlayerPrefs.HasKey("borderless"))
		{
			_borderlessToggle.isOn = PlayerPrefs.GetInt("borderless") == 1;
		}

		else
		{
			PlayerPrefs.SetInt("borderless", _borderlessToggle.isOn ? 1 : 0);
		}
		
        if (PlayerPrefs.HasKey("ResolutionIndex"))
		{
			_resolutionDropdown.value = (PlayerPrefs.GetInt("ResolutionIndex"));
		}
		else
		{
			_resolutionDropdown.value = 0;
			PlayerPrefs.SetInt("ResolutionIndex", _resolutionDropdown.value);
		}
	}

	private void Update()
	{
        _resolutionsLength = Screen.resolutions.Length;
        if (_resLengthToCompare != _resolutionsLength)
        {
            _resLengthToCompare = _resolutionsLength;
            UpdateResolutionsList();
        }
	}

    //might be a tad slow since I'm clearing like 3 lists every time the monitor's safe resolutions change, but this is for the greater good lol
    //basically does what I do at the start, but makes sure that the thing updates properly.
    private void UpdateResolutionsList()
    {
        _validResolutions.Clear();
        _resolutionDropdown.ClearOptions();
        _validResolutions = Screen.resolutions.ToList();
        _selectedResolutionsList.Clear();
		List<string> resolutionStringList = new List<string>();
		//string with to string of resolution it's checking used to avoid duplicates.
        string newRes;
		foreach (Resolution res in _validResolutions)
		{
			newRes = res.width.ToString() + " x " + res.height.ToString();
			float trueWidth = res.width;
			float trueHeight = res.height;
            float aRatio = trueWidth / trueHeight;
			if (!resolutionStringList.Contains(newRes) && aRatio > 1.7f)
			{
				resolutionStringList.Add(newRes);
				_selectedResolutionsList.Add(res);
			}
		}

		_resolutionDropdown.AddOptions(resolutionStringList);

		if (PlayerPrefs.HasKey("ResolutionIndex"))
		{
			_resolutionDropdown.value = (PlayerPrefs.GetInt("ResolutionIndex"));
		}
		else
		{
			_resolutionDropdown.value = 0;
			PlayerPrefs.SetInt("ResolutionIndex", _resolutionDropdown.value);
		}
	}

	//says what it does
    public void ChangeResolution()
    {
        _selectedResolutionIndex = _resolutionDropdown.value;
        Screen.SetResolution(_selectedResolutionsList[_selectedResolutionIndex].width, _selectedResolutionsList[_selectedResolutionIndex].height,_currentScreenMode);
    }
    
    //used to change between borderless and windowed, no fullscreen bc it's lame
    public void ChangeFullScreen()
    {
        _isBorderless = _borderlessToggle.isOn;
        if (_isBorderless)
        {
            _currentScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            _currentScreenMode = FullScreenMode.Windowed;
        }
		Screen.SetResolution(_selectedResolutionsList[_selectedResolutionIndex].width, _selectedResolutionsList[_selectedResolutionIndex].height, _currentScreenMode);

	}
    
    //deprecated functionality
    //public void ChangeResolution()
    //{
    //    switch (_resolutionDropdown.value)
    //    {
    //        case 0:
				//Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                
    //            break;
    //        case 1:
				//Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
				////Screen.fullScreenMode = FullScreenMode.Windowed;
                
    //            break;
    //        case 2:
				//Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
				////Screen.fullScreenMode = FullScreenMode.Windowed;
                

    //            break;
    //        case 3:
    //            Screen.SetResolution(1366, 768, FullScreenMode.Windowed);
    //            break;
    //        case 4:
    //            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
