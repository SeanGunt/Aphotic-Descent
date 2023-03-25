using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePings : MonoBehaviour
{
    private float disappearTimer, disappearTimerMax;
    private bool disappearStarted;
    
    private void Awake()
    {
        disappearTimerMax = 1f;
        disappearTimer = 0f;
        disappearStarted = false;
    }

    private void Update()
    {
        if (disappearStarted)
        {
            disappearTimer += Time.deltaTime;
        }

        if (disappearTimer >= disappearTimerMax)
        {
            disappearStarted = false;
            this.gameObject.SetActive(false);
        }
    }

    public void SetDisappearTimer(float disappearTimerMax)
    {
        this.disappearTimerMax = disappearTimerMax;
        disappearTimer = 0f;
        disappearStarted = true;
    }
}
