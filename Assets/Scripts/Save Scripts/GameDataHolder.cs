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
    public static int subParts;
    public static bool doorKey;
    public static bool secondDoorOpened;
    public static bool kelpMazeObjectiveTriggerd;
    public static bool kelpMazeEndTriggered;
    public static bool labStartObjectiveTriggered;
    public static bool eelObjectiveTriggered;
    public static bool eelObjective2Triggered;
    public static bool ridgeObjectiveTriggered;
    public static bool hermitCaveObjectiveTriggered;
    public static bool pistolShrimpObjectiveTriggered;
    public static bool biolampsObjectivetriggered;
    public static bool marshObjectiveTriggered;
    public static bool trenchObjectiveTriggered;
    public static bool inSub;
    public static bool inLab;
    public static bool inKelpMaze;
    public static bool inEelCave;
    public static bool inPsShrimpCave;
    public static bool inMudMarsh;
    public static bool inAnglerTrench;
    public static bool eelIsDead;
    public static bool freakfishFound;
    public static bool zooplanktonFound;
    public static bool eelFound;
    public static bool hermitcrabFound;
    public static bool pistolshrimpFound;
    public static bool shrimpmanFound;
    public static bool anglerFound;
    public static int objectiveId;
    public static int numOfZooplanktonCollected;
    public static int biolampsAlive;
    public static bool bathysphereCutscenePlayed;
    public static bool hintCamPlayed;
    public static bool zooplanktonCutscenePlayed;

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
        subParts = data.subParts;

        kelpMazeObjectiveTriggerd = data.kelpMazeObjectiveTriggerd;
        kelpMazeEndTriggered = data.kelpMazeEndTriggered;
        labStartObjectiveTriggered = data.labStartObjectiveTriggered;
        eelObjectiveTriggered = data.eelObjectiveTriggered;
        eelObjective2Triggered = data.eelObjective2Triggered;
        ridgeObjectiveTriggered = data.ridgeObjectiveTriggered;
        hermitCaveObjectiveTriggered = data.hermitCaveObjectiveTriggered;
        pistolShrimpObjectiveTriggered = data.pistolShrimpObjectiveTriggered;
        biolampsObjectivetriggered = data.biolampsObjectivetriggered;
        marshObjectiveTriggered = data.marshObjectiveTriggered;
        trenchObjectiveTriggered = data.trenchObjectiveTriggered;

        inAnglerTrench = data.inAnglerTrench;
        inMudMarsh = data.inMudMarsh;
        inPsShrimpCave = data.inPsShrimpCave;
        inEelCave = data.inEelCave;
        inLab = data.inLab;
        inKelpMaze = data.inKelpMaze;
        inSub = data.inSub;

        eelIsDead = data.eelIsDead;

        bathysphereCutscenePlayed = data.bathysphereCutscenePlayed;
        hintCamPlayed = data.hintCamPlayed;
        zooplanktonCutscenePlayed = data.zooplanktonCutscenePlayed;

        numOfZooplanktonCollected = data.numOfZooplanktonCollected;

        biolampsAlive = data.biolampsAlive;

        freakfishFound = data.freakfishFound;
        zooplanktonFound = data.zooplanktonFound;
        eelFound = data.eelFound;
        hermitcrabFound = data.hermitcrabFound;
        pistolshrimpFound = data.pistolshrimpFound;
        shrimpmanFound = data.shrimpmanFound;
        anglerFound = data.anglerFound;
    }

    public void SaveData(GameData data)
    {
        data.knifeHasBeenPickedUp = knifeHasBeenPickedUp;
        data.flashlightHasBeenPickedUp = flashlightHasBeenPickedUp;
        data.invisibilityAcquired = invisibilityAcquired;
        data.hasUpgradedSuit = hasUpgradedSuit;

        data.boxes = boxes;
        data.subParts = subParts;
        data.doorKey = doorKey;
        data.secondDoorOpened = secondDoorOpened;
        data.objectiveId = objectiveId;

        data.kelpMazeObjectiveTriggerd = kelpMazeObjectiveTriggerd;
        data.kelpMazeEndTriggered = kelpMazeEndTriggered;
        data.labStartObjectiveTriggered = labStartObjectiveTriggered;
        data.eelObjectiveTriggered = eelObjectiveTriggered;
        data.eelObjective2Triggered = eelObjective2Triggered;
        data.ridgeObjectiveTriggered = ridgeObjectiveTriggered;
        data.hermitCaveObjectiveTriggered = hermitCaveObjectiveTriggered;
        data.pistolShrimpObjectiveTriggered = pistolShrimpObjectiveTriggered;
        data.biolampsObjectivetriggered = biolampsObjectivetriggered;
        data.marshObjectiveTriggered = marshObjectiveTriggered;
        data.trenchObjectiveTriggered = trenchObjectiveTriggered;

        data.inAnglerTrench = inAnglerTrench;
        data.inMudMarsh = inMudMarsh;
        data.inPsShrimpCave = inPsShrimpCave;
        data.inEelCave = inEelCave;
        data.inLab = inLab;
        data.inKelpMaze = inKelpMaze;
        data.inSub = inSub;

        data.eelIsDead = eelIsDead;

        data.bathysphereCutscenePlayed = bathysphereCutscenePlayed;
        data.hintCamPlayed = hintCamPlayed;
        data.zooplanktonCutscenePlayed = zooplanktonCutscenePlayed;

        data.numOfZooplanktonCollected = numOfZooplanktonCollected;

        data.biolampsAlive = biolampsAlive;

        data.freakfishFound = freakfishFound;
        data.zooplanktonFound = zooplanktonFound;
        data.eelFound = eelFound;
        data.hermitcrabFound = hermitcrabFound;
        data.pistolshrimpFound = pistolshrimpFound;
        data.shrimpmanFound = shrimpmanFound;
        data.anglerFound = anglerFound;
    }
}
