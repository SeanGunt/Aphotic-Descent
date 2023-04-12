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
    [SerializeField] private AudioClip scaryStinger;
    [SerializeField] private AudioSource audioSource;
    private bool soundHasPlayed;


    private void Start()
    {
        //StareAim.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetWeight(0, 1);
            BoxCollider bc = GetComponent<BoxCollider>();
            ShrimpManChamberAnimator.SetBool("ManLook", true);
            if (!soundHasPlayed)
            {
                audioSource.PlayOneShot(scaryStinger);
                soundHasPlayed = true;
            }
            Debug.Log("I see you...");
        }
        else if (other.gameObject.layer == 10)
        {
            SetWeight(1, 1);
            BoxCollider bc = GetComponent<BoxCollider>();
            ShrimpManChamberAnimator.SetBool("ShrimpLook", true);
            Debug.Log("Hello hubby");
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
        else if (other.gameObject.layer == 10)
        {
            ShrimpManChamberAnimator.SetBool("ShrimpLook", true);
            SetWeight(0, 0);
            Debug.Log("Goodbye hubby");
        }
    }

    private void SetWeight(int index, float weight)
    {
        WeightedTransformArray arrayOfTransforms = multiAimConstraint.data.sourceObjects;
        for(int i = 0; i < arrayOfTransforms.Count; i++)
        {
            arrayOfTransforms.SetWeight(i, 0f);
        }
        multiAimConstraint.data.sourceObjects = arrayOfTransforms;

        WeightedTransformArray a = multiAimConstraint.data.sourceObjects;
        a.SetWeight(index, weight);
        multiAimConstraint.data.sourceObjects = a;
    }
}
