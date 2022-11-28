using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisabler : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint2, checkpoint3, checkpoint4;

    private void Start()
    {
        if(GameDataHolder.kelpMazeObjectiveTriggerd)
        {
            checkpoint2.SetActive(false);
        }

        if (GameDataHolder.kelpMazeEndTriggered)
        {
            checkpoint3.SetActive(false);
        }

        if (GameDataHolder.labStartObjectiveTriggered)
        {
            checkpoint4.SetActive(false);
        }
    }
}
