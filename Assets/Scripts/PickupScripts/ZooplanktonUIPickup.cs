using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZooplanktonUIPickup : MonoBehaviour
{
  private TextMeshProUGUI zooText;
    // Start is called before the first frame update
    void Start()
    {
        zooText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateZooText(ZooplankotCount zooplankotCount)
    {
        zooText.text = zooplankotCount.NumberOfZooplankton.ToString();
        
    }
}
