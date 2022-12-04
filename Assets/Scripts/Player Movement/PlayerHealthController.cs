using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]private float playerHealth, maxHealth, invincibleTimer, timeInvincible, healCooldown, maxHealCooldown, regenRate;
    [SerializeField]private Image redSplatterImage = null;
    [SerializeField]private bool isInvincible, startCooldown, canRegen;
    public bool isBleeding, gameOver;
    [SerializeField]private GameObject gameOverMenu;
    public AudioSource audioSource;
    public AudioClip hitSound;

    void Start()
    {
        gameOver = false;
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
                isBleeding = true;
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
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            gameOver = true;
        }
        if (gameOver)
        {
            gameOverMenu.SetActive(true);
            startCooldown = false;
            Time.timeScale = 0;
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
            int randomHitSound = Random.Range(0,2);
            Debug.Log(randomHitSound);
            audioSource.PlayOneShot(hitSound);
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
          if (isInvincible || playerHealth <= 0.1f)
            return;
          isInvincible = true;
          invincibleTimer = timeInvincible;
          isBleeding = true;
        }

        playerHealth = Mathf.Clamp(playerHealth + amount, 0, maxHealth);
    }

    public void ResetHealth()
    {
        playerHealth = maxHealth;
        gameOver = false;
        Time.timeScale = 1;
    }
}
