using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpCutscene : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private GameObject freakFish;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, freakFish.transform.position);
    }
}
