using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YouWin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;

    private void Awake()
    {
        winText.text = "";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            winText.text = "You reached the end of the maze!";
            Invoke("ClearUI", 5f);
        }
    }

    void ClearUI()
    {
        winText.text = "";
    }
}
