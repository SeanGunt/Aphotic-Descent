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
      shatteredBox.transform.localScale = other.gameObject.transform.lossyScale;
      Instantiate(shatteredBox, other.gameObject.transform.position, other.gameObject.transform.rotation);
      GameDataHolder.boxes -= 1;
    }
  }
}