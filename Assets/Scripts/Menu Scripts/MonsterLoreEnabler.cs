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

        if (index == 3)
        {
            GameDataHolder.eelFound = true;
        }

        if (index == 4)
        {
            GameDataHolder.hermitcrabFound = true;
        }

        if (index == 5)
        {
            GameDataHolder.pistolshrimpFound = true;
        }

        if (index == 6)
        {
            GameDataHolder.shrimpmanFound = true;
        }

        if (index == 7)
        {
            GameDataHolder.anglerFound = true;
        }
    }
}
