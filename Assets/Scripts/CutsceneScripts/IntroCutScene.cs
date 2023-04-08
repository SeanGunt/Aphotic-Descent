using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroCutScene : MonoBehaviour
{
   public GameObject aSyncLoaderGameObject;
   private ASyncLoader aSyncLoader;
    private VideoPlayer videoPlayer;
    double videoLength;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.length;
        aSyncLoader = aSyncLoaderGameObject.GetComponent<ASyncLoader>();

    }

    private void Update()
    {
        videoLength -= Time.deltaTime;
        if (videoLength <= 0)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
            aSyncLoader.LoadLevelBtn("VerticalSlice");
        }

        if(Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            Time.timeScale = 1f;
            //SceneManager.LoadScene("VerticalSlice");
             aSyncLoader.LoadLevelBtn("VerticalSlice");
        }
        else if (Gamepad.current != null && (Gamepad.current.aButton.isPressed || Gamepad.current.startButton.isPressed))
        {
            Time.timeScale = 1f;
            aSyncLoader.LoadLevelBtn("VerticalSlice");
        }
    }
}
