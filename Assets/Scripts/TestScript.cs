using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        GameDataHolder.knifeHasBeenPickedUp = true;
        GameDataHolder.flashlightHasBeenPickedUp = true;
        GameDataHolder.invisibilityAcquired = true;
        GameDataHolder.hasUpgradedSuit = true;
    }
}
