using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("SaveGame");
        }
    }

    private IEnumerator SaveGame()
    {
        yield return new WaitForSeconds(0.5f);
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Game Was Saved");
    }
}
