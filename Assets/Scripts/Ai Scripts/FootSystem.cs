using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSystem : MonoBehaviour
{
    public LayerMask terrainLayer;
    public FootSystem otherFoot;
    public float stepDistance, stepHeight, stepLength, footSpacing,speed;
    public Transform body;
    public Vector3 footOffset;
    Vector3 oldPosition, newPosition, currentPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    void Start()
    {
        footSpacing = transform.localPosition.x;
        oldPosition = newPosition = currentPosition = transform.position;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;
        Raycast();

        //Make sure the body of the legs has the Z rotation set to 0!
        if (body.transform.rotation.z > 0 || body.transform.rotation.z < 0)
        {
            IsVertical();
        }
        else
        {
            IsHorizontal();
        }

        if(lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void IsVertical()
    {
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : 1;
                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = hit.normal;
            }
        }
    }

    private void IsHorizontal()
    {
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : 1;
                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = hit.normal;
            }
        }
    }

    public bool Raycast()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        Ray ray = new Ray(transform.position, down);
        Debug.DrawRay(transform.position, down * 1, Color.red);
        return false;
    }


    public bool isMoving()
    {
        return lerp < 1;
    }

    void OnDrawGizmosSelected()
    {
        if (body != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, body.position);
        }
    }
}
