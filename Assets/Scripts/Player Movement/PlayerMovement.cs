using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  private CharacterController controller;
  private PlayerInput playerInput;
  [SerializeField] private float groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, tiredCooldown,
  walkBobSpeed, walkBobAmount, underwaterBobSpeed, underwaterBobAmount;
  private float moveSpeed, defaultYPos, timer;
  public float playerStamina, maxStamina, staminaDelay;
  [SerializeField] private LayerMask ignoreMask;
  private Vector3 velocity, moveDirection;
  [HideInInspector] public bool isGrounded, hasUpgradedSuit, headbobActive;
  [SerializeField] private bool isSwimming, canSwim, isTired, canUseHeadbob;
  [SerializeField] public Image staminaBar, tiredBar, upgradedUI;
  [SerializeField] private Camera playerCamera;
  [SerializeField] private Animator animator;
  [HideInInspector] public bool inWater;
  private State state;
  enum State
  {
    inWater, outOfWater, settingPosition
  }

  public void LoadData(GameData data)
  {
    this.transform.position = data.playerPosition;
    this.transform.rotation = data.playerRotation;
    hasUpgradedSuit = data.hasUpgradedSuit;
  }

  public void SaveData(GameData data)
  {
    data.playerPosition = this.transform.position;
    data.playerRotation = this.transform.rotation;
    data.hasUpgradedSuit = hasUpgradedSuit;
  }

  private void Awake()
  {
    state = State.settingPosition;
    isTired = false;
    tiredBar.enabled = false;
    playerCamera = GetComponentInChildren<Camera>();
    defaultYPos = playerCamera.transform.localPosition.y;
    playerInput = GetComponent<PlayerInput>();
    controller = GetComponent<CharacterController>();
    
    if(PlayerPrefs.GetInt("headBob") == 1)
    {
      headbobActive = true;
    }
    else
    {
      headbobActive = false;
    }
  }

  private void Update()
  {
      if(GameDataHolder.hasUpgradedSuit == true)
      {
        hasUpgradedSuit = true;
        upgradedUI.enabled = true;
      }
      else if (GameDataHolder.invisibilityAcquired == true)
      {
        upgradedUI.enabled = true;
      }
      else
      {
        upgradedUI.enabled = false;
      }
        switch (state)
      {
        default:
          case State.settingPosition:
              DataPersistenceManager.instance.LoadGame();
              StartCoroutine(SetPlayerState(0.15f));
          break;

          case State.inWater:
              MoveInWater();
          break;

          case State.outOfWater:
              MoveOutOfWater();
              canSwim = false;
          break;
      }

    if(PlayerPrefs.GetInt("headBob") == 1)
    {
      headbobActive = true;
      //Debug.Log(canUseHeadbob + " headbob");
    }
    else
    {
      headbobActive = false;
      //Debug.Log(canUseHeadbob + " headbob");
    }

      staminaDelay -= Time.deltaTime;
  }

    private void MoveInWater()
    {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, 1f , Vector3.down, out hit, groundDistance, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }

      RaycastHit hit2;
      if (Physics.SphereCast(transform.position, 0.25f , Vector3.down, out hit2, groundDistance, ~ignoreMask))
      {
        velocity.y = 0;
      }

        if(isGrounded)
        {
          canUseHeadbob = true;
          //velocity.y = 0;
          moveSpeed = groundedSpeed;
          isSwimming = false;
          if (!canSwim || playerStamina < maxStamina && staminaDelay <= 0)
          {
            StaminaRecharge(playerStamina, maxStamina);
          }
        }
        else
        {
          isSwimming = true;
          moveSpeed = airSpeed;
        }
      
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * transform.right + move.z * transform.forward;
        controller.Move(move * Time.deltaTime * moveSpeed);

        bool isAscendKeyHeld = playerInput.actions["Ascend"].ReadValue<float>() > 0.1f;
        
        //Float Up
        if (isAscendKeyHeld && canSwim && hasUpgradedSuit)
        {
          velocity.y = floatSpeed;
          //isSwimming = true;
        }

        bool isDescendKeyHeld = playerInput.actions["Descend"].ReadValue<float>() > 0.1f;
        
        //Float Down
        if (isDescendKeyHeld && canSwim && !isGrounded && hasUpgradedSuit)
        {
          velocity.y = -floatSpeed * 2;
          //isSwimming = true;
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
          canUseHeadbob = false;
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

        //HeadBobbingCall
        if(canUseHeadbob && headbobActive)
        {
          HandleHeadBob(underwaterBobAmount, underwaterBobSpeed);
        }
          
        moveDirection = move;
        velocity.y += gravityInWater * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("walkHorizontal", move.x);
        animator.SetFloat("walkVertical", move.z);
        if (move.x > -0.1 && move.x < 0.1 && move.z > -0.1 && move.z < 0.1)
        {
          animator.SetBool("notMoving", true);
        }
        else
        {
          animator.SetBool("notMoving", false);
        }
    }

      private void MoveOutOfWater()
    {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, 1f , Vector3.down, out hit, groundDistance, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }

      RaycastHit hit2;
      if (Physics.SphereCast(transform.position, 0.25f , Vector3.down, out hit2, groundDistance, ~ignoreMask))
      {
        velocity.y = 0;
      }

        if(isGrounded)
        {
          canUseHeadbob = true;
          if (!canSwim || playerStamina < maxStamina && staminaDelay <= 0)
          {
            StaminaRecharge(playerStamina, maxStamina);
          }
        }

      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
      Vector3 move = new Vector3(input.x, 0, input.y);
      move = move.x * transform.right + move.z * transform.forward;
      controller.Move(move * Time.deltaTime * moveSpeed);
      
      if(canUseHeadbob)
        {
          HandleHeadBob(walkBobAmount, walkBobSpeed);
        }

      moveDirection = move;
      velocity.y += gravityOutOfWater * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);
      
      animator.SetFloat("walkHorizontal", move.x);
      animator.SetFloat("walkVertical", move.z);
      if (move.x > -0.1 && move.x < 0.1 && move.z > -0.1 && move.z < 0.1)
        {
          animator.SetBool("notMoving", true);
        }
        else
        {
          animator.SetBool("notMoving", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        state = State.inWater;
        inWater = true;
        if (playerStamina > 0)
        {
          canSwim = true;
        }
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        state = State.outOfWater;
        inWater = false;
      }
    }

    private void StaminaRecharge(float empty, float max)
    {
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

    private void HandleHeadBob(float amount, float speed)
    {
      if (!isGrounded) return;
      if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
      {
        timer += Time.deltaTime * speed;
        playerCamera.transform.localPosition = new Vector3(
          playerCamera.transform.localPosition.x,
          defaultYPos + Mathf.Sin(timer) * amount,
          playerCamera.transform.localPosition.z
        );
      }
    }

    public void BecomeTired()
    {
      isTired = true;
      tiredBar.enabled = true;
      tiredBar.fillAmount = 1;
    }

    IEnumerator SetPlayerState(float waitTime)
    {
      yield return new WaitForSeconds(waitTime);
      state = State.outOfWater;
    }
}
