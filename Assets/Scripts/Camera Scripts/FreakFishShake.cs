using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakFishShake : MonoBehaviour
{
    public bool start = false;
    public float duration = 1f;
    public AnimationCurve curve;

    private void Awake()
    {
        start = true;
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPostion =  transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength =  curve.Evaluate(elapsedTime / duration);
            transform.position = startPostion + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPostion;
    }
}
