using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayCodeScene ()
    {
        SceneManager.LoadScene(1);
        DataPersistenceManager.instance.LoadGame();
    }

    public void PlayArtScene ()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
