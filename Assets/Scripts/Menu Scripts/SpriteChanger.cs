using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite undiscoveredSprite, discoveredSprite;
    [SerializeField] private int index;
    [SerializeField] private Image image;

    private void Update()
    {
        ChangeSprite(0, GameDataHolder.freakfishFound);
        ChangeSprite(1, GameDataHolder.zooplanktonFound);
        ChangeSprite(2, GameDataHolder.eelFound);
        ChangeSprite(3, GameDataHolder.pistolshrimpFound);
        ChangeSprite(4, GameDataHolder.hermitcrabFound);
        ChangeSprite(5, GameDataHolder.shrimpmanFound);
        ChangeSprite(6, GameDataHolder.anglerFound);
    }

    private void ChangeSprite(int monsterNum, bool savedBool)
    {
        if (index == monsterNum && savedBool == true)
        {
            image.sprite = discoveredSprite;
        }
        else if (index == monsterNum && savedBool == false)
        {
            image.sprite = undiscoveredSprite;
        }
        else
        {
            return;
        }
    }
}
