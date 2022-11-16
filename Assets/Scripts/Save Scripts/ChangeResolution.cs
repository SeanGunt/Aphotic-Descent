using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeResolution : MonoBehaviour
{
    private float resX, resY;
    private int resWidth, resHeight;
    private int firstPlayInt = 0;
    [SerializeField] private RenderTexture psxTexture;

    private void Awake()
    {
        firstPlayInt = PlayerPrefs.GetInt("First Play Menu");

        if (firstPlayInt == 0)
        {
            resX = 11.5f;
            resY = 10.0f;
            this.transform.localScale = new Vector3(resX, resY, 0);
            psxTexture.Release();
            psxTexture.height = 496;
            psxTexture.width = 512;
            psxTexture.Create();

            PlayerPrefs.SetFloat("ResX", resX);
            PlayerPrefs.SetFloat("ResY", resY);
            PlayerPrefs.SetInt("ResHeight", 496);
            PlayerPrefs.SetInt("ResWidth", 512);
            PlayerPrefs.SetInt("First Play Menu", -1);
        }
        else
        {
            resHeight = PlayerPrefs.GetInt("ResHeight", resHeight);
            resWidth = PlayerPrefs.GetInt("ResWidth", resWidth);
            resX = PlayerPrefs.GetFloat("ResX", resX);
            resY = PlayerPrefs.GetFloat("ResY", resY);
            this.transform.localScale = new Vector3(resX, resY, 0);
            psxTexture.Release();
            psxTexture.height = resHeight;
            psxTexture.width = resWidth;
            psxTexture.Create();
        }
    }

    public void Resolution16x9()
    {
        resX = 17.78f;
        resY = 10f;

        psxTexture.Release();
        psxTexture.height = 270;
        psxTexture.width = 480;

        PlayerPrefs.SetInt("ResHeight", resHeight);
        PlayerPrefs.SetInt("ResWidth", resWidth);

        PlayerPrefs.SetFloat("ResX", resX);
        PlayerPrefs.SetFloat("ResY", resY);
        psxTexture.Create();
        this.transform.localScale = new Vector3(resX, resY, 0);
        PlayerPrefs.Save();
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            return;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Resolution4x3()
    {
        resX = 11.5f;
        resY = 10.0f;

        psxTexture.Release();
        psxTexture.height = 496;
        psxTexture.width = 512;

        PlayerPrefs.SetInt("ResHeight", resHeight);
        PlayerPrefs.SetInt("ResWidth", resWidth);

        PlayerPrefs.SetFloat("ResX", resX);
        PlayerPrefs.SetFloat("ResY", resY);
        psxTexture.Create();
        this.transform.localScale = new Vector3(resX, resY, 0);
        PlayerPrefs.Save();
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            return;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
