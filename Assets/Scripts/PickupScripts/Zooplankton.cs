using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zooplankton : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [SerializeField] private bool collected;
    [ContextMenu("Generate Guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        data.lilGuygsCollected.TryGetValue(id, out collected);
        if(collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.lilGuygsCollected.ContainsKey(id))
        {
            data.lilGuygsCollected.Remove(id);
        }
        data.lilGuygsCollected.Add(id, collected);
    }
    public void CollectPlankton()
    {
        GameDataHolder.numOfZooplanktonCollected++;
        collected = true;
        this.gameObject.SetActive(false);
    }
}

