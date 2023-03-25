using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SonarPulse : MonoBehaviour
{
    [SerializeField]private Transform pulseTransform;
    private SpriteRenderer pulseSpriteRenderer;
    private Color pulseColor;

    private float range;
    private float rangeMax;
    private float fadeRange;

    private List<Collider> collidersHit;

    private void Awake()
    {
        rangeMax = 6f;
        fadeRange = 1.5f;
        collidersHit = new List<Collider>();
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        float rangeSpeed = .25f;
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
            collidersHit.Clear();
        }
        
        pulseTransform.localScale = new Vector3(range, range);
        Collider[] hitCollidersArray = Physics.OverlapSphere(pulseTransform.position, range * 100);
        foreach (Collider colliderHit in hitCollidersArray)
        {
            if (colliderHit != null && colliderHit.gameObject.GetComponent<SonarPingManager>())
            {
                if (!collidersHit.Contains(colliderHit))
                {
                    collidersHit.Add(colliderHit);
                    SonarPingManager sPManager = colliderHit.GetComponent<SonarPingManager>();
                    sPManager.InstantiatePings();
                    sPManager.SetPingTimer(rangeMax/rangeSpeed);
                }
            }
        }

        if (range > rangeMax - fadeRange)
        {
            pulseColor.a = Mathf.Lerp(0f, 1f, (rangeMax - range) / fadeRange);
        }
        else
        {
            pulseColor.a = 1f;
        }
        pulseSpriteRenderer.color = pulseColor;
        
    }
}
