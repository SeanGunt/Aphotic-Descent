using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
  public CharacterController controller;
   
  [SerializeField] private float moveSpeed, groundedSpeed, airSpeed, floatSpeed, outOfWaterSpeed, 
  groundDistance, gravityInWater, gravityOutOfWater;
  [SerializeField] private Transform groundCheck;
  [SerializeField] private LayerMask groundMask;
  private Vector3 velocity;
  private bool isGrounded;
  private State state;
    
  enum State
  {
    inWater, outOfWater
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
    Time.timeScale = 1;
    state = State.inWater;
  }
  private void Update()
  {
    switch (state)
    {
      default:
        case State.inWater:
              MoveInWater();
      break;

        case State.outOfWater:
              MoveOutOfWater();
        break;
    }
    Debug.Log(state);
  }

    private void MoveInWater()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded)
        {
          moveSpeed = groundedSpeed;
        }
        else
        {
          moveSpeed = airSpeed;
        }
      
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Float Up
        if (Input.GetKey(KeyCode.Space))
        {
          velocity.y = floatSpeed;
        }

        //Float Down
        if (Input.GetKey(KeyCode.LeftShift))
        {
          velocity.y = -floatSpeed * 2;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravityInWater * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void MoveOutOfWater()
    {
      moveSpeed = outOfWaterSpeed;
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
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.tag == "Water")
      {
        state = State.outOfWater;
      }
    }
}
