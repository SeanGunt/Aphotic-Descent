using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Knife;
    public bool CanAttack;
    public float AttackCooldown;
    public bool IsAttacking = false;
    private BoxCollider bc;
    public Animator animator;
    private void Awake()
    {
        bc = Knife.GetComponent<BoxCollider>();
    }
    

    void Update()
    {
        if (Input.GetButtonDown("Knife") || Input.GetAxisRaw("Knife") > 0)
        {
            if (CanAttack && GameDataHolder.knifeHasBeenPickedUp)
            {
                KnifeAttack();
            }
        }
        if (CanAttack)
        {
            bc.enabled = false;
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
        bc.enabled  = true;
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
