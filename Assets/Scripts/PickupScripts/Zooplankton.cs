using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zooplankton : MonoBehaviour, IDataPersistence
{   [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject basicTextObj;
    private Text basicText;
    private string zooplanktonCollectedText = "Zooplankton Collected! Added to Inventory";
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string id;
    [SerializeField] private bool collected;
    private AudioSource zooplanktonAudioSource;
    [SerializeField] private AudioClip[] zooplanktonNoises;
    private float zooplanktonNoiseTimer;
    [ContextMenu("Generate Guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        zooplanktonNoiseTimer = Random.Range(2.0f, 5.0f);
        zooplanktonAudioSource = GetComponent<AudioSource>();
        zooplanktonAudioSource.PlayOneShot(zooplanktonNoises[0]);
        basicText = basicTextObj.GetComponent<Text>();
    }

    public void LoadData(GameData data)
    {
        data.lilGuygsCollected.TryGetValue(id, out collected);
        if(collected)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        zooplanktonNoiseTimer -= Time.deltaTime;
        if (zooplanktonNoiseTimer <= 0)
        {
            int randomNoise = Random.Range(0,3);
            zooplanktonAudioSource.PlayOneShot(zooplanktonNoises[randomNoise]);
            zooplanktonNoiseTimer = Random.Range(2.0f, 5.0f);
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
        basicText.text = zooplanktonCollectedText;
        basicTextObj.SetActive(true);
        audioSource.PlayOneShot(pickupSound);
        GameDataHolder.numOfZooplanktonCollected++;
        collected = true;
        this.gameObject.SetActive(false);
    }
}

