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
    private bool freakFishAttached = false;
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
        state = State.normal;

        if(this.gameObject.name == "naMeFinder")
        {
            freakFishAttached = true;
            freakFishScript = GetComponent<ffScr>();
            stopTime = freakFishScript.stunTime;
            resetTime = stopTime;
        }
        else
        {
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
        {
            rb.AddForce(cam.transform.forward * knockbackForce, ForceMode.Impulse);
            state = State.beingKnockedBack;
            StartCoroutine("ResetKnockBack", 0.5f);

            if(freakFishAttached && !stopped)
            {
                stopped = true;
                freakFishScript.theAgent.speed = 0;
                
                Debug.Log("ff was hit");
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

            if(stopTime <= 0)
                {
                    freakFishScript.theAgent.speed = freakFishScript.agentSpeed;
                    stopTime = resetTime;
                    Debug.Log("ff speed resetting");
                    stopped = false;
                }
        }
    }
}