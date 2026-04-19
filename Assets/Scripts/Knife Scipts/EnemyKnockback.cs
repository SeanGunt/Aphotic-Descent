using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    [SerializeField] private float knockbackForce;
    private State state;

    ffScr freakFishScript;
    anglerAi angScr;
    fishEnemy eelScr;
    private bool freakFishAttached = false;
    private bool anglerFishAttached = false;
    private bool eelAttached = false;
    private float stopTime;
    private float resetTime;
    private bool stopped = false;

    enum State
    {
        beingKnockedBack, normal
    }
    private void Awake()
    {
        cam = Camera.main;
        rb = this.GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = this.GetComponentInParent<Rigidbody>();
        }
        state = State.normal;

        if(this.gameObject.name == "naMeFinder")
        {
            freakFishAttached = true;
            freakFishScript = GetComponent<ffScr>();
            stopTime = freakFishScript.stunTime;
            resetTime = stopTime;
        }
        
        //deprecated functionality
        //if(this.gameObject.name == "angLureTrigger")
        //{
        //    anglerFishAttached = true;
        //    angScr = GetComponentInParent<anglerAi>();
        //   // stopTime = angScr.anglerStunTime;
        //    resetTime = stopTime;
        //}

        if(this.gameObject.name == "FrankyFace")
        {
            eelAttached = true;
            eelScr = GetComponentInParent<fishEnemy>();
            stopTime = eelScr.StunTime;
            resetTime = stopTime;
            //
            //resetTime = stopTime
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
        {
			rb.AddForce(cam.transform.forward * knockbackForce, ForceMode.Impulse);
			state = State.beingKnockedBack;
			StartCoroutine("ResetKnockBack", 0.5f);
			if (freakFishAttached && !stopped)
            {
				
				stopped = true;
                freakFishScript.theAgent.speed = 0;
                freakFishScript.animator.SetBool("isStunned", true);
                //freakFishScript.animator.SetBool("isBiting", false);
				freakFishScript.OverrideCooldown();
				Debug.Log("ff was hit");
            }

            if(anglerFishAttached && !stopped)
            {
				stopped = true;
                angScr.anglerAgent.speed = 0;
                
                Debug.Log("angler was hit");
            }

            if (eelAttached && !stopped)
            {
                //template stuff for eel being stopped
                stopped = true;
                eelScr.StunTheEel();
                //eelScr.FrankyAnimator.SetBool("isStunned", true);
                //eelScr.OverrideCooldown();
                //eelscr.agent.speed = 0;
            }
        }
    }

    private void Normal()
    {
            rb.isKinematic = true;
    }

    private void BeingKnockedBack()
    {
            rb.isKinematic = false;
    }

    IEnumerator ResetKnockBack(float knockbackDuration)
    {
        yield return new WaitForSeconds(knockbackDuration);
        state = State.normal;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.normal:
                Normal();
            break;

            case State.beingKnockedBack:
                BeingKnockedBack();
            break;

        }

        if(stopped)
        {
            stopTime -= Time.deltaTime;

            if(stopTime <= 0 && freakFishAttached)
            {
                freakFishScript.theAgent.speed = freakFishScript.agentSpeed;
                stopTime = resetTime;
                Debug.Log("ff speed resetting");
                stopped = false;
                freakFishScript.animator.SetBool("isStunned", false);
                freakFishScript.animator.SetBool("isBiting", false);
            }
            
            if(stopTime <= 0 && anglerFishAttached)
            {
                angScr.anglerAgent.speed = angScr.anglerSpeed;
                stopTime = resetTime;
                Debug.Log("angler speed resetting");
                stopped = false;
            }
        }
    }
}