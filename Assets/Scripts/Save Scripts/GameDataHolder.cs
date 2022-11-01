using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHolder : MonoBehaviour, IDataPersistence
{
    public static bool knifeHasBeenPickedUp;

    public void LoadData(GameData data)
    {
        knifeHasBeenPickedUp = data.knifeHasBeenPickedUp;
    }

    public void SaveData(GameData data)
    {
        data.knifeHasBeenPickedUp = knifeHasBeenPickedUp;
    }
}
