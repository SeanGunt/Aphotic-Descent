using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private PlayerHealthController pHC;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        pHC = player.GetComponent<PlayerHealthController>();
    }
    
    public void EndPlayer()
    {
        pHC.ChangeHealth(-15.0f);
        pHC.TakeDamage();
        Debug.Log("Hit Player");
    }
}
