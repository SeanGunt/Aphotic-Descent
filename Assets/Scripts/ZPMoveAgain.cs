using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPMoveAgain : MonoBehaviour
{
    public Animator animator;
    private int intMove = 0;

    void Start()
    {
        animator.SetInteger("IntMove", intMove);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            intMove = 1;
            animator.SetInteger("IntMove", intMove);
            Debug.Log("ZP on the way!");
        }
    }
}
