using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager instance;
    [HideInInspector] public bool start;
    public AnimationCurve curve;
    private Vector3 initialPosition;
    private bool isShaking;

    private void Awake()
    {
        instance = this;
    }

    public void StartCameraShake(float duration, float strengthMultiplier)
    {
        if (PlayerPrefs.HasKey("screenShake") && PlayerPrefs.GetInt("screenShake") != 1) return;
        if (isShaking) return;
        StartCoroutine(Shaking(duration, strengthMultiplier));
    }

    IEnumerator Shaking(float duration, float strengthMultiplier)
    {
        isShaking = true;
        Vector3 startPostion = this.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float strength =  curve.Evaluate(elapsedTime / duration) * strengthMultiplier;
            this.transform.localPosition = startPostion + new Vector3(x,y,0) * strength;
            yield return null;
        }
        isShaking = false;
        this.transform.localPosition = startPostion;
    }
}
