using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterBehindCorner : MonoBehaviour
{
    [SerializeField] private GameObject CrabLabImposter;
    [SerializeField] private Animator imposterAnimator;

    private void Start()
    {
        imposterAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            imposterAnimator.SetBool("isSeen", true);
            Destroy(this.gameObject, 1.5f);
        }
    }
}
