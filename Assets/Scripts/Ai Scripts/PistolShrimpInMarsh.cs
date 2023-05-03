using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolShrimpInMarsh : MonoBehaviour
{
    [HideInInspector] public bool startedEncounter;
    public static bool killedPlayer;
    private RaycastHit hit;
    [SerializeField] private MultiAimConstraint multiAimConstraint;
    [SerializeField] private GameObject shootingPos, bullet;
    [SerializeField] private Transform gunBoneTransform;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float timeToAttackPlayer;
    [SerializeField] private Animator pistolAnimator;
    [HideInInspector] public PlayerHealthController pHelCon;
    private InvisibilityMechanic invisibilityMechanic;
    private float attackPlayerTimer;
    private bool shotPlayed, rayCastIsHitting;
    private GameObject player;
    private State state;
    private enum State
    {
        canSeePlayer, cantSeePlayer, pistolJumpscaring
    }

    private void Awake()
    {
        state = State.cantSeePlayer;
        attackPlayerTimer = timeToAttackPlayer;
        player = GameObject.FindGameObjectWithTag("Player");
        invisibilityMechanic = player.GetComponent<InvisibilityMechanic>();
        pistolAnimator = GetComponent<Animator>();
        pistolAnimator.SetBool("PistolJumpscarePlay", false);
    }

    private void Update()
    {
        if (killedPlayer)
        {
            audioSource.Pause();
        }
        switch(state)
        {
            case State.cantSeePlayer:
                CantSeePlayer();
            break;
            case State.canSeePlayer:
                HandleShooting();
                CanSeePlayer();
            break;
            case State.pistolJumpscaring:
                PistolJumpscare();
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
        if(rayCastIsHitting && !invisibilityMechanic.isInvisible && !killedPlayer)
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
                    //if (pHelCon.playerHealth <= 2.5f)
                    //{
                        //state = State.pistolJumpscaring;
                    //}
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

    private void PistolJumpscare()
    {
        pistolAnimator.SetBool("PistolJumpscarePlay", true);
        Debug.Log("I should be scaring you!");
    }
    
}
