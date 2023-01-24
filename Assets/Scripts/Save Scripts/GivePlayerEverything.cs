using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerEverything : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameDataHolder.flashlightHasBeenPickedUp = true;
            GameDataHolder.knifeHasBeenPickedUp = true;
            GameDataHolder.invisibilityAcquired = true;
            GameDataHolder.hasUpgradedSuit = true;
        }
    }
}
