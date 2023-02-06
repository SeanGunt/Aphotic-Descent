using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider bc;
    private PlayerMovement playerMovement;
    public float knifeSwingStaminaAmount;

    public void EnableCollider()
    {
        bc.enabled = true;
    }

    public void DisableCollider()
    {
        bc.enabled = false;
    }

    public void UseStamina()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (playerMovement.playerStamina > 0)
        {   
            playerMovement.playerStamina -= knifeSwingStaminaAmount;
            playerMovement.staminaBar.fillAmount = playerMovement.playerStamina/playerMovement.maxStamina;
            playerMovement.staminaDelay = 1f;
        }

        if(playerMovement.playerStamina <= 0)
        {
            playerMovement.BecomeTired();
        }
    }
}
