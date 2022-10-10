using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    [SerializeField] private float knockbackForce;
    private State state;

    enum State
    {
        beingKnockedBack, normal
    }
    private void Awake()
    {
        cam = Camera.main;
        rb = this.GetComponent<Rigidbody>();
        state = State.normal;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
        {
            rb.AddForce(cam.transform.forward * knockbackForce, ForceMode.Impulse);
            state = State.beingKnockedBack;
            StartCoroutine("ResetKnockBack", 0.5f);
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
    }
}