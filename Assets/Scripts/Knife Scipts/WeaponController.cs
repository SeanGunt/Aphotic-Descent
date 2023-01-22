using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public GameObject Knife;
    public bool CanAttack;
    public float AttackCooldown;
    public bool IsAttacking = false;
    private PlayerInputActions playerInputActions;
    private BoxCollider bc;
    public Animator animator;
    private void Awake()
    {
        bc = Knife.GetComponent<BoxCollider>();
        bc.enabled = false;
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    
    void Update()
    {
        if (playerInputActions.PlayerControls.Knife.triggered)
        //if (Input.GetButtonDown("Knife") || Input.GetAxisRaw("Knife") > 0)
        {
            if (CanAttack && GameDataHolder.knifeHasBeenPickedUp)
            {
                KnifeAttack();
            }
        }

        if (GameDataHolder.knifeHasBeenPickedUp)
        {
            Knife.SetActive(true);
        }
        else
        {
            Knife.SetActive(false);
        }
    }


    public void KnifeAttack()
    {
        animator.SetTrigger("swungKnife");
        IsAttacking = true;
        CanAttack = false;
        StartCoroutine(ResetAttackCooldown());
    }


    IEnumerator ResetAttackCooldown()
    {
       StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    IEnumerator ResetAttackBool() 
    {
        yield return new WaitForSeconds(1.0f);
        IsAttacking = false;
    }
}
