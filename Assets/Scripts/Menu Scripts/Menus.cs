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

    public void PlayCodeScene ()
    {
        SceneManager.LoadScene(1);
        DataPersistenceManager.instance.LoadGame();
    }

    public void PlayArtScene ()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayWhiteboxMaze ()
    {
        SceneManager.LoadScene(3);
    }

    public void PlaySubmarine ()
    {
        SceneManager.LoadScene(4);
    }

    //public void PlayReef ()
    //{
        //SceneManager.LoadScene(5);
    //}
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
