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
    private InputAction knife;
    private BoxCollider bc;
    public Animator animator;
    public float knifeSwingCost;
    PlayerMovement playerMoveScript;
    private void Awake()
    {
        bc = Knife.GetComponent<BoxCollider>();
        bc.enabled = false;
        playerInputActions = InputManager.inputActions;
    }

    private void OnEnable()
    {
        knife = playerInputActions.PlayerControls.Knife;
        //playerInputActions.Enable();
    }

    private void OnDisable()
    {
        //playerInputActions.Disable();
    }
    
    void Update()
    {
        if (knife.triggered)
        //if (Input.GetButtonDown("Knife") || Input.GetAxisRaw("Knife") > 0)
        {
            if (CanAttack && GameDataHolder.knifeHasBeenPickedUp)
            {
                playerMoveScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
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
        if(playerMoveScript.playerStamina > 0 && !IsAttacking)
        {
            playerMoveScript.playerStamina -= (playerMoveScript.maxStamina / knifeSwingCost); //the lower the number the more stam it costs, and maxStam is 8 atm

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
