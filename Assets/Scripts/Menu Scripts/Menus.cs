using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menus : MonoBehaviour
{
    public GameObject Player;
    public GameObject PauseMenu;
    public GameObject LevelSelect;
    public GameObject SettingsMenu;
    [SerializeField] private GameObject gameUI, levelSelectFirstButton, levelSelectClosedButton, settingsFirstButton, settingsClosedButton;
    

    public void OpenLevelSelect()
    {
        PauseMenu.SetActive(false);
        LevelSelect.SetActive(true);
        Player.GetComponent<PauseControls>().otherMenuActive = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelSelectFirstButton);
    }

    public void CloseLevelSelect()
    {
        PauseMenu.SetActive(true);
        LevelSelect.SetActive(false);
        Player.GetComponent<PauseControls>().otherMenuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelSelectClosedButton);
    }

    public void OpenSettings()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        Player.GetComponent<PauseControls>().otherMenuActive = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void CloseSettings()
    {
        PauseMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Player.GetComponent<PauseControls>().otherMenuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsClosedButton);
    }
    
    public void PlayWhiteboxMaze ()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        gameUI.SetActive(true);
        Player.GetComponent<PauseControls>().paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
