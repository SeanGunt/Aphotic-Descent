using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField] private GameObject[] cutsceneObjects;

    private void Start()
    {
        if (GameDataHolder.bathysphereCutscenePlayed)
        {
            Destroy(cutsceneObjects[0]);
        }
    }
}
