using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOverFirstButton, Player;

    private PlayerInputActions playerInputActions;
    private InputAction escape;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInputActions = InputManager.inputActions;
    }

    private void OnEnable()
    {
        escape = playerInputActions.PlayerControls.Escape;
        // escape.Enable();
    }

    private void OnDisable()
    {
        // escape.Disable();
    }
    
    void Start()
    {
        OpenGameOver();
    }

    void Update()
    {
        if (escape.triggered)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }

    public void OpenGameOver()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
        Debug.Log("OpenGameOverCalled");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetLevel()
    {
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Player.GetComponent<PlayerHealthController>().ResetHealth();
        Time.timeScale = 1;
        Debug.Log("ResetHealthCalled");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
