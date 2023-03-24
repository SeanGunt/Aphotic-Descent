using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Vector3 flashlightPosition;
    public Quaternion playerRotation;
    public int invisCharges;
    public int boxes;
    public int objectiveId;
    public int numOfZooplanktonCollected;
    public bool knifeHasBeenPickedUp;
    public bool hasUpgradedSuit;
    public bool flashlightHasBeenPickedUp;
    public bool doorKey;
    public bool secondDoorOpened;
    public bool kelpMazeObjectiveTriggerd;
    public bool kelpMazeEndTriggered;
    public bool labStartObjectiveTriggered;
    public bool invisibilityAcquired;
    public bool ridgeObjectiveTriggered;
    public bool inSub;
    public bool inLab;
    public bool inKelpMaze;
    public bool inEelCave;
    public bool inPsShrimpCave;
    public bool inMudMarsh;
    public bool inAnglerTrench;
    public bool bathysphereCutscenePlayed;
    public bool eelIsDead;
    public bool eelObjectiveTriggered;
    public SerializableDictionary<string, bool> lilGuygsCollected;
    public SerializableDictionary<string, bool> loreCollected;

    public GameData()
    {
        playerPosition = new Vector3(-60.7f,-56.4f,-89.6f);
        playerRotation = new Quaternion(0,0,0,0);
        flashlightPosition = new Vector3(-60.68f, -55.55f, -89.46f);
        invisCharges = 3;
        knifeHasBeenPickedUp = false;
        hasUpgradedSuit = false;
        flashlightHasBeenPickedUp = false;
        invisibilityAcquired = false;
        doorKey = false;
        secondDoorOpened = false;
        kelpMazeObjectiveTriggerd = false;
        kelpMazeEndTriggered = false;
        labStartObjectiveTriggered = false;
        eelObjectiveTriggered = false;
        ridgeObjectiveTriggered = false;
        inAnglerTrench = false;
        inMudMarsh = false;
        inPsShrimpCave = false;
        inEelCave = false;
        inLab = false;
        inKelpMaze = false;
        inSub = true;
        eelIsDead = false;
        boxes = 4;
        objectiveId = 0;
        numOfZooplanktonCollected = 0;
        lilGuygsCollected = new SerializableDictionary<string, bool>();
        loreCollected = new SerializableDictionary<string, bool>();
        bathysphereCutscenePlayed = false;
    }
}
