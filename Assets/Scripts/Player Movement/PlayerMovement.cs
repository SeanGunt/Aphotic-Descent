using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  public CharacterController controller;
  [SerializeField] private float groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater, playerStamina, maxStamina, tiredCooldown,
  walkBobSpeed, walkBobAmount, underwaterBobSpeed, underwaterBobAmount;
  private float moveSpeed, defaultYPos, timer;
  [SerializeField] private LayerMask ignoreMask;
  private Vector3 velocity, moveDirection;
  private bool isGrounded, hasUpgradedSuit;
  [SerializeField] private bool isSwimming, canSwim, isTired, canUseHeadbob;
  [SerializeField] private Image staminaBar, tiredBar, walkState, swimState;
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
    playerCamera = GetComponentInChildren<Camera>();
    defaultYPos = playerCamera.transform.localPosition.y;
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
        if (Input.GetButton("Ascend") && canSwim && hasUpgradedSuit)
        {
          velocity.y = floatSpeed;
          isSwimming = true;
          walkState.gameObject.SetActive(false);
          swimState.gameObject.SetActive(true);
        }

        //Float Down
        if (Input.GetButton("Descend") && canSwim && !isGrounded && hasUpgradedSuit)
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
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravityInWater * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("walkHorizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("walkVertical", Input.GetAxis("Vertical"));
        if (Input.GetAxis("Horizontal") > -0.1 && Input.GetAxis("Horizontal") < 0.1 && Input.GetAxis("Vertical") > -0.1 && Input.GetAxis("Vertical") < 0.1)
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
        }

      moveSpeed = outOfWaterSpeed;
      canSwim = false;
      float x = Input.GetAxis("Horizontal");
      float z = Input.GetAxis("Vertical");
      Vector3 move = transform.right * x + transform.forward * z;
      
      if(canUseHeadbob)
        {
          HandleHeadBob(walkBobAmount, walkBobSpeed);
        }

      moveDirection = move;
      controller.Move(move * moveSpeed * Time.deltaTime);
      velocity.y += gravityOutOfWater * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);
      animator.SetFloat("walkHorizontal", Input.GetAxis("Horizontal"));
      animator.SetFloat("walkVertical", Input.GetAxis("Vertical"));
      if (Input.GetAxis("Horizontal") > -0.1 && Input.GetAxis("Horizontal") < 0.1 && Input.GetAxis("Vertical") > -0.1 && Input.GetAxis("Vertical") < 0.1)
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

    IEnumerator SetPlayerState(float waitTime)
    {
      yield return new WaitForSeconds(waitTime);
      state = State.outOfWater;
    }
    
  void start()
  {
    DontDestroyOnLoad(gameObject);
  }
}
