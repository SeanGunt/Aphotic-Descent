using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreSaver : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [SerializeField] private bool collected;
    [SerializeField] private TextMeshProUGUI loreText;
    [SerializeField] private string loreTextString;
    [ContextMenu("Generate Guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        data.loreCollected.TryGetValue(id, out collected);
        if(collected)
        {
            this.gameObject.SetActive(false);
            loreText.text = loreTextString;
        }
        else
        {
            loreText.text = "????";
        }
    }

    public void SaveData(GameData data)
    {
        if (data.loreCollected.ContainsKey(id))
        {
            data.loreCollected.Remove(id);
        }
        data.loreCollected.Add(id, collected);
    }

    public void CollectLore()
    {
        loreText.text = loreTextString;
        collected = true;
    }
}
