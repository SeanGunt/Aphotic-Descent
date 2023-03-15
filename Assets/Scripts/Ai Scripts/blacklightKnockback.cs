using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacklightKnockback : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    [SerializeField] private float knockbackForce;
    private State state;

    ffScr freakFishScript;
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
        rb = this.GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = this.GetComponentInParent<Rigidbody>();
        }
        state = State.normal;
        
        if(this.gameObject.name == "angLureTrigger")
        {
            anglerFishAttached = true;
            angScr = GetComponentInParent<anglerAi>();
            //stopTime = angScr.anglerStunTime;
            resetTime = stopTime;
        }
    }

    public void knockingBack()
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

    private void Normal()
    {
//        rb.isKinematic = true;
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
                Debug.Log("angler debug 1");
                Normal();
            break;

            case State.beingKnockedBack:
                Debug.Log("angler debug 2");
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
