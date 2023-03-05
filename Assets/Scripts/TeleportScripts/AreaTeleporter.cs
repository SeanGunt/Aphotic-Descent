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
            GameDataHolder.inAnglerTrench = true;
            GameDataHolder.inKelpMaze = false;
            GameDataHolder.inLab = false;
            GameDataHolder.inEelCave = false;
            GameDataHolder.inMudMarsh = false;
            GameDataHolder.inPsShrimpCave = false;
        }

        // if(Keyboard.current.digit7Key.isPressed)
        // {
        //     Teleport(6);
        //     GameDataHolder.inAnglerTrench = true;
        // }
    }

    private void Teleport(int arrayPosition)
    {
        player.transform.localPosition = positions[arrayPosition].position;
    }
}
