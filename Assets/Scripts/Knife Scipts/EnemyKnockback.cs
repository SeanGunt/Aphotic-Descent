using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float range = 100f;
    public float KnockbackForce = 250;
    public Camera Cam;
    public GameObject Enemy;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ShootRaycast();
        }
    }

    void ShootRaycast()
    {
        RaycastHit hit;
         if(Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                Knockback();
            }
        }
    }

    void Knockback()
    {
        Enemy.transform.position += transform.forward * Time.deltaTime * KnockbackForce;
    }
}