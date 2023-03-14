using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] private float outOfWaterSpeed, inWaterSpeed, ouOfWaterGravity, inWaterGravity,
    maxForce;
    private Vector2 look, move;
    private Vector3 currentVelocity, targetVelocity, velocityChange;
    private PlayerInput playerInput;
    private void Awake()
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        currentVelocity = rigidBody.velocity;
        targetVelocity = new Vector3(move.x, 0f, move.y);
        targetVelocity *= outOfWaterSpeed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        velocityChange = (targetVelocity - currentVelocity);

        Vector3.ClampMagnitude(velocityChange, maxForce);
        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
    }   

    private void OnMove()
    {
        Vector2 move = playerInput.actions["Movement"].ReadValue<Vector2>();
    }
}
