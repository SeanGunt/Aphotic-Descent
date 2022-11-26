using System.Collections;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private GameObject saveTextObj;
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
        saveTextObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveTextObj.SetActive(false);
    }
}
