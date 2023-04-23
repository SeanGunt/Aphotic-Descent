using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPPath : MonoBehaviour
{
    [SerializeField] private GameObject ZPlankton, destinationPoint;
    private float speed;
    private Vector3 actualPosition;
    private bool isMoving;
    private float distance;
    //private bool isBlacklighted;
    [SerializeField] private MarshPuzzle1 marshVariable;
    
    void Awake()
    {
        speed = 2.5f;
    }
    
    void Update()
    {
        actualPosition = ZPlankton.transform.position;
        if (isMoving)
        {
            ZPlankton.transform.position = Vector3.MoveTowards(actualPosition, destinationPoint.transform.position, speed * Time.deltaTime);
            distance = Vector3.Distance(this.transform.position, destinationPoint.transform.position);
            RotateTowards(destinationPoint.transform.position);
        }

        if (distance < 0.5f)
        {
            isMoving = false;
        }
    }

    public void FreeTheShrimp()
    {
        marshVariable.ZPFreed++;
        isMoving = true;
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * 2f);
    }
}
