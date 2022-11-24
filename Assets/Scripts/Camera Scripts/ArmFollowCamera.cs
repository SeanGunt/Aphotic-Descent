using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Animator animator;
    private void Update()
    {
        animator.SetFloat("lookDir", cameraTransform.rotation.x);
    }
}
