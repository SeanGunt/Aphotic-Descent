using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpSteps : MonoBehaviour
{
    [SerializeField] private psEnemyAI pistolShrimpAI;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] steppySounds;

    private bool canPlaySound;

    private float soundTimer = 0.3f;

    private float stepTimer;
    [SerializeField] private float stepInterval = 0.5f;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pistolShrimpAI != null && pistolShrimpAI.isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PSStepsSounds();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // reset timer so the next step happens after interval when moving again
            stepTimer = 0f;
        }

        
    }

    public void PSStepsSounds()
    {
        if (steppySounds.Length == 0) return;

        int randomIndex = Random.Range(0, steppySounds.Length);
        audioSource.PlayOneShot(steppySounds[randomIndex]);
    }

}
