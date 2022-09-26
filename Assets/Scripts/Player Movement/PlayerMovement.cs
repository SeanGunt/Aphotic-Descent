using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
   
    public float speed = 12f;
    public float gravity = -9.81f;
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    public float invisibilityCharges = 3.0f;

    public Text invisText;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    [SerializeField]
    private bool isInvisible;
    [SerializeField] public bool isSafe;

    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      invisText.text = "";
    }
    // Update is called once per frame
    void Update()
    {
      isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

      if(isGrounded && velocity.y < 0)
      {
        velocity.y = -2f;
      }
      
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
          if (invisibilityCharges > 0 && !isSafe)
          {
            isInvisible = true;
            invisibleTimer = timeInvisible;
            isSafe = true;
            invisibilityCharges--;
            invisText.text = "You are invisible!";
          }
        }
        if (isInvisible)
        {
          invisibleTimer -= Time.deltaTime;
          if (invisibleTimer <= 0)
          {
            isInvisible = false;
            isSafe = false;
            invisText.text = "You are no longer invisible!";
          }
        }

    }

}
