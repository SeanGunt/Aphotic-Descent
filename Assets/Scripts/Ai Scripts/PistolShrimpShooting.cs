using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShooting : MonoBehaviour
{
    private RaycastHit hit;
    private PShrimpBlacklightEvent pShrimpBlacklightEvent;
    private PlayerHealthController playerHealthController;
    private GameObject player;
    [HideInInspector] public LineRenderer lineRenderer;
    private bool blacklightObjectBeingDestroyed, playerBeingAttacked;
    private float destroyTimer, attackPlayerTimer;
    [SerializeField] private float timeToDestroy, timeToAttackPlayer;
    [SerializeField] private Transform gunBoneTransform;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private psEnemyAI pistolShrimpAi;
    private bool canUseLaser;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthController = player.GetComponent<PlayerHealthController>();
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        destroyTimer = timeToDestroy;
        attackPlayerTimer = timeToAttackPlayer;
    }
    private void Update()
    {
        if(pistolShrimpAi.inPhase1 && pistolShrimpAi.isMoving)
        {
            canUseLaser = true;
        }

        if(pistolShrimpAi.inPhase2 && pistolShrimpAi.isMoving)
        {
            canUseLaser = false;
            lineRenderer.enabled = false;
        }
        else if(pistolShrimpAi.inPhase2 && !pistolShrimpAi.isMoving)
        {
            canUseLaser = true;
        }

        Vector3 forwardVector = gunBoneTransform.rotation * Vector3.up;
        if(Physics.Raycast(this.transform.position, forwardVector, out hit, 300f, ~ignoreLayers) && canUseLaser)
        {
            lineRenderer.enabled = true;
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
        else
        {
            playerBeingAttacked = false;
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
