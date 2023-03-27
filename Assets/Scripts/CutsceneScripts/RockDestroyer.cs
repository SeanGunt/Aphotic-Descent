using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyer : MonoBehaviour
{
    private void Update()
    {
        if (GameDataHolder.eelIsDead)
        {
            this.gameObject.SetActive(false);
        }
    }
}
