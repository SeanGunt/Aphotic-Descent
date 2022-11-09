using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && titleScreenEvent != null)
        {
            titleScreenEvent.Invoke();
        }
    }

    void Ping()
    {
        Debug.Log("Ping");
    }
}
