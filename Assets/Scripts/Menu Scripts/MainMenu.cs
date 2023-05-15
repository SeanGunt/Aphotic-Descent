using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, optionsMenu, howToMenu, creditsMenu, rebindingMenu;

    [SerializeField] GameObject optionsFirstButton, optionsClosedButton, howToFirstButton, howToClosedButton, creditsFirstButton, creditsClosedButton, rebindingFirstButton, rebindingClosedButton, currentImg, newImg;
    private PlayerInputActions playerInputActions;

    private bool otherControlsActive;

    public void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        playerInputActions = new PlayerInputActions();
        otherControlsActive = false;
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
