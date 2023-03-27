using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissShrimpDoorKill : MonoBehaviour
{
    [SerializeField] private PissShrimpDoor pissShrimpDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !pissShrimpDoor.isClosed)
        {
            PlayerHealthController playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
            playerHealthController.TakeDamage();
            playerHealthController.ChangeHealth(-25f);
        }
    }
}
