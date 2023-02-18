using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZooplanktonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numOfZooplanktonCollectedText;
    private void Update()
    {
        numOfZooplanktonCollectedText.text = GameDataHolder.numOfZooplanktonCollected.ToString();
    }
}
