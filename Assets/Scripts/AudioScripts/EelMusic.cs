using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelMusic : MonoBehaviour
{
    [SerializeField] private BGMManager bGMManager;
    [SerializeField] private int musicIndex;

    public void PlayEelIdleMusic()
    {
        bGMManager.SwitchBGMFade(musicIndex);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayEelIdleMusic();
        }
    }
}
