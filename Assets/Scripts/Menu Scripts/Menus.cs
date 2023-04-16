using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private PauseControls playerPauseControls;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject RebindingMenu;
    private PlayerInput playerInput;
    public Volume volume;
    [SerializeField] private GameObject gameUI, settingsFirstButton, settingsClosedButton, objectiveTextObj, rebindingFirstButton, rebindingClosedButton, overlay1, overlay2, overlay3, overlay4, overlay5, overlay6;
    

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
    }
    
    public void OpenSettings()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        playerPauseControls.otherMenuActive = true;
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
        BGMManager.instance.EndPause();
        PauseMenu.SetActive(false);
        gameUI.SetActive(true);
        objectiveTextObj.SetActive(true);
        playerInput.currentActionMap.Enable();
        playerPauseControls.paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerPauseControls.UnpauseTheGame();
    }

    public void OpenRemappingMenu()
    {
        SettingsMenu.SetActive(false);
        RebindingMenu.SetActive(true);
        playerPauseControls.otherMenuActive = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingFirstButton);
    }

    public void CloseRemappingMenu()
    {
        SettingsMenu.SetActive(true);
        RebindingMenu.SetActive(false);
        playerPauseControls.otherMenuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(rebindingClosedButton);
    }

    public void MenuShift(GameObject newButton)
    {
        if (!newButton.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(newButton);
        }
        else
        {
            return;
        }
        
    }

    public void ResetMenus()
    {
        overlay1.SetActive(true);
        overlay2.SetActive(false);
        overlay3.SetActive(false);
        overlay4.SetActive(false);
        overlay5.SetActive(false);
        overlay6.SetActive(false);
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
