using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacklightKnockback : MonoBehaviour
{
    //this is made for the angler fish and its ai, so be aware if you want to stick it on something else
    private Camera cam;
    private Rigidbody rb;
    [HideInInspector] public float knockbackForce;
    private float resetKb;
    private State state;
    anglerAi angScr;
    private bool anglerFishAttached = false;
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
        rb = this.GetComponentInParent<Rigidbody>();
        resetKb = knockbackForce;
        
        state = State.normal;
        
        if(this.gameObject.name == "angLureTrigger")
        {
            anglerFishAttached = true;
            angScr = GetComponentInParent<anglerAi>();
            stopTime = angScr.anglerStunTime;
            resetTime = stopTime;
        }
    }

    public void knockingBack()
    {
       if(angScr.isAlive)
       {
            rb.AddForce(cam.transform.forward * knockbackForce, ForceMode.Impulse);
            state = State.beingKnockedBack;
            StartCoroutine("ResetKnockBack", 0.5f);

            if(anglerFishAttached && !stopped)
            {
                stopped = true;
                angScr.anglerAgent.speed = 0;
                
                Debug.Log("angler was hit");
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
        knockbackForce = resetKb;
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
