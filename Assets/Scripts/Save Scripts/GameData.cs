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
    public bool knifeHasBeenPickedUp;
    public bool hasUpgradedSuit;
    public bool flashlightHasBeenPickedUp;
    public bool doorKey;
    public bool secondDoorOpened;
    public bool kelpMazeObjectiveTriggerd;
    public bool kelpMazeEndTriggered;
    public bool labStartObjectiveTriggered;
    public bool invisibilityAcquired;

    public GameData()
    {
        playerPosition = new Vector3(-60f,-20.5f,-90f);
        playerRotation = new Quaternion(0,0,0,0);
        flashlightPosition = new Vector3(-59.75f, -20.14f, -89.62f);
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
        boxes = 4;
        objectiveId = 0;
    }
}
