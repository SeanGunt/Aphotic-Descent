using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPhysics : MonoBehaviour
{
    [SerializeField] private float gravityScale;
    private void FixedUpdate()
    {
        OnBreak();
    }
    public void OnBreak()
    {
        foreach(Rigidbody rb in this.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = -9.81f * gravityScale * Vector3.up;
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }
}
