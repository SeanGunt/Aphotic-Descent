using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpManSFX : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private shrimpManScript shrimpManAI;
    [SerializeField] private AudioClip[] diveSFX;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shrimpManAI != null && shrimpManAI.goingDown)
        {
            int randomNoise = Random.Range(0, 2);
            audioSource.PlayOneShot(diveSFX[randomNoise]);
            Debug.Log("splishy splash");
        }
    }
}
