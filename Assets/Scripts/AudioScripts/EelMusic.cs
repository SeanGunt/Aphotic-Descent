using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelMusic : MonoBehaviour
{
    [SerializeField] private int index;
    public void PlayEelIdleMusic()
    {
        BGMManager.instance.SwitchBGMFade(index);
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
