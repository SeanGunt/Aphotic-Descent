using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  public CharacterController controller;
  [SerializeField] private float moveSpeed, groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, playerStamina, maxStamina, tiredCooldown;
  [SerializeField] private Transform groundCheck;
  [SerializeField] private LayerMask groundMask;
  private Vector3 velocity;
  private bool isGrounded;
  [SerializeField] private bool isSwimming, canSwim, isTired;
  [SerializeField] private Image staminaBar, tiredBar;
  private State state;
    
  enum State
  {
    inWater, outOfWater, settingPosition
  }

  public void LoadData(GameData data)
  {
    this.transform.position = data.playerPosition;
    this.transform.rotation = data.playerRotation;
  }

  public void SaveData(GameData data)
  {
    data.playerPosition = this.transform.position;
    data.playerRotation = this.transform.rotation;
  }

  private void Awake()
  {
    state = State.settingPosition;
  }
  private void Update()
  {
        switch (state)
      {
        default:
          case State.settingPosition:
              DataPersistenceManager.instance.LoadGame();
          break;

          case State.inWater:
              MoveInWater();
          break;

          case State.outOfWater:
              MoveOutOfWater();
              canSwim = false;
          break;
      }
        Debug.Log(state);
  }

    private void MoveInWater()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded)
        {
          velocity.y = 0;
          moveSpeed = groundedSpeed;
          isSwimming = false;
          if (!canSwim || playerStamina < maxStamina)
          {
            StaminaRecharge(playerStamina, maxStamina);
          }
        }
        else
        {
          moveSpeed = airSpeed;
        }
      
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        //Float Up
        if (Input.GetButton("Ascend") && canSwim)
        {
          velocity.y = floatSpeed;
          isSwimming = true;
        }

        //Float Down
        if (Input.GetButton("Descend") && canSwim && !isGrounded)
        {
          velocity.y = -floatSpeed * 2;
          isSwimming = true;
        }

        playerStamina = Mathf.Clamp(playerStamina, 0f, maxStamina);
        //Initial Stamina Check
        if (playerStamina >= maxStamina)
        {
          canSwim = true;
          tiredBar.enabled = false;
        }
        
        //Stamina Drain
        if (isSwimming)
        {
          playerStamina -= Time.deltaTime;
          staminaBar.fillAmount = playerStamina/maxStamina;
          if (playerStamina <= 0)
          {
            canSwim = false;
            isSwimming = false;
            isTired = true;
            tiredBar.enabled = true;
            tiredBar.fillAmount = 1;
          }
        }

        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravityInWater * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

      private void MoveOutOfWater()
    {
      isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded)
        {
          velocity.y = 0;
        }
      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      float x = Input.GetAxis("Horizontal");
      float z = Input.GetAxis("Vertical");
      Vector3 move = transform.right * x + transform.forward * z;

      controller.Move(move * moveSpeed * Time.deltaTime);
      velocity.y += gravityOutOfWater * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        state = State.inWater;
        if (playerStamina > 0)
        {
          canSwim = true;
        }
      }

      if(other.gameObject.tag == "Checkpoint")
      {
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Saved");
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        state = State.outOfWater;
      }
    }

    private void StaminaRecharge(float empty, float max)
    {
      Debug.Log("StaminaRecharging");
      if (isTired)
      {
        tiredCooldown -= Time.deltaTime;
        tiredBar.fillAmount = tiredCooldown/2;
        if (tiredCooldown <= 0)
        {
          isTired = false;
          tiredBar.enabled = false;
          tiredCooldown = 2;
        }
      }
      if (empty <= max && !isTired)
      {
        empty += Time.deltaTime * 2;
        playerStamina = empty;
        staminaBar.fillAmount = playerStamina/maxStamina;
      }
    }
}
