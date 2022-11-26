using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseControls : MonoBehaviour
{
    public GameObject PauseMenu;
    [SerializeField] private GameObject gameUI, objectiveText;
    [SerializeField] GameObject Player;
    public bool paused, otherMenuActive;
    public Volume volume;

    void Update()
    {
        if (!Player.GetComponent<PlayerHealthController>().gameOver && !otherMenuActive)
        {
            if (Input.GetButtonDown("Pause") && !paused)
            {
                DepthOfField depthOfField;
                if (volume.profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = true;
                }
                PauseMenu.SetActive(true);
                gameUI.SetActive(false);
                objectiveText.SetActive(false);
                paused = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetButtonDown("Pause") && paused)
            {
                DepthOfField depthOfField;
                if (volume.profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                }
                PauseMenu.SetActive(false);
                gameUI.SetActive(true);
                objectiveText.SetActive(true);
                paused = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (paused == false)
                {
                    PauseMenu.SetActive(false);
                }
            }
        }
    }
}
