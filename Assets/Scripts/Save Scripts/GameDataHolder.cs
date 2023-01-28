using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHolder : MonoBehaviour, IDataPersistence
{
    public static bool knifeHasBeenPickedUp;
    public static bool hasUpgradedSuit;
    public static bool flashlightHasBeenPickedUp;
    public static bool invisibilityAcquired;
    public static int boxes;
    public static bool doorKey;
    public static bool secondDoorOpened;
    public static bool kelpMazeObjectiveTriggerd;
    public static bool kelpMazeEndTriggered;
    public static bool labStartObjectiveTriggered;
    public static bool eelObjectiveTriggered;
    public static bool inLab;
    public static int objectiveId;

    public void LoadData(GameData data)
    {
        knifeHasBeenPickedUp = data.knifeHasBeenPickedUp;
        flashlightHasBeenPickedUp = data.flashlightHasBeenPickedUp;
        invisibilityAcquired = data.invisibilityAcquired;
        hasUpgradedSuit = data.hasUpgradedSuit;
        
        boxes = data.boxes;
        doorKey = data.doorKey;
        secondDoorOpened = data.secondDoorOpened;
        objectiveId = data.objectiveId;

        kelpMazeObjectiveTriggerd = data.kelpMazeObjectiveTriggerd;
        kelpMazeEndTriggered = data.kelpMazeEndTriggered;
        labStartObjectiveTriggered = data.labStartObjectiveTriggered;
        eelObjectiveTriggered = data.eelObjectiveTriggered;
        inLab = data.inLab;
    }

    public void SaveData(GameData data)
    {
        data.knifeHasBeenPickedUp = knifeHasBeenPickedUp;
        data.flashlightHasBeenPickedUp = flashlightHasBeenPickedUp;
        data.invisibilityAcquired = invisibilityAcquired;
        data.hasUpgradedSuit = hasUpgradedSuit;

        data.boxes = boxes;
        data.doorKey = doorKey;
        data.secondDoorOpened = secondDoorOpened;
        data.objectiveId = objectiveId;

        data.kelpMazeObjectiveTriggerd = kelpMazeObjectiveTriggerd;
        data.kelpMazeEndTriggered = kelpMazeEndTriggered;
        data.labStartObjectiveTriggered = labStartObjectiveTriggered;
        data.eelObjectiveTriggered = eelObjectiveTriggered;
        data.inLab = inLab;
    }
}
