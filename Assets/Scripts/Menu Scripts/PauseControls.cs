using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseControls : MonoBehaviour
{
    public GameObject PauseMenu, pauseButton;
    private PlayerInput playerInput;
    [SerializeField] private GameObject gameUI, objectiveText, basicTextObj;
    [SerializeField] private Menus menuController;
    GameObject Player;
    public bool paused, otherMenuActive;
    public Volume volume;
    [SerializeField] Animator pauseAnim;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
        pauseAnim = Player.GetComponent<Animator>();
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
                BGMManager.instance.Pause();
                basicTextObj.SetActive(false);
                StartCoroutine(PauseAnimations());
                PauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(pauseButton);
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
                BGMManager.instance.EndPause();
                EventSystem.current.SetSelectedGameObject(null);
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

    IEnumerator PauseAnimations()
    {
        pauseAnim.SetBool("pauseArm", true);
        yield return new WaitForSeconds(0.5f);
    }
}
