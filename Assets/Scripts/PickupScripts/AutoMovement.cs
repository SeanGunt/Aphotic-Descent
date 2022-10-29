using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed, zSpeed, amplitude, frequency;
    [SerializeField] Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject();
        MoveObject();
    }

    void RotateObject()
    {
        transform.Rotate(
            xSpeed * Time.deltaTime,
            ySpeed * Time.deltaTime,
            zSpeed * Time.deltaTime
            );
    }

    void MoveObject()
    {
        transform.position = new Vector3(
            initialPos.x,
            Mathf.Sin(Time.time* frequency) * amplitude + initialPos.y,
            initialPos.z
        );
    }
}
