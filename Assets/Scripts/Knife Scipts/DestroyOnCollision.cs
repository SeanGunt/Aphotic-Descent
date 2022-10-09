using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour


{   
  public float knockbackForce = 250;
  private Camera cam;

  private void Awake()
  {
    cam = Camera.main;
  }
  
    
    private void OnTriggerEnter(Collider other)
    {
     if (other.gameObject.tag == "Destroyable") 
      {
        Destroy(other.gameObject);
      }

       if (other.gameObject.tag == "Enemy")
       {
          other.transform.position += Vector3.Lerp(other.transform.position , cam.transform.forward * knockbackForce , Time.deltaTime * 1000);
          Debug.Log(other.transform.position);
            
        }
    }

}

   