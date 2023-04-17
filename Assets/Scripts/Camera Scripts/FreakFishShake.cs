using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakFishShake : MonoBehaviour
{
    public bool start = false;
    public float duration = 2f;
    private float actualDuration;
    public AnimationCurve curve;

    private void Awake()
    {
        start = true;
        if (PlayerPrefs.HasKey("screenShake") && PlayerPrefs.GetInt("screenShake") != 1)
        {
            actualDuration = 0f;
        }
        else
        {
            actualDuration = duration;
        }
    }

    void Update()
    {
        if (start)
        {
            StartCoroutine(Shaking());
            start = false;
        }

        if (PlayerPrefs.HasKey("screenShake") && PlayerPrefs.GetInt("screenShake") != 1)
        {
            actualDuration = 0f;
        }
    }

    IEnumerator Shaking()
    {
        Debug.Log("Shake Started");
        Debug.Log("Duration is " + actualDuration);
        Vector3 startPostion =  transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < actualDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength =  curve.Evaluate(elapsedTime / actualDuration);
            transform.position = startPostion + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPostion;
    }
}
