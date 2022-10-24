using System.Collections;
using UnityEngine;

public class FreeFlyCam : MonoBehaviour
{
    public float cameraSensitivity = 800f;
    public float verticalSpeed = 4.0f;
    public float moveSpeed = 10.0f;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start ()
    {
        Screen.lockCursor = true;
    }

    void Update ()
    {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp (rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (Input.GetButton("SpeedUp"))
        {
            transform.position += transform.forward * (moveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (moveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetButton("SlowDown"))
        {
            transform.position += transform.forward * (moveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (moveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime; 
        }

        if (Input.GetButton("Up")) {transform.position += transform.up * verticalSpeed * Time.deltaTime;}
        if (Input.GetButton("Down")) {transform.position -= transform.up * verticalSpeed * Time.deltaTime;}

        if (Input.GetKey(KeyCode.End))
        {
            Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
