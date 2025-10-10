using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCheats : MonoBehaviour
{
    public GameObject areaTeleporter;
    public GameObject giveUpgrades;
    public GameObject freeCam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableTeleporter()
    {
        areaTeleporter.SetActive(true);
    }

    public void MassUpgrade()
    {
        giveUpgrades.SetActive(true);
    }
}
