using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trenchKnifePuzzle : MonoBehaviour
{
    void OnColliderEnter(Collision other)
    {
        if(other.gameObject.tag == "Knife")
        {
            Debug.Log("something hit!");
            Destroy(this.gameObject);
        }
    }
}
