using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    double videoLength = 62f;
    bool hasStarted;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength < 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScreen");
        }

        if((Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed) && videoLength < 58f)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScreen");
        }
        else if (Gamepad.current != null && (Gamepad.current.aButton.isPressed || Gamepad.current.startButton.isPressed))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("WinScreen");
        }
    }
}
