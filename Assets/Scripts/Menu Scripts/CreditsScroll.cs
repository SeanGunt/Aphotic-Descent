using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float scrollSpeed;
    float yPos;

    private void Awake()
    {
        startPosition = this.transform.position;
    }

    private void OnEnable()
    {
        ResetPositions();
    }

    private void OnDisable()
    {
        ResetPositions();
    }

    private void Update()
    {
        yPos += Time.deltaTime * scrollSpeed;
        this.transform.position = new Vector3(0, yPos, 0);
    }

    private void ResetPositions()
    {
        this.transform.position = startPosition;
        yPos = startPosition.y;
    }
}
