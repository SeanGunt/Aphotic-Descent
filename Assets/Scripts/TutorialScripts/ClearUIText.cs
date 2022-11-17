using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearUIText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutText;

    private void ClearUI()
    {
        tutText.text = "";
        this.gameObject.SetActive(false);
    }
}
