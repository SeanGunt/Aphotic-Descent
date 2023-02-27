using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisabler : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint2, checkpoint3, checkpoint4, checkpoint5, checkpoint6;

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

        if (GameDataHolder.eelObjectiveTriggered)
        {
            checkpoint5.SetActive(false);
        }

        if(GameDataHolder.ridgeObjectiveTriggered)
        {
            checkpoint6.SetActive(false);
        }
    }
}
