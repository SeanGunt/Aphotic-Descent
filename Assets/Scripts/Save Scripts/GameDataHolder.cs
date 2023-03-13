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
    public static bool ridgeObjectiveTriggered;
    public static bool inSub;
    public static bool inLab;
    public static bool inKelpMaze;
    public static bool inEelCave;
    public static bool inPsShrimpCave;
    public static bool inMudMarsh;
    public static bool inAnglerTrench;
    public static bool eelIsDead;
    public static int objectiveId;
    public static int numOfZooplanktonCollected;
    public static bool bathysphereCutscenePlayed;

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
        ridgeObjectiveTriggered = data.ridgeObjectiveTriggered;

        
        inAnglerTrench = data.inAnglerTrench;
        inMudMarsh = data.inMudMarsh;
        inPsShrimpCave = data.inPsShrimpCave;
        inEelCave = data.inEelCave;
        inLab = data.inLab;
        inKelpMaze = data.inKelpMaze;
        inSub = data.inSub;

        eelIsDead = data.eelIsDead;

        bathysphereCutscenePlayed = data.bathysphereCutscenePlayed;

        numOfZooplanktonCollected = data.numOfZooplanktonCollected;
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
        data.ridgeObjectiveTriggered = ridgeObjectiveTriggered;
        
        data.inAnglerTrench = inAnglerTrench;
        data.inMudMarsh = inMudMarsh;
        data.inPsShrimpCave = inPsShrimpCave;
        data.inEelCave = inEelCave;
        data.inLab = inLab;
        data.inKelpMaze = inKelpMaze;
        data.inSub = inSub;

        data.eelIsDead = eelIsDead;

        data.bathysphereCutscenePlayed = bathysphereCutscenePlayed;

        data.numOfZooplanktonCollected = numOfZooplanktonCollected;
    }
}
