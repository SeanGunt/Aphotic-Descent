using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMudMarshTeleport : MonoBehaviour
{
    [SerializeField] private GameObject mudMarshTeleport;
    private void Update()
    {
        if (GameDataHolder.biolampsAlive <= 0)
        {
            mudMarshTeleport.SetActive(true);
        }
        else
        {
            mudMarshTeleport.SetActive(false);
        }
    }
}
