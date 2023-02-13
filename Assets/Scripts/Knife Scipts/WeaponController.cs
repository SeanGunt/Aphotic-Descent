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
    private PlayerInput playerInput;
    private BoxCollider bc;
    public Animator animator;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
        bc = Knife.GetComponent<BoxCollider>();
        bc.enabled = false;
        playerInput = this.GetComponent<PlayerInput>();
    }
    
    void Update()
    {
        if (playerInput.actions["Knife"].ReadValue<float>() > 0)
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
        if(!IsAttacking && playerMovement.playerStamina > 0)
        {

            animator.SetTrigger("swungKnife");
            IsAttacking = true;
            CanAttack = false;
            StartCoroutine(ResetAttackCooldown());
        }
    }


    IEnumerator ResetAttackCooldown()
    {
       StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    IEnumerator ResetAttackBool() 
    {
        yield return new WaitForSeconds(1.1f);
        IsAttacking = false;
    }
}
