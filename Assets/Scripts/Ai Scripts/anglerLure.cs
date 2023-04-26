using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglerLure : MonoBehaviour
{
    anglerAi angScript;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] jingleNoises;
    // Start is called before the first frame update
    void Awake()
    {
        angScript = GameObject.Find("AnglerPhishe").GetComponent<anglerAi>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Knife")
        {
            Debug.Log("jingle jangle");
            int randomNoise = Random.Range(0, 3);
            audioSource.PlayOneShot(jingleNoises[randomNoise]);
            angScript.anglerAgent.destination = this.transform.position;
            angScript.isInvestigating = true;
        }
    }
}
