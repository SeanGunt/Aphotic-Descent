using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecondDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    void Awake()
    {
        Debug.Log("called");
        door.transform.eulerAngles = new Vector3(0,-90,0);
    }
}
