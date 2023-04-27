using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissShrimpBullet : MonoBehaviour
{
    private bool canEnter = true;
    private void Awake()
    {
        Invoke("DestroyThis",1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && canEnter)
        {
            PlayerHealthController playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
            if (!playerHealthController.isNearDeath)
            {
                playerHealthController.ChangeHealth(-10f);
                playerHealthController.TakeDamage();
            }
            else if (playerHealthController.isNearDeath)
            {
                playerHealthController.DieToThePShrimpInDaMarsh();
                PistolShrimpInMarsh.killedPlayer = true;
            }
            canEnter = false;
        }
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
