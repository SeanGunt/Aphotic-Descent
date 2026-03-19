using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneBiolamps : MonoBehaviour
{
    [SerializeField] private MudMarshCutscene mudMarshCutscene;

    [SerializeField] Animator animator;

    void Start()
    {
        
    }


    void Update()
    {
        if (mudMarshCutscene.inMarshCutscene)
        {
            animator.SetBool("cutsceneStarted", true);
            
        }
    }
}
