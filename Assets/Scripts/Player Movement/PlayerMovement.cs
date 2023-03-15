using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  private Rigidbody rigidBody;
  private PlayerInput playerInput;
  private CapsuleCollider capsuleCollider;
  [SerializeField] private float groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, tiredCooldown,
  walkBobSpeed, walkBobAmount, underwaterBobSpeed, underwaterBobAmount, rbDrag, speedMultipler;
  private Vector3 moveDirection;
  private float defaultYPos, timer, horizontalMovement, verticalMovement;
  public float playerStamina, maxStamina, staminaDelay, moveSpeed;
  [SerializeField] private PhysicMaterial noFriction, normalFriction;
  [SerializeField] private LayerMask ignoreMask;
  private Vector2 move;
  [SerializeField] private Vector3 playerInputVector = Vector3.zero;
  [HideInInspector] public bool isGrounded, hasUpgradedSuit, headbobActive, isAscendKeyHeld, isDescendKeyHeld;
  [SerializeField] private bool isSwimming, canSwim, isTired, canUseHeadbob;
  [SerializeField] public Image staminaBar, tiredBar, upgradedUI;
  [SerializeField] private Camera playerCamera;
  [SerializeField] private Animator animator;
  [HideInInspector] public bool inWater, inCutscene;
  [SerializeField] private GameObject freeFlyCamera, playerCam, UICanvas, groundCheck;
  [SerializeField] private PlayerMovement thePlayer;
  [SerializeField] private PlayerSettings playerSettings;
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
      }
      else
      {
        hasUpgradedSuit = false;
        upgradedUI.enabled = false;
      }
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
      HandleGroundedAnims(move);

      if(isGrounded)
      {
        capsuleCollider.material = normalFriction;
        rbDrag = 6f;
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
        //isSwimming = true;
        moveSpeed = airSpeed;
        capsuleCollider.material = noFriction;
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
    }

    private void OutOfWater()
    {
      move = playerInput.actions["Movement"].ReadValue<Vector2>();
      moveDirection = transform.forward * move.y + transform.right * move.x;
      HandleGroundCheck();
      HandleGroundedAnims(move);

      if(isGrounded)
      {
        capsuleCollider.material = normalFriction;
        rbDrag = 6f;
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
        rbDrag = 1f;
        speedMultipler = 1f;
        capsuleCollider.material = noFriction;
      }

      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      
      if(canUseHeadbob && headbobActive)
        {
          HandleHeadBob(walkBobAmount, walkBobSpeed);
        }

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

    private void HandleMove(float gravity)
    {
      rigidBody.AddForce(moveDirection.normalized * moveSpeed * speedMultipler, ForceMode.Acceleration);
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

    private void HandleGroundCheck()
    {
      RaycastHit hit;
      if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, groundDistance, ~ignoreMask))
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }
    }

    private void HandleSwimmingAnims()
    {
      animator.SetBool("isSwimming", true);
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
