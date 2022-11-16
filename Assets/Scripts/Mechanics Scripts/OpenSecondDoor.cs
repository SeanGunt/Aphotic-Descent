using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecondDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] flashlightMechanic fmech;
    public void Open()
    {
        door.transform.eulerAngles = new Vector3(0,-90,0);
        fmech.flashlightText.text = "Door Opened";
        fmech.Invoke("ClearUI", 3);
    }
}
