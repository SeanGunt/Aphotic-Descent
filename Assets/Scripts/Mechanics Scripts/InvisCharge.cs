using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisCharge : MonoBehaviour
{
    public Sprite fullCharge, emptyCharge;
    Image chargeImage;

    private void Awake()
    {
        chargeImage = GetComponent<Image>();
    }

    public void SetChargeImage(ChargeStatus status)
    {
        switch (status)
        {
            case ChargeStatus.Empty:
                chargeImage.sprite = emptyCharge;
                break;
            case ChargeStatus.Full:
                chargeImage.sprite = fullCharge;
                break;
        }
    }
}

public enum ChargeStatus
{
    Empty = 0,
    Full = 1
}
