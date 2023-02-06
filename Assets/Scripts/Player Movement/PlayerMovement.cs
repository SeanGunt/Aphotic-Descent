using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  private CharacterController controller;
  private PlayerInputActions playerInputActions;
  private InputAction movement, ascend, descend;
  [SerializeField] private float groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, tiredCooldown,
  walkBobSpeed, walkBobAmount, underwaterBobSpeed, underwaterBobAmount;
  private float moveSpeed, defaultYPos, timer;
  public float playerStamina, maxStamina, staminaDelay;
  [SerializeField] private LayerMask ignoreMask;
  private Vector3 velocity, moveDirection;
  [HideInInspector] public bool isGrounded, hasUpgradedSuit;
  [SerializeField] private bool isSwimming, canSwim, isTired, canUseHeadbob;
  [SerializeField] public Image staminaBar, tiredBar, walkState, swimState;
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
    playerInputActions = InputManager.inputActions;
    controller = GetComponent<CharacterController>();
  }

  private void OnEnable()
  {
    movement = playerInputActions.PlayerControls.Movement;
    ascend = playerInputActions.PlayerControls.Ascend;
    descend = playerInputActions.PlayerControls.Descend;
    //movement.Enable();

    //playerInputActions.PlayerControls.Ascend.Enable();
    //playerInputActions.PlayerControls.Descend.Enable();
  }

  private void OnDisable()
  {
    //movement.Disable();
    //playerInputActions.PlayerControls.Ascend.Disable();
    //playerInputActions.PlayerControls.Descend.Disable();
  }
  
  private void Update()
  {
      if(GameDataHolder.hasUpgradedSuit == true)
      {
        hasUpgradedSuit = true;
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
      staminaDelay -= Time.deltaTime;
  }

    private void MoveInWater()
    {
      if (Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.1f, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }

        if(isGrounded)
        {
          canUseHeadbob = true;
          velocity.y = 0;
          moveSpeed = groundedSpeed;
          isSwimming = false;
          walkState.gameObject.SetActive(true);
          swimState.gameObject.SetActive(false);
          if (!canSwim || playerStamina < maxStamina && staminaDelay <= 0)
          {
            StaminaRecharge(playerStamina, maxStamina);
          }
        }
        else
        {
          moveSpeed = airSpeed;
        }
      
        Vector2 input = movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * transform.right + move.z * transform.forward;
        controller.Move(move * Time.deltaTime * moveSpeed);

        bool isAscendKeyHeld = playerInputActions.PlayerControls.Ascend.ReadValue<float>() > 0.1f;
        
        //Float Up
        if (isAscendKeyHeld && canSwim && hasUpgradedSuit)
        {
          velocity.y = floatSpeed;
          isSwimming = true;
          walkState.gameObject.SetActive(false);
          swimState.gameObject.SetActive(true);
        }

        bool isDescendKeyHeld = playerInputActions.PlayerControls.Descend.ReadValue<float>() > 0.1f;
        
        //Float Down
        if (isDescendKeyHeld && canSwim && !isGrounded && hasUpgradedSuit)
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
        if(canUseHeadbob)
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
      if (Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.1f, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }

        if(isGrounded)
        {
          canUseHeadbob = true;
          velocity.y = 0;
          walkState.gameObject.SetActive(true);
          swimState.gameObject.SetActive(false);
          if (!canSwim || playerStamina < maxStamina && staminaDelay <= 0)
          {
            StaminaRecharge(playerStamina, maxStamina);
          }
        }

      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      Vector2 input = movement.ReadValue<Vector2>();
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
