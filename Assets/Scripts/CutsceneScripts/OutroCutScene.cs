using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OutroCutScene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField]private ASyncLoader sceneLoader;
    double videoLength = 62f;
    double lengthTillSkip;
    bool hasStarted;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
        lengthTillSkip = videoLength - 3f;
        
    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength < 0)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
            sceneLoader.LoadLevelBtn("VerticalSlice");
        }

        if((Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed) && videoLength < lengthTillSkip)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
            sceneLoader.LoadLevelBtn("VerticalSlice");
        }
        else if (Gamepad.current != null && (Gamepad.current.aButton.isPressed || Gamepad.current.startButton.isPressed))
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
            sceneLoader.LoadLevelBtn("VerticalSlice");
        }
    }
}
