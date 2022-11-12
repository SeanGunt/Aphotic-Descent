using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{   
  public AudioClip[] kelpDestructionNoises;
  public AudioSource audioSource;
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Destroyable") 
    {
      Destroy(other.gameObject);
    }

    if (other.gameObject.tag == "Kelp")
    {
      int randomNoise = Random.Range(0, 4);
      audioSource.PlayOneShot(kelpDestructionNoises[randomNoise]);
      Destroy(other.gameObject);
    }
  }
}