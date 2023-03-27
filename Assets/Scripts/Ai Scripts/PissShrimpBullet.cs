using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissShrimpBullet : MonoBehaviour
{
    private bool canEnter;
    private void Awake()
    {
        Invoke("DestroyThis",1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
            playerHealthController.ChangeHealth(-10f);
            playerHealthController.TakeDamage();
        }
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
