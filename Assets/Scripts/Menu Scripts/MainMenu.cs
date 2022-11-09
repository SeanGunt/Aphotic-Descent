using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu, optionsMenu, howToMenu, levelSelect;

    [SerializeField] GameObject optionsFirstButton, optionsClosedButton, howToFirstButton, howToClosedButton, levelSelectFirstButton, levelSelectClosedButton;

    public void PlayWhiteboxMaze ()
    {
        Time.timeScale = 1;
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene(1);
    }

    public void OpenLevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(levelSelectFirstButton);
    }

    public void ClosedLevelSelect()
    {
        levelSelect.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(levelSelectClosedButton);
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

    public void CloseHowTo()
    {
        howToMenu.SetActive(false);
        mainMenu.SetActive(true);

        //clears selected button
        EventSystem.current.SetSelectedGameObject(null);

        //sets a new selected button
        EventSystem.current.SetSelectedGameObject(howToClosedButton);
    }

    public void QuitGame ()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
