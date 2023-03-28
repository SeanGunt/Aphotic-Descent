using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolShrimpInMarsh : MonoBehaviour
{
    [HideInInspector] public bool startedEncounter;
    private RaycastHit hit;
    [SerializeField] private MultiAimConstraint multiAimConstraint;
    [SerializeField] private GameObject shootingPos, bullet;
    [SerializeField] private Transform gunBoneTransform;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float timeToAttackPlayer;
    private InvisibilityMechanic invisibilityMechanic;
    private float attackPlayerTimer;
    private bool shotPlayed, rayCastIsHitting;
    private GameObject player;
    private State state;
    private enum State
    {
        canSeePlayer, cantSeePlayer
    }

    private void Awake()
    {
        state = State.cantSeePlayer;
        attackPlayerTimer = timeToAttackPlayer;
        player = GameObject.FindGameObjectWithTag("Player");
        invisibilityMechanic = player.GetComponent<InvisibilityMechanic>();
    }

    private void Update()
    {
        switch(state)
        {
            case State.cantSeePlayer:
                CantSeePlayer();
            break;
            case State.canSeePlayer:
                HandleShooting();
                CanSeePlayer();
            break;
        }
    }

    private void CantSeePlayer()
    {
        lineRenderer.enabled = false;
        if (startedEncounter)
        {
            SwitchTarget(0,1);
            state = State.canSeePlayer;
        }
    }

    private void CanSeePlayer()
    {
        if (!invisibilityMechanic.isSafe)
        {
            lineRenderer.enabled = true;
            Vector3 direction = player.transform.position - this.transform.position;
            Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, rotation.y, this.transform.eulerAngles.z);
        }
    }

    private void HandleShooting()
    {
        Vector3 forwardVector =  gunBoneTransform.rotation * Vector3.up;
        rayCastIsHitting = Physics.Raycast(shootingPos.transform.position, forwardVector, out hit, 300f, ~ignoreLayer);
        if(rayCastIsHitting && !invisibilityMechanic.isInvisible)
        {
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, shootingPos.transform.position);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.GetComponent<PlayerHealthController>() != null)
            {
                if (!shotPlayed)
                {
                    audioSource.Play();
                    shotPlayed = true;
                }

                attackPlayerTimer -= Time.deltaTime;

                if(attackPlayerTimer <= 0)
                {
                    Instantiate(bullet, hit.point, Quaternion.identity);
                    shotPlayed = false;
                    attackPlayerTimer = timeToAttackPlayer;
                }
                
            }
            else
            {
                shotPlayed = false;
                attackPlayerTimer = timeToAttackPlayer;
                audioSource.Stop(); 
            }
        }
        else
        {
            shotPlayed = false;
            attackPlayerTimer = timeToAttackPlayer;
            audioSource.Stop();
        }
    }

    public void SwitchTarget(int index, float weight)
    {
        WeightedTransformArray arrayOfTransforms = multiAimConstraint.data.sourceObjects;
        for(int i = 0; i < arrayOfTransforms.Count; i++)
        {
            arrayOfTransforms.SetWeight(i, 0f);
        }
        multiAimConstraint.data.sourceObjects = arrayOfTransforms;

        WeightedTransformArray a = multiAimConstraint.data.sourceObjects;
        a.SetWeight(index, weight);
        multiAimConstraint.data.sourceObjects = a;
    }
    
}
