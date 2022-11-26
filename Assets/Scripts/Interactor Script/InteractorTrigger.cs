using UnityEngine;
using UnityEngine.Events;

public class InteractorTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;
    private bool inTrigger;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
        }
    }

    private void Update()
    {
        if (!inTrigger) return;
        else
        {
            if (Input.GetButtonDown("Interact"))
            {
                unityEvent.Invoke();
            }
        }
    }
}
