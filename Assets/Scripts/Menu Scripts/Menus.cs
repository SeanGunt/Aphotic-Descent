using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    private GameObject Player;
    public GameObject PauseMenu;
    public GameObject LevelSelect;
    public GameObject SettingsMenu;
    public GameObject RebindingMenu;
    private PlayerInput playerInput;
    public Volume volume;
    [SerializeField] private GameObject gameUI, levelSelectFirstButton, levelSelectClosedButton, settingsFirstButton, settingsClosedButton, objectiveTextObj, rebindingFirstButton, rebindingClosedButton;
    

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
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

    public void ResumeGame()
    {
        DepthOfField depthOfField;
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            depthOfField.active = false;
        }
        PauseMenu.SetActive(false);
        gameUI.SetActive(true);
        objectiveTextObj.SetActive(true);
        playerInput.currentActionMap.Enable();
        Player.GetComponent<PauseControls>().paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenRemappingMenu()
    {
        SettingsMenu.SetActive(false);
        RebindingMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingFirstButton);
    }

    public void CloseRemappingMenu()
    {
        SettingsMenu.SetActive(true);
        RebindingMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingClosedButton);
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
