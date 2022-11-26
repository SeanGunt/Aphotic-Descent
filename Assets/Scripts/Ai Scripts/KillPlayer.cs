using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private PlayerHealthController pHC;
    public void EndPlayer()
    {
        pHC.ChangeHealth(-15.0f);
        pHC.TakeDamage();
        Debug.Log("Hit Player");
    }
}
