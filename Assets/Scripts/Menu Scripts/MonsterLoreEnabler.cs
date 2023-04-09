using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoreEnabler : MonoBehaviour
{
    [SerializeField] private int index;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnableLore();
        }
    }

    private void EnableLore()
    {
        if (index == 1)
        {
            GameDataHolder.freakfishFound = true;
        }

        if (index == 2)
        {
            GameDataHolder.zooplanktonFound = true;
        }

        if (index == 4)
        {
            GameDataHolder.hermitcrabFound = true;
        }
    }
}
