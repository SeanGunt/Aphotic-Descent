using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{   
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Destroyable") 
    {
      Destroy(other.gameObject);
    }
  }
}