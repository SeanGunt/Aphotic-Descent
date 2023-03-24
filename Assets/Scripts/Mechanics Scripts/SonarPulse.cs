using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SonarPulse : MonoBehaviour
{
    [SerializeField]private Transform pulseTransform;
    [SerializeField]private GameObject sPing;
    private SpriteRenderer pulseSpriteRenderer;
    private Color pulseColor;

    private float range;
    private float rangeMax;
    private float fadeRange;

    private List<Collider> collidersHit;

    private void Awake()
    {
        rangeMax = 500f;
        fadeRange = 100f;
        collidersHit = new List<Collider>();
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        float rangeSpeed = 150f;
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
            collidersHit.Clear();
        }
        
        pulseTransform.localScale = new Vector3(range, range);
        Collider[] hitCollidersArray = Physics.OverlapSphere(pulseTransform.position, range);
        foreach (Collider colliderHit in hitCollidersArray)
        {
            if (colliderHit != null)
            {
                if (!collidersHit.Contains(colliderHit))
                {
                    collidersHit.Add(colliderHit);
                    Instantiate(sPing, colliderHit.transform.position, Quaternion.identity);
                    SonarPings sonarPing = sPing.GetComponent<SonarPings>();
                    if (colliderHit.gameObject.GetComponent<LoreSaver>() != null)
                    {
                        sonarPing.SetColor(new Color(0, 0, .85f));
                    }
                    if (colliderHit.gameObject.layer == 10)
                    {
                        sonarPing.SetColor(new Color(.85f, 0, 0));
                    }
                    sonarPing.SetDisappearTimer(rangeMax/rangeSpeed);
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
