using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuCredits : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    double videoLength = 62f;
    [SerializeField] private GameObject backButton;
    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength < 0)
        {
            videoLength = videoPlayer.length;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
