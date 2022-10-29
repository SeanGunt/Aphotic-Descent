using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField]float batteryCooldown, maxTime;
    [SerializeField]bool isInactive;
    [SerializeField]GameObject Itself;
    void Update()
    {
        if (isInactive)
        {
            RespawnCooldown();
        }
        else
        {
            Itself.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        flashlightMechanic controller = other.GetComponent<flashlightMechanic>();

        if(controller != null)
        {
            if (controller.flashlightBattery < controller.maxBattery)
            {
                controller.FillBattery(120);
                Debug.Log("FillBatteryCalled");
                isInactive = true;
                Itself.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No trigger found");
        }
    }

    void RespawnCooldown()
    {
        batteryCooldown = Mathf.Clamp(batteryCooldown, 0f, maxTime);
        if (isInactive)
        {
            batteryCooldown -= Time.deltaTime;
            if (batteryCooldown <= 0.1f)
            {
                isInactive = false;
                batteryCooldown = maxTime;
            }
        }
    }
}
