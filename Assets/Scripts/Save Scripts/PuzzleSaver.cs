using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSaver : MonoBehaviour, IDataPersistence
{
    private bool completed;
    [SerializeField] private GameObject[] objectsToDestroy;
    [SerializeField] private string id;
    [ContextMenu("Generate Guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public void LoadData(GameData data)
    {
        data.puzzleCompleted.TryGetValue(id, out completed);
        if(completed)
        {
            foreach(GameObject objectToDestroy in objectsToDestroy)
            {
                Destroy(objectToDestroy);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (data.puzzleCompleted.ContainsKey(id))
        {
            data.puzzleCompleted.Remove(id);
        }
        data.puzzleCompleted.Add(id, completed);
    }

    public void CompletePuzzle()
    {
        completed = true;
        foreach(GameObject objectToDestroy in objectsToDestroy)
        {
            Destroy(objectToDestroy);
        }
    }
}
