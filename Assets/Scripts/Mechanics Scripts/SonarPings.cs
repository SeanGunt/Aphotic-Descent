using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SonarPings : MonoBehaviour
{
    private SpriteRenderer pingRenderer;
    private float disappearTimer;
    private float disappearTimerMax;
    private Color color, pingColor;
    private bool colorSet;
    public int type;

    private void Awake()
    {
        pingRenderer = this.GetComponent<SpriteRenderer>();
        disappearTimerMax = 1f;
        disappearTimer = 0f;
        StartCoroutine(PingFade(2.75f));
        colorSet = false;
    }

    private void Update()
    {
        if (type == 1 && !colorSet)
        {
            SetColor(new Color(0f, 0f, 1f, 1f));
            colorSet = true;
        }
        else if (type == 2 && !colorSet)
        {
            SetColor(new Color(1f, 0f, 0f, 1f));
            colorSet = true;
        }
    }

    public void SetColor(Color color)
    {
        pingColor = color;
    }

    // public void SetDisappearTimer(float disappearTimerMax)
    // {
    //     this.disappearTimerMax = disappearTimerMax;
    //     disappearTimer = 0f;
    //     disappearStarted = true;
    // }

    IEnumerator PingFade(float t)
    {
        while(pingRenderer.color.a > 0)
        {
           pingRenderer.color = new Color(pingColor.r, pingColor.g, pingColor.b, pingRenderer.color.a-Time.deltaTime/t);
           yield return null;
        }
        Destroy(gameObject);
    }
}
