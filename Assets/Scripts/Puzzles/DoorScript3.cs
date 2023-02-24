using UnityEngine;

public class DoorScript3 : MonoBehaviour
{
    public bool open, close, inTrigger, canOpen, canClose;
    private BoxCollider bCollider;
    private UItext uItext;
    private State state;
    
    private enum State
    {
        open, closed, broken
    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.open:
                OpenDoor();
                open = true;
                canClose = true;
                break;
            case State.closed:
                CloseDoor();
                close = true;
                canOpen = true;
                break;
            case State.broken:
                close = true;
                canOpen = false;
                break;
        }
    }

    private void OpenDoor()
    {

    }

    private void CloseDoor()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
            if (canOpen)
            {
                state = State.open;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
            if (canClose)
            {
                state = State.closed;
            }
        }
    }
}
