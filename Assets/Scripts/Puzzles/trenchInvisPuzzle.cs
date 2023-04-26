using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trenchInvisPuzzle : MonoBehaviour
{
    [SerializeField]private Collider subCollider;
    private UItext textController;
    private bool puzzleComplete;
    private Animator puzzleAnims;

    private void Awake()
    {
        subCollider.enabled = false;
        textController = GetComponent<UItext>();
        puzzleAnims = GetComponent<Animator>();
        puzzleComplete = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<InvisibilityMechanic>() != false && !puzzleComplete) 
        {
            if (other.gameObject.GetComponent<InvisibilityMechanic>().isInvisible == false)
            {
                textController.Text = "Prove yourself to them. Use the gift.";
                //puzzleAnims.SetBool("NegativeReact", true);
            }
            else
            {
                textController.Text = "You were recognized, the sub part is free.";
                subCollider.enabled = true;
                //puzzleAnims.SetBool("PositiveReact", true);
                puzzleComplete = true;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == 11 && !puzzleComplete)
        {
            //puzzleAnims.SetBool("Unreact", true);
            Invoke("ReturnToIdle", 1f);
        }
        else if(col.gameObject.layer == 11 && puzzleComplete)
        {
            //puzzleAnims.SetBool("Unreact", true);
            Invoke("ReturnToIdle", 1f);
        }
    }

    private void ReturnToIdle()
    {
        //puzzleAnims.SetBool("Unreact", false);
    }
}
