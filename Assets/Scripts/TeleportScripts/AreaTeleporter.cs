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
        }

        if(Keyboard.current.digit2Key.isPressed)
        {
            Teleport(1);
        }

        if(Keyboard.current.digit3Key.isPressed)
        {
            Teleport(2);
        }

        if(Keyboard.current.digit4Key.isPressed)
        {
            Teleport(3);
        }

        if(Keyboard.current.digit5Key.isPressed)
        {
            Teleport(4);
        }

        if(Keyboard.current.digit6Key.isPressed)
        {
            Teleport(5);
        }

        if(Keyboard.current.digit7Key.isPressed)
        {
            Teleport(6);
        }
    }

    private void Teleport(int arrayPosition)
    {
        player.transform.localPosition = positions[arrayPosition].position;
    }
}
