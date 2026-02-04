using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, optionsMenu, howToMenu, creditsMenu, rebindingMenu;

    [SerializeField] GameObject optionsFirstButton, optionsClosedButton, howToFirstButton, howToClosedButton, creditsFirstButton, creditsClosedButton, rebindingFirstButton, rebindingClosedButton, currentImg, newImg;
    [SerializeField] Button continueButton;
    private PlayerInputActions playerInputActions;

    private bool otherControlsActive;

    public void Awake()
    {
        //Debug.Log(Application.persistentDataPath);
        string fileToCheck = Path.Combine(Application.persistentDataPath, "Save");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
		//this might set the game to windowed 1920 x 1080 by default.
		if (Screen.fullScreenMode != FullScreenMode.Windowed)
        {
			Screen.fullScreenMode = FullScreenMode.Windowed;
			Screen.SetResolution(1920, 1080, false);
		}
		playerInputActions = new PlayerInputActions();
        otherControlsActive = false;
        if (File.Exists(fileToCheck))
        {
            Debug.Log("Save file exists");
            continueButton.interactable = true;
        }
        else
        {
            Debug.Log("File does not exist");
            continueButton.interactable = false;
            //continueButton.
            //continueButton.colors.normalColor.a = .5f ;
            //this is where the continue button would get greyed out.
        }
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.DeleteData();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene("IntroCutScene");
    }

    public void PlayWhiteboxMaze ()
    {
        Time.timeScale = 1;
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene(1);
    }
    
    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);

    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }

    public void OpenHowTo()
    {
        mainMenu.SetActive(false);
        howToMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(howToFirstButton);
    }

    public void SwitchHowToImg()
    {
        if (!otherControlsActive)
        {
            currentImg.gameObject.SetActive(false);
            newImg.gameObject.SetActive(true);
            otherControlsActive = true;
        }
        else
        {
            currentImg.gameObject.SetActive(true);
            newImg.gameObject.SetActive(false);
            otherControlsActive = false;
        }
    }

    public void CloseHowTo()
    {
        howToMenu.SetActive(false);
        mainMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(howToClosedButton);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(creditsClosedButton);
    }

    public void OpenCreditsScene()
    {
        SceneManager.LoadScene("CreditsMM");
    }

    public void OpenRebinding()
    {
        rebindingMenu.SetActive(true);
        optionsMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingFirstButton);
    }

    public void CloseRebinding()
    {
        rebindingMenu.SetActive(false);
        optionsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingClosedButton);
    }

    public void QuitGame ()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
