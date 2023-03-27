using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankySighting : MonoBehaviour
{
    [SerializeField] private GameObject frankySighted;
    [SerializeField] private Animator frankyBreach;

    private void Awake()
    {
        if(GameDataHolder.eelIsDead)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        frankyBreach = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            frankyBreach.SetBool("isBreaching", true);
            Destroy(this.gameObject, 4.5f);
        }
    }
}
