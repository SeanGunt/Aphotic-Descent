using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePiecesDespawnScr : MonoBehaviour
{

    void Awake()
    {
        StartCoroutine(despawnTimer());
    }

    IEnumerator despawnTimer()
    {
        yield return new WaitForSeconds(5);
        Object.Destroy(this.gameObject);
    }

}
