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
}
