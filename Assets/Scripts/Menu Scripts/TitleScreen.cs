using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private UnityEvent titleScreenEvent;
    void Start()
    {
        if (titleScreenEvent == null)
        {
            titleScreenEvent = new UnityEvent();
        }
        titleScreenEvent.AddListener(Ping);
    }
    void Update()
    {
        if ((Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed || Gamepad.current.aButton.isPressed) && titleScreenEvent != null)
        {
            titleScreenEvent.Invoke();
        }
    }

    void Ping()
    {
        Debug.Log("Ping");
    }
}
