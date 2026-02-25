using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrankDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene("EelCutScene");
        }
    }

    private void OnEnable()
    {
        //delay added here to make sure game saves properly before it auto transitions to the next scene.
        Invoke("GoToEelScene",2.5f);
       
    }

    private void GoToEelScene()
    {
		DataPersistenceManager.instance.SaveGame();
		SceneManager.LoadScene("EelCutScene");
	}
}
