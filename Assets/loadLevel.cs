using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
   public int iLevelToLoad;
   public int sLevelToLoad;
   public bool useIntigerToLoadLevel = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject collissionGameObject = collision.gameObject;

        if (collissionGameObject.name == "Player")
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        if(useIntigerToLoadLevel)
        {
            SceneManager.LoadScene(iLevelToLoad);
        }
        else
        {
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
}
