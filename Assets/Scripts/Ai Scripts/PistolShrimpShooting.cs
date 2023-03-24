using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShooting : MonoBehaviour
{
    private RaycastHit hit;
    private PShrimpBlacklightEvent pShrimpBlacklightEvent;
    private PlayerHealthController playerHealthController;
    private GameObject player;
    private LineRenderer lineRenderer;
    private bool blacklightObjectBeingDestroyed, playerBeingAttacked;
    private float destroyTimer, attackPlayerTimer;
    [SerializeField] private float timeToDestroy, timeToAttackPlayer;
    [SerializeField] private Transform gunBoneTransform;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private psEnemyAI pistolShrimpAi;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthController = player.GetComponent<PlayerHealthController>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        destroyTimer = timeToDestroy;
        attackPlayerTimer = timeToAttackPlayer;
    }
    private void Update()
    {
        Vector3 forwardVector = gunBoneTransform.rotation * Vector3.up;
        lineRenderer.enabled = true;
        if(Physics.Raycast(this.transform.position, forwardVector, out hit, 300f, ~ignoreLayers))
        {
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, hit.point);

            if(hit.collider.GetComponent<PShrimpBlacklightEvent>() != null)
            {
                pShrimpBlacklightEvent = hit.collider.GetComponent<PShrimpBlacklightEvent>();
                blacklightObjectBeingDestroyed = true;
                if(blacklightObjectBeingDestroyed)
                {
                    destroyTimer -= Time.deltaTime;
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

            if(hit.collider.GetComponent<PlayerHealthController>() != null)
            {
                playerBeingAttacked = true;
            }
            else
            {
                playerBeingAttacked = false;
                attackPlayerTimer =  timeToAttackPlayer;
            }
        }

        HandleAttackingPlayer();
    }

    private void HandleAttackingPlayer()
    {
        if (playerBeingAttacked)
        {
            attackPlayerTimer -= Time.deltaTime;

            if(attackPlayerTimer <= 0)
            {
                playerHealthController.ChangeHealth(-10f);
                playerHealthController.TakeDamage();
                attackPlayerTimer = timeToAttackPlayer;
            }
        }
    }
}
