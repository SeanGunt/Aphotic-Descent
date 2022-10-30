using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]private float playerHealth, maxHealth, invincibleTimer, timeInvincible, healCooldown, maxHealCooldown, regenRate;
    [SerializeField]private Image redSplatterImage = null;
    [SerializeField]private bool isInvincible, startCooldown, canRegen;
    public bool isBleeding;

    void Start()
    {

    }

    void Update()
    {
        if (isInvincible)
        {
          invincibleTimer -= Time.deltaTime;
          if (invincibleTimer <= 0)
            isInvincible = false;
        }
        if(startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if(healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (playerHealth <= maxHealth - 0.01)
            {
                playerHealth += Time.deltaTime *regenRate;
                UpdateHealth();
            }
            else
            {
                isBleeding = false;
                playerHealth = maxHealth;
                healCooldown = maxHealCooldown;
                canRegen = false;
            }
        }
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (playerHealth/maxHealth);
        redSplatterImage.color = splatterAlpha;
    }

    public void TakeDamage()
    {
        if(playerHealth >= 0)
        {
            canRegen = false;
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;
        }
    }

    public void ChangeHealth(float amount)
    {
        if (amount < 0)
        {
          if (isInvincible || playerHealth <= 2)
            return;
          isInvincible = true;
          invincibleTimer = timeInvincible;
          isBleeding = true;
        }

        playerHealth = Mathf.Clamp(playerHealth + amount, 0, maxHealth);
    }
}
