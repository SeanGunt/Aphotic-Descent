using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisabler : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint2, checkpoint3, checkpoint4, checkpoint5, checkpoint6, checkpoint8,
    checkpoint10, checkpoint11;

    private void Start()
    {
        CheckObjectivesTriggered(checkpoint2, GameDataHolder.kelpMazeObjectiveTriggerd);
        CheckObjectivesTriggered(checkpoint3, GameDataHolder.kelpMazeEndTriggered);
        CheckObjectivesTriggered(checkpoint4, GameDataHolder.labStartObjectiveTriggered);
        CheckObjectivesTriggered(checkpoint5, GameDataHolder.eelObjectiveTriggered);
        CheckObjectivesTriggered(checkpoint6, GameDataHolder.ridgeObjectiveTriggered);
        CheckObjectivesTriggered(checkpoint8, GameDataHolder.pistolShrimpObjectiveTriggered);
        CheckObjectivesTriggered(checkpoint10, GameDataHolder.marshObjectiveTriggered);
        CheckObjectivesTriggered(checkpoint11, GameDataHolder.trenchObjectiveTriggered);
    }
    private void CheckObjectivesTriggered(GameObject checkpoint, bool triggered)
    {
        if(triggered)
        {
            checkpoint.SetActive(false);
        }
    }
}

