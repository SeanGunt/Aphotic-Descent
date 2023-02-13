using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseControls : MonoBehaviour
{
    public GameObject PauseMenu;
    private PlayerInput playerInput;
    [SerializeField] private GameObject gameUI, objectiveText;
    GameObject Player;
    public bool paused, otherMenuActive;
    public Volume volume;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
    }
    
    void Update()
    {
        if (!Player.GetComponent<PlayerHealthController>().gameOver && !otherMenuActive)
        {
            if (playerInput.actions["Pause"].triggered && !paused && !LogPickup.logPickedUp && !Puzzle4UI.computerActivated)
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
                playerInput.currentActionMap.Disable();
                playerInput.actions["Pause"].Enable();
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if ((playerInput.actions["Pause"].triggered) && paused && !LogPickup.logPickedUp && !Puzzle4UI.computerActivated)
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
                playerInput.currentActionMap.Enable();
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
