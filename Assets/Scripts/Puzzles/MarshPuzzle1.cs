using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshPuzzle1 : MonoBehaviour
{
    public int ZPFreed;
    [SerializeField] private GameObject shrimp1, shrimp2, shrimpBlocker;
    private MarshPuzzle1 scriptFunctionality;

    private void Awake()
    {
        ZPFreed = 0;
        scriptFunctionality = GetComponent<MarshPuzzle1>();
    }
    
    private void Update()
    {
        if (ZPFreed == 3)
        {
            shrimp1.gameObject.SetActive(false);
            shrimp2.gameObject.SetActive(true);
            shrimpBlocker.gameObject.SetActive(false);
            scriptFunctionality.enabled = false;
        }
    }
}
