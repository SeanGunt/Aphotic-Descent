using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisabler : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint2;

    private void Start()
    {
        if(!GameDataHolder.kelpMazeObjectiveTriggerd) return;
        else
        {
            checkpoint2.SetActive(false);
        }
    }
}
