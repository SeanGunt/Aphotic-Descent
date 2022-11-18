using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHolder : MonoBehaviour, IDataPersistence
{
    public static bool knifeHasBeenPickedUp;
    public static bool flashlightHasBeenPickedUp;
    public static int boxes;
    public static bool doorKey;
    public static bool secondDoorOpened;

    public void LoadData(GameData data)
    {
        knifeHasBeenPickedUp = data.knifeHasBeenPickedUp;
        flashlightHasBeenPickedUp = data.flashlightHasBeenPickedUp;
        boxes = data.boxes;
        doorKey = data.doorKey;
        secondDoorOpened = data.secondDoorOpened;
    }

    public void SaveData(GameData data)
    {
        data.knifeHasBeenPickedUp = knifeHasBeenPickedUp;
        data.flashlightHasBeenPickedUp = flashlightHasBeenPickedUp;
        data.boxes = boxes;
        data.doorKey = doorKey;
        data.secondDoorOpened = secondDoorOpened;
    }
}
