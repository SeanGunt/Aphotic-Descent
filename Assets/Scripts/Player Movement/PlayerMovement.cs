using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  private Rigidbody rigidBody;
  private PlayerInput playerInput;
  private CapsuleCollider capsuleCollider;
  private RaycastHit slopeHit;
  [SerializeField] private float groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, tiredCooldown,
  walkBobSpeed, walkBobAmount, underwaterBobSpeed, underwaterBobAmount, rbDrag, speedMultipler;
  private Vector3 moveDirection, slopeMoveDirection;
  private float defaultYPos, timer, horizontalMovement, verticalMovement;
  public float playerStamina, maxStamina, staminaDelay, moveSpeed;
  [SerializeField] private LayerMask ignoreMask;
  private Vector2 move;
  [SerializeField] private Vector3 playerInputVector = Vector3.zero;
  [HideInInspector] public bool isGrounded, hasUpgradedSuit, headbobActive, isAscendKeyHeld, isDescendKeyHeld, inPissCage;
  [SerializeField] private bool isSwimming, canSwim, isTired, canUseHeadbob, isMoving;
  [SerializeField] public Image staminaBar, tiredBar, upgradedUI, upgradedSonarCover;
  [SerializeField] private Camera playerCamera;
  [SerializeField] private Animator animator;
  [HideInInspector] public bool inWater, inCutscene;
  [SerializeField] private GameObject freeFlyCamera, playerCam, UICanvas, groundCheck;
  [SerializeField] private PlayerMovement thePlayer;
  [SerializeField] private PlayerSettings playerSettings;
  [Header("Step Variables")]
  [SerializeField] private GameObject stepRayUpper;
  [SerializeField] private GameObject stepRayLower;
  [SerializeField] private float stepSmooth = 0.1f;
  [SerializeField] private float lowerRayLength = 0.5f;
  [SerializeField] private float upperRayLength = 0.6f;
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
    rigidBody = GetComponent<Rigidbody>();
    capsuleCollider = GetComponent<CapsuleCollider>();
    playerCamera = GetComponentInChildren<Camera>();
    defaultYPos = playerCamera.transform.localPosition.y;
    playerInput = GetComponent<PlayerInput>();
    thePlayer = GetComponent<PlayerMovement>();
    playerSettings = GetComponent<PlayerSettings>();
    //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    
    if(PlayerPrefs.GetInt("headBob") == 1)
    {
      headbobActive = true;
    }
    else
    {
      headbobActive = false;
    }
  }

  private void OnEnable()
  {
    playerSettings.enabled = true;
    UICanvas.SetActive(true);
  }

  private void Update()
  {
      if(GameDataHolder.hasUpgradedSuit == true)
      {
        hasUpgradedSuit = true;
        upgradedUI.enabled = true;
        upgradedSonarCover.enabled = true;
      }
      else
      {
        hasUpgradedSuit = false;
        upgradedUI.enabled = false;
        upgradedSonarCover.enabled = false;
      }

      if (Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.y) > 0.1f
      || Mathf.Abs(move.x) < -0.1f || Mathf.Abs(move.y) < -0.1f)
      {
        isMoving = true;
        Debug.Log(move);
      }
      else
      {
        isMoving = false;
      }
      HandleSlopeMovement();
      HandleDrag();
      switch (state)
      {
        default:
          case State.settingPosition:
              DataPersistenceManager.instance.LoadGame();
              StartCoroutine(SetPlayerState(0.15f));
          break;
          case State.inWater:
              InWater();
          break;
          case State.outOfWater:
              OutOfWater();
          break;
      }

    if(PlayerPrefs.GetInt("headBob") == 1)
    {
      headbobActive = true;
    }
    else
    {
      headbobActive = false;
    }

    staminaDelay -= Time.deltaTime;
    if(Keyboard.current.backslashKey.isPressed)
    {
      playerInput.SwitchCurrentActionMap("FreeFlyCamControls");
      freeFlyCamera.SetActive(true);
      playerCam.SetActive(false);
      thePlayer.enabled = false;
      playerSettings.enabled = false;
      UICanvas.SetActive(false);
    }
  }

  private void FixedUpdate()
  {
    switch(state)
    {
      case State.inWater:
        if (!inCutscene)
        {
          RigidbodyInWater();
        }
      break;
      case State.outOfWater:
        RigidbodyOutOfWater();
        canSwim = false;
      break;
    }
  }


    private void RigidbodyInWater()
    {
      HandleMove(gravityInWater);

      //float up
      if (isAscendKeyHeld && canSwim && hasUpgradedSuit)
      {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, floatSpeed, rigidBody.velocity.z);
        HandleSwimmingAnims();
        isSwimming = true;
      }
        
      //Float Down
      if (isDescendKeyHeld && canSwim && !isGrounded && hasUpgradedSuit)
      {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, -floatSpeed * 2, rigidBody.velocity.z);
        isSwimming = true;
      }
    }

    private void RigidbodyOutOfWater()
    {
      HandleMove(gravityOutOfWater);
    }

    private void InWater()
    {
      move = playerInput.actions["Movement"].ReadValue<Vector2>();
      isAscendKeyHeld = playerInput.actions["Ascend"].ReadValue<float>() > 0.01f;
      isDescendKeyHeld = playerInput.actions["Descend"].ReadValue<float>() > 0.01f;
      moveDirection = transform.forward * move.y + transform.right * move.x;
      HandleGroundCheck();

      if(isGrounded)
      {
        HandleGroundedAnims(move);
        rigidBody.useGravity = false;
        rbDrag = 8f;
        speedMultipler = 8f;
        canUseHeadbob = true;
        moveSpeed = groundedSpeed;
        isSwimming = false;
        if (!canSwim || playerStamina < maxStamina && staminaDelay <= 0)
        {
            StaminaRecharge(playerStamina, maxStamina);
        }
      }
      else
      {
        rigidBody.useGravity = true;
        moveSpeed = airSpeed;
        speedMultipler = 1f;
        rbDrag = 1f;
      }
        playerStamina = Mathf.Clamp(playerStamina, 0f, maxStamina);
        //Initial Stamina Check
        if (playerStamina >= maxStamina)
        {
          canSwim = true;
          tiredBar.enabled = false;
        }
  
        //HeadBobbingCall
        if(canUseHeadbob && headbobActive)
        {
          HandleHeadBob(underwaterBobAmount, underwaterBobSpeed);
        }

        HandleStaminaDrain();
    }

    private void OutOfWater()
    {
      move = playerInput.actions["Movement"].ReadValue<Vector2>();
      moveDirection = transform.forward * move.y + transform.right * move.x;
      HandleGroundCheck();
      HandleGroundedAnims(move);

      if(isGrounded)
      {
        rigidBody.useGravity = false;
        rbDrag = 8f;
        speedMultipler = 8f;
        isSwimming = false;
        canUseHeadbob = true;
        if (!canSwim && playerStamina < maxStamina && staminaDelay <= 0)
        {
            StaminaRecharge(playerStamina, maxStamina);
        }
      }
      else
      {
        rigidBody.useGravity = true;
        rbDrag = 1f;
        speedMultipler = 1f;
      }

      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      
      if(canUseHeadbob && headbobActive)
      {
        HandleHeadBob(walkBobAmount, walkBobSpeed);
      }

      HandleStaminaDrain();
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
        tiredBar.enabled = true;
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

    private void HandleDrag()
    {
      rigidBody.drag = rbDrag;
    }

    private bool OnSlope()
    {
      if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, groundDistance, ~ignoreMask))
      {
        if (slopeHit.normal != Vector3.up)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      return false;
    }

    private void HandleSlopeMovement()
    {
      slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void HandleMove(float gravity)
    {
      StepClimb();
      Physics.gravity = new Vector3(0f, gravity, 0f);
      if (!OnSlope())
      {
        rigidBody.AddForce(moveDirection.normalized * moveSpeed * speedMultipler, ForceMode.Acceleration);
      }
      else
      {
        rigidBody.AddForce(slopeMoveDirection.normalized * moveSpeed * speedMultipler, ForceMode.Acceleration);
      }

    }

    private void HandleGroundCheck()
    {
      if (Physics.CheckSphere(groundCheck.transform.position, groundDistance, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }
    }

    private void StepClimb()
    {
      Debug.DrawRay(stepRayLower.transform.position, this.transform.forward, Color.red);
      Debug.DrawRay(stepRayUpper.transform.position, this.transform.forward, Color.red);
      Debug.DrawRay(stepRayLower.transform.position, this.transform.forward + new Vector3(0,0,0.5f), Color.red);
      Debug.DrawRay(stepRayUpper.transform.position, this.transform.forward + new Vector3(0,0,0.5f), Color.red);
      Debug.DrawRay(stepRayLower.transform.position, this.transform.forward + new Vector3(0,0,-0.5f), Color.red);
      Debug.DrawRay(stepRayUpper.transform.position, this.transform.forward + new Vector3(0,0,-0.5f), Color.red);
      if (!isMoving) return;
      RaycastHit hitLower;
      if (Physics.Raycast(stepRayLower.transform.position, this.transform.forward, out hitLower, lowerRayLength, ~ignoreMask))
      {
        RaycastHit hitUpper;
        if (!Physics.Raycast(stepRayUpper.transform.position, this.transform.forward, out hitUpper, upperRayLength, ~ignoreMask))
        {
          rigidBody.velocity -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
          return;
        }
      }

      RaycastHit hitLowerleft;
      if (Physics.Raycast(stepRayLower.transform.position, this.transform.forward + new Vector3(0,0,0.5f), out hitLowerleft, lowerRayLength, ~ignoreMask))
      {
        RaycastHit hitUpper;
        if (!Physics.Raycast(stepRayUpper.transform.position, this.transform.forward + new Vector3(0,0,0.5f), out hitUpper, upperRayLength, ~ignoreMask))
        {
          rigidBody.velocity -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
          return;
        }
      }

      RaycastHit hitLowerright;
      if (Physics.Raycast(stepRayLower.transform.position, this.transform.forward + new Vector3(0,0,-0.5f), out hitLowerright, lowerRayLength, ~ignoreMask))
      {
        RaycastHit hitUpper;
        if (!Physics.Raycast(stepRayUpper.transform.position, this.transform.forward + new Vector3(0,0,-0.5f), out hitUpper, upperRayLength, ~ignoreMask))
        {
          rigidBody.velocity -= new Vector3(0f, -stepSmooth * Time.fixedDeltaTime, 0f);
          return;
        }
      }
    }

    
    private void HandleHeadBob(float amount, float speed)
    {
      if (!isGrounded) return;
      if(Mathf.Abs(move.x) > 0.1f || Mathf.Abs(move.y) > 0.1f
      || Mathf.Abs(move.x) < -0.1f || Mathf.Abs(move.y) < -0.1f)
      {
        timer += Time.deltaTime * speed;
        playerCamera.transform.localPosition = new Vector3(
          playerCamera.transform.localPosition.x,
          defaultYPos + Mathf.Sin(timer) * amount,
          playerCamera.transform.localPosition.z
        );
      }
    }

    private void HandleGroundedAnims(Vector3 move)
    {
        animator.SetBool("isSwimming", false);
        HandleNormalAnims();
    }

    private void HandleSwimmingAnims()
    {
      animator.SetBool("isSwimming", true);
      HandleNormalAnims();
    }

    private void HandleNormalAnims()
    {
      animator.SetFloat("walkHorizontal", move.x);
      animator.SetFloat("walkVertical", move.y);
      if (move.x > -0.1 && move.x < 0.1 && move.y > -0.1 && move.y < 0.1)
      {
        animator.SetBool("notMoving", true);
      }
      else
      {
        animator.SetBool("notMoving", false);
      }
    }

    private void HandleStaminaDrain()
    {
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
