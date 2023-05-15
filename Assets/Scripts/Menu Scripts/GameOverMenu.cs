using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameObject gameOverFirstButton;
    private GameObject Player;
    private PlayerInputActions playerInputActions;
    private InputAction escape;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInputActions = new PlayerInputActions();
    }
    
    void Start()
    {
        OpenGameOver();
    }

    public void OpenGameOver()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
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
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
