using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelperSkull : MonoBehaviour
{
    [SerializeField] private GameObject KeplerSkullObj;
    [SerializeField] private Animator KeplerSkull;
    [SerializeField] private AudioClip scaryStinger;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        KeplerSkull = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("passing thru");
            KeplerSkull.SetBool("playerIsNear", true);
            Debug.Log("play anim");
            audioSource.PlayOneShot(scaryStinger);
        }
    }
}
