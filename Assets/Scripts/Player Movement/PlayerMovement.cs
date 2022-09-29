using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public CharacterController controller;
   
  [SerializeField] private float moveSpeed, groundedSpeed, airSpeed, floatSpeed, groundDistance, gravity;
  [SerializeField] private Transform groundCheck;
  [SerializeField] private LayerMask groundMask;
  private Vector3 velocity;
  private bool isGrounded;
  private State state;
    
  enum State
  {
    move
  }

  private void Awake()
  {
    state = State.move;
  }
  private void Update()
  {
    switch (state)
    {
      default:
        case State.move:
              Move();
      break;
    }
  }

    private void Move()
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
    Debug.Log(moveSpeed);
      
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

      velocity.y += gravity * Time.deltaTime;

      controller.Move(velocity * Time.deltaTime);
    }
}
