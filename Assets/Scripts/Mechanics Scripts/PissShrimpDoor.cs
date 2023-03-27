using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissShrimpDoor : MonoBehaviour
{   
    [SerializeField] private Transform pissShrimpCageDoor;
    private bool canEnter = true;
    private PlayerMovement playerMovement;
    private PlayerHealthController playerHealthController;
    private GameObject player;
    [HideInInspector] public bool isClosed = false;
    [SerializeField] private AudioSource cageAudioSource;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealthController = player.GetComponent<PlayerHealthController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && canEnter)
        {
            StartCoroutine(StartCageClose(35f));
        }
    }

    private IEnumerator StartCageClose(float duration)
    {
        BGMManager.instance.SwitchBGMFade(8);
        cageAudioSource.Play();
        canEnter = false;
        float startTime = Time.time;
        float startDoorY =  pissShrimpCageDoor.localPosition.y;
        float endDoorY = 16f;
        while(pissShrimpCageDoor.localPosition.y > endDoorY)
        {   
            float elapsedTime = Time.time - startTime;
            float t =  elapsedTime / duration;
            float newY = Mathf.Lerp(startDoorY, endDoorY, t);
            pissShrimpCageDoor.localPosition = new Vector3(pissShrimpCageDoor.localPosition.x, newY, pissShrimpCageDoor.localPosition.z);
            yield return null;
        }
        cageAudioSource.Stop();
        isClosed = true;
        if(!playerMovement.inPissCage)
        {
            playerHealthController.ChangeHealth(-15f);
        }
    }
}
