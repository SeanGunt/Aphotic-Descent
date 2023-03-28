using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GivePlayerEverything : MonoBehaviour
{
    private void Update()
    {
        if(Keyboard.current.upArrowKey.isPressed && Keyboard.current.downArrowKey.isPressed && Keyboard.current.leftArrowKey.isPressed && Keyboard.current.rightArrowKey.isPressed)
        {
            GameDataHolder.doorKey = true;
            GameDataHolder.flashlightHasBeenPickedUp = true;
            GameDataHolder.knifeHasBeenPickedUp = true;
            GameDataHolder.invisibilityAcquired = true;
            GameDataHolder.hasUpgradedSuit = true;
        }
    }
}
