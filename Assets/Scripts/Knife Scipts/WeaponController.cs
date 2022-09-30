using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Knife;
    public bool CanAttack;
    public float AttackCooldown = 1.0f;
    public bool IsAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (CanAttack)
            {
                KnifeAttack();
            }
        }
    }


    public void KnifeAttack()
    {
        IsAttacking = true;
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
