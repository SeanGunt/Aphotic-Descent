using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SonarPulse : MonoBehaviour
{
    [SerializeField]private Transform pulseTransform;
    private SpriteRenderer pulseSpriteRenderer;
    private Color pulseColor;

    private float range, rangeMax, rangeSpeed, fadeRange, pingDelay;

    private List<Collider> collidersHit;

    private void Awake()
    {
        rangeMax = 15f;
        fadeRange = 1.5f;
        rangeSpeed = 1.5f;
        pingDelay = 1f;
        collidersHit = new List<Collider>();
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
            collidersHit.Clear();
        }
        
        pulseTransform.localScale = new Vector3(range, range);
        StartCoroutine(PingColliders());
        if (range > rangeMax - fadeRange)
        {
            pulseColor.a = Mathf.Lerp(0f, 1f, (rangeMax - range) / fadeRange);
        }
        else
        {
            pulseColor.a = 1f;
        }
        pulseSpriteRenderer.color = new Color(pulseSpriteRenderer.color.r, pulseSpriteRenderer.color.g, pulseSpriteRenderer.color.b, pulseColor.a);
        
    }

    IEnumerator PingColliders()
    {
        Collider[] hitCollidersArray = Physics.OverlapSphere(pulseTransform.position, range);
        foreach (Collider colliderHit in hitCollidersArray)
        {
            if (colliderHit != null && colliderHit.gameObject.GetComponent<SonarPingManager>())
            {
                if (!collidersHit.Contains(colliderHit))
                {
                    collidersHit.Add(colliderHit);
                    SonarPingManager sPManager = colliderHit.GetComponent<SonarPingManager>();
                    sPManager.InstantiatePings();
                    if (colliderHit.gameObject.layer == 24)
                    {
                        sPManager.ColorManager(1);
                    }
                    if (colliderHit.gameObject.layer == 10)
                    {
                        sPManager.ColorManager(2);
                    }
                    sPManager.SetPingTimer(rangeMax/rangeSpeed);
                }
            }
            yield return new WaitForSeconds(pingDelay);
        }
    }
}
