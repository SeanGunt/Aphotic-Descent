using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DogScript : MonoBehaviour
{
    [SerializeField] private Animator dogAnimator;
    public void DogBobbling()
    {
        dogAnimator.SetFloat("bobbleFloat", 1);
        //Debug.Log("I felt that!");
        StartCoroutine(BobbleSwitch());
    }

    IEnumerator BobbleSwitch()
    {
        yield return new WaitForSeconds(1);
        //Debug.Log("Waited!");
        dogAnimator.SetFloat("bobbleFloat", 0);
    }
}
