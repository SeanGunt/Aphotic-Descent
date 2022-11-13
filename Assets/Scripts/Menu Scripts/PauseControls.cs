using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseControls : MonoBehaviour
{
    public GameObject PauseMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] GameObject Player;
    public bool paused, otherMenuActive;

    void Update()
    {
        if (!Player.GetComponent<PlayerHealthController>().gameOver && !otherMenuActive)
        {
            if (Input.GetButtonDown("Pause") && !paused)
            {
                PauseMenu.SetActive(true);
                gameUI.SetActive(false);
                paused = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetButtonDown("Pause") && paused)
            {
                PauseMenu.SetActive(false);
                gameUI.SetActive(true);
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
