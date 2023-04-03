using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SonarPulse : MonoBehaviour
{
    [SerializeField]private Transform pfSonarPing;
    [SerializeField]private Transform pulseTransform;
    private SpriteRenderer pulseSpriteRenderer;
    [SerializeField]private LayerMask pingLayers;
    private Color pulseColor;

    private float range, rangeMax, rangeSpeed, fadeRange, pingDelay, sphereRangeSpeed, sphereRange;

    private List<Collider> collidersHit;

    private void Awake()
    {
        rangeMax = 11.5f;
        fadeRange = 1.5f;
        rangeSpeed = 3.5f;
        sphereRangeSpeed = 7.8925f;
        pingDelay = 2.5f;
        collidersHit = new List<Collider>();
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        range += rangeSpeed * Time.deltaTime;
        sphereRange += sphereRangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
            sphereRange = 0f;
            collidersHit.Clear();
        }
        
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
        pulseTransform.localScale = new Vector3(range, range);
        Collider[] hitCollidersArray = Physics.OverlapSphere(pulseTransform.position, sphereRange, pingLayers, QueryTriggerInteraction.Collide);
        foreach (Collider colliderHit in hitCollidersArray)
        {
            if (colliderHit != null && (colliderHit.gameObject.layer == 10 || colliderHit.gameObject.layer == 24 || colliderHit.gameObject.layer == 26))
            {
                if (!collidersHit.Contains(colliderHit))
                {
                    collidersHit.Add(colliderHit);
                    //SonarPingManager sPManager = colliderHit.GetComponent<SonarPingManager>();
                    //sPManager.InstantiatePings();
                    Transform radarPingTransform = Instantiate(pfSonarPing, colliderHit.transform.position ,Quaternion.Euler(-90, 0, 0));
                    SonarPings sonarPing = radarPingTransform.GetComponent<SonarPings>();
                    if (colliderHit.gameObject.layer == 24)
                    {
                        sonarPing.type = 1;
                    }
                    if (colliderHit.gameObject.layer == 10)
                    {
                        sonarPing.type = 2;
                    }
                    if (colliderHit.gameObject.layer == 26)
                    {
                        sonarPing.type = 3;
                    }
                    //sonarPing.SetDisappearTimer(rangeMax/rangeSpeed);
                }
            }
            yield return new WaitForSeconds(pingDelay);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pulseTransform.position, sphereRange);
    }
}
