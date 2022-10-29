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
    private void Awake()
    {
        bc = Knife.GetComponent<BoxCollider>();
    }
    

    void Update()
    {
        if (Input.GetButtonDown("Knife") || Input.GetAxisRaw("Knife") > 0)
        {
            if (CanAttack)
            {
                KnifeAttack();
            }
        }
        if (CanAttack)
        {
            bc.enabled = false;
        }
    }


    public void KnifeAttack()
    {
        IsAttacking = true;
        bc.enabled  = true;
        CanAttack = false;
        Animator anim = Knife.GetComponent<Animator>();
        anim.SetTrigger("Attack");
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
