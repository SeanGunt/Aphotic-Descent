using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public GameObject Player;
    public GameObject PauseMenu;

    public GameObject LevelSelect;

    public void OpenLevelSelect()
    {
        PauseMenu.SetActive(false);
        LevelSelect.SetActive(true);
    }

    public void BackButton()
    {
        PauseMenu.SetActive(true);
        LevelSelect.SetActive(false);
    }
    
    public void PlayWhiteboxMaze ()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
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
