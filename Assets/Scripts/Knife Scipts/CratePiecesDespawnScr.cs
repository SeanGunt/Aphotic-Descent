using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePiecesDespawnScr : MonoBehaviour
{
    [SerializeField] private float destructionTimer;
    void Awake()
    {
        StartCoroutine(despawnTimer());
    }

    IEnumerator despawnTimer()
    {
        yield return new WaitForSeconds(destructionTimer);
        Object.Destroy(this.gameObject);
    }

}
