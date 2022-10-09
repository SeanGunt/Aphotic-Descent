using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHolder : MonoBehaviour, IDataPersistence
{
    public static GameDataHolder instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {

    }
}
