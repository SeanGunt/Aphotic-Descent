using UnityEngine;

public class DoorScript3 : MonoBehaviour
{
    public bool open, close, inTrigger, canOpen, canClose, unbroken;
    private BoxCollider bCollider;
    private UItext uItext;
    private State state;
    private Animator animator;
    
    private enum State
    {
        open, closed, idle, broken
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        canOpen = true;
        state = State.idle;
    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.open:
                OpenDoor();
                break;
            case State.closed:
                CloseDoor();
                break;
            case State.idle:
                close = true;
                canOpen = true;
                break;
            case State.broken:
                close = true;
                canOpen = false;
                FixDoor();
                break;
        }
    }

    private void OpenDoor()
    {
        animator.SetBool("OpenDoor", true);
        animator.SetBool("CloseDoor",false);
    }

    private void CloseDoor()
    {
        animator.SetBool("CloseDoor", true);
        animator.SetBool("OpenDoor", false);
    }

    private void IdleDoor()
    {
        animator.SetBool("CloseDoor", false);
        animator.SetBool("OpenDoor", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
            if (canOpen && close && inTrigger)
            {
                state = State.open;
                open = true;
                close = false;
                canClose = true;
                canOpen = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
            if (canClose && open && !inTrigger)
            {
                state = State.closed;
                close = true;
                open = false;
                canOpen = true;
                canClose = false;
            }
        }
    }

    public void FixDoor()
    {
        if (unbroken)
        {
            state = State.idle;
        }
    }

    public void BreakDoor()
    {
        state = State.broken;
        close = true;
        canOpen = false;
    }
}
