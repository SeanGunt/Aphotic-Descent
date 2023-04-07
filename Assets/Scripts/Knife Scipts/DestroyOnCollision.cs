using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{   
  public AudioClip[] kelpDestructionNoises;
  public AudioSource audioSource;
  public GameObject shatteredBox;
  public GameObject shatteredBoxInWater;
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Destroyable") 
    {
      Destroy(other.gameObject);
      shatteredBoxInWater.transform.localScale = other.gameObject.transform.localScale;
      Instantiate(shatteredBoxInWater, other.gameObject.transform.position, other.gameObject.transform.rotation);
    }

    if (other.gameObject.tag == "Kelp")
    {
      int randomNoise = Random.Range(0, 4);
      audioSource.PlayOneShot(kelpDestructionNoises[randomNoise]);
      Destroy(other.gameObject);
    }

    if (other.gameObject.tag == "Boxes")
    {
      Destroy(other.gameObject);
      shatteredBox.transform.localScale = other.gameObject.transform.localScale;
      Instantiate(shatteredBox, other.gameObject.transform.position, other.gameObject.transform.localRotation);
      GameDataHolder.boxes -= 1;
    }

    if (other.gameObject.tag == "Rope")
    {
      Rigidbody ropeRB = other.GetComponentInParent<Rigidbody>();
      ropeRB.useGravity = true;
      ropeRB.isKinematic = false;
      Destroy(other.gameObject);
    }
  }
}