using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject Player;
    public GameObject PauseMenu;

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
}
