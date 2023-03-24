using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePissCave: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameDataHolder.inPsShrimpCave = true;
            GameDataHolder.inEelCave = false;
        }
    }
}
