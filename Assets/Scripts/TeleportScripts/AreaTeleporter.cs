using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaTeleporter : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Transform[] positions;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(Keyboard.current.digit1Key.isPressed)
        {
            Teleport(0);
            GameDataHolder.inSub = false;
            GameDataHolder.inKelpMaze = true;
            GameDataHolder.inLab = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inPsShrimpCave = false;
            GameDataHolder.inAnglerTrench = false;
        }

        if(Keyboard.current.digit2Key.isPressed)
        {
            Teleport(1);
            GameDataHolder.inSub = false;
            GameDataHolder.inLab = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inPsShrimpCave = false;
            GameDataHolder.inAnglerTrench = false;
        }

        if(Keyboard.current.digit3Key.isPressed)
        {
            Teleport(2);
            GameDataHolder.inSub = false;
            GameDataHolder.inEelCave = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inLab = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inPsShrimpCave = false;
            GameDataHolder.inAnglerTrench = false;
        }

        if(Keyboard.current.digit4Key.isPressed)
        {
            Teleport(3);
            GameDataHolder.inSub = false;
            GameDataHolder.inMudMarsh = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inLab = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inPsShrimpCave = false;
            GameDataHolder.inAnglerTrench = false;
        }

        if(Keyboard.current.digit5Key.isPressed)
        {
            Teleport(4);
            GameDataHolder.inSub = false;
            GameDataHolder.inPsShrimpCave = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inLab = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inAnglerTrench = false;
        }

        if(Keyboard.current.digit6Key.isPressed)
        {
            Teleport(5);
            GameDataHolder.inSub = false;
            GameDataHolder.inAnglerTrench = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inLab = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inPsShrimpCave = false;
        }
    }

    private void Teleport(int arrayPosition)
    {
        player.transform.localPosition = positions[arrayPosition].position;
    }
}
