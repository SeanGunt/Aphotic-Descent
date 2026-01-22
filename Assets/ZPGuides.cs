using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPGuides : MonoBehaviour
{
    [SerializeField] private GameObject blockade, ZP1, ZP2, ZP3;
    [SerializeField] private GameObject[] destinationPoints1, destinationPoints2, destinationPoints3;
    private Vector3 destinationPoint1, destinationPoint2, destinationPoint3, currentPosition1, cP2, cP3;
    private int positionInPoints;
    private float totalLength;
    private bool movingToNextPosition, atFinalDestination;

    [SerializeField] private Collider moveTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(!blockade.activeSelf)
        {
            MovingTheZP();
        }
    }
    private void MovingTheZP()
    {
        currentPosition1 = ZP1.transform.position;
        cP2 = ZP2.transform.position;
        cP3 = ZP3.transform.position;
        if (destinationPoints1.Length == 0 || destinationPoints2.Length == 0 || destinationPoints3.Length == 0)
        {
            return;
        }
        if(!movingToNextPosition)
        {
            if (positionInPoints == destinationPoints1.Length-1)
            {
                atFinalDestination = true;
                positionInPoints = 0;
                return;
            }
            destinationPoint1 = destinationPoints1[positionInPoints].transform.position;
            destinationPoint2 = destinationPoints2[positionInPoints].transform.position;
            destinationPoint3 = destinationPoints3[positionInPoints].transform.position;
            
            positionInPoints++;
            movingToNextPosition = true;
        }
        ZP1.transform.position = Vector3.MoveTowards(currentPosition1, destinationPoint1, 2 * Time.deltaTime);
        ZP2.transform.position = Vector3.MoveTowards(cP2, destinationPoint2, 2 * Time.deltaTime);
        ZP3.transform.position = Vector3.MoveTowards(cP3, destinationPoint3, 2 * Time.deltaTime);
        totalLength = Vector3.Distance(currentPosition1, destinationPoint1);

        if (totalLength <= 0.2f)
        {
            movingToNextPosition = false;
        }
    }
}
