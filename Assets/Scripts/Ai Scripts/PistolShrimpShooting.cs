using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShooting : MonoBehaviour
{
    private RaycastHit hit;
    private PShrimpBlacklightEvent pShrimpBlacklightEvent;
    private LineRenderer lineRenderer;
    private bool blacklightObjectBeingDestroyed;
    private float destroyTimer;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private Transform gunBoneTransform;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private psEnemyAI pistolShrimpAi;

    private void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        destroyTimer = timeToDestroy;
    }
    private void Update()
    {
        Vector3 forwardVector = gunBoneTransform.rotation * Vector3.up;
        if (!pistolShrimpAi.isMoving)
        {
            lineRenderer.enabled = true;
            if(Physics.Raycast(this.transform.position, forwardVector, out hit, 25f, ~ignoreLayers))
            {
                if(hit.collider.GetComponent<PShrimpBlacklightEvent>() != null)
                {
                    pShrimpBlacklightEvent = hit.collider.GetComponent<PShrimpBlacklightEvent>();
                    blacklightObjectBeingDestroyed = true;
                    if(blacklightObjectBeingDestroyed)
                    {
                        destroyTimer -= Time.deltaTime;
                        Debug.Log(destroyTimer);
                    }
                    if (destroyTimer <= 0)
                    {
                        pShrimpBlacklightEvent.Delete();
                        destroyTimer = timeToDestroy;
                    }
                
                }
                else
                {
                    blacklightObjectBeingDestroyed = false;
                }
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.SetPosition(0, this.transform.position);
                    lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
