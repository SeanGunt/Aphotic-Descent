using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShrimpManChamberLook : MonoBehaviour
{
    public GameObject ShrimpManChamber;

    [SerializeField] private Animator ShrimpManChamberAnimator;
    //[SerializeField] private GameObject StareAim;
    [SerializeField] private MultiAimConstraint multiAimConstraint;


    private void Start()
    {
        //StareAim.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetWeight(0, 1);
            BoxCollider bc = GetComponent<BoxCollider>();
            ShrimpManChamberAnimator.SetBool("ManLook", true);
            //StareAim.SetActive(true);
            //bc.enabled = false;
            Debug.Log("I see you...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShrimpManChamberAnimator.SetBool("ManLook", false);
            SetWeight(0, 0);
            Debug.Log("Goodbye Diver");
        }
    }

    private void SetWeight(int index, float weight)
    {
        WeightedTransformArray arrayOfTransforms = multiAimConstraint.data.sourceObjects;
        arrayOfTransforms.SetWeight(index, weight);
        multiAimConstraint.data.sourceObjects = arrayOfTransforms;
    }
}