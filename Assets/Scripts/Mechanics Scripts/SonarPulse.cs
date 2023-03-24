using UnityEngine;
using System.Collections.Generic;

public class SonarPulse : MonoBehaviour
{
    [SerializeField]private Transform pulseTransform;
    private float range;
    private float rangeMax;

    private List<Collider> alreadyHitColliders;

    private void Awake()
    {
        rangeMax = 300f;
    }

    private void Update()
    {
        float rangeSpeed = 150f;
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
        }
        pulseTransform.localScale = new Vector3(range, range);

        
    }
}
