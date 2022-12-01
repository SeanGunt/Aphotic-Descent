using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip objectiveSound;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
            audioSource.PlayOneShot(objectiveSound);
            Destroy(this.gameObject);
        }
    }
}
