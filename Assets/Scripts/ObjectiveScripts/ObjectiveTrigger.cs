using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            unityEvent.Invoke();
            Destroy(this.gameObject);
        }
    }
}
