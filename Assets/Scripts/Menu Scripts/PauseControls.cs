using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseControls : MonoBehaviour
{
    public GameObject PauseMenu;
    [SerializeField] GameObject Player;
    public bool paused;
      
    
    void Update()
    {
        if (!Player.GetComponent<PlayerHealthController>().gameOver)
        {
            if (Input.GetButtonDown("Pause") && paused == false)
            {
                PauseMenu.SetActive(true);
                paused = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetButtonDown("Pause") && paused == true)
            {
                PauseMenu.SetActive(false);
                paused = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                if (paused == false)
                {
                    PauseMenu.SetActive(false);
                }
            }
        }
    }
}
