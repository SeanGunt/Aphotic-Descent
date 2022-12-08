using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelMusic : MonoBehaviour
{
    [SerializeField] private BGMManager bGMManager;

    public void PlayEelIdleMusic()
    {
        bGMManager.state = BGMManager.State.EelIdle;
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
