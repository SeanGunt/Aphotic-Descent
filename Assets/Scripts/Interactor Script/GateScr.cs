using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScr : MonoBehaviour
{
    public bool gateClosed = false;

    [SerializeField] private Transform toWhere;

    // Update is called once per frame
    void Update()
    {
        if(gateClosed)
        {
            var newPos = Vector3.MoveTowards(this.transform.position, toWhere.transform.position, 1 * Time.deltaTime);
            transform.position = newPos;
        }
    }
}
