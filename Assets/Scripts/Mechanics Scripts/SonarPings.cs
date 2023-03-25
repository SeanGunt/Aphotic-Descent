using UnityEngine;
using UnityEngine.UI;

public class SonarPings : MonoBehaviour
{
    private SpriteRenderer ping;
    private float disappearTimer;
    private float disappearTimerMax;
    private Color color;

    private void Awake()
    {
        ping = this.GetComponent<SpriteRenderer>();
        disappearTimerMax = 1f;
        disappearTimer = 0f;
        color = new Color(1, 1, 1, 1);
    }

    private void Update()
    {
        disappearTimer += Time.deltaTime;

        color.a = Mathf.Lerp(disappearTimerMax, 0f, disappearTimer/disappearTimerMax);
        ping.color = color;

        if (disappearTimer >= disappearTimerMax) 
        {
            this.gameObject.SetActive(false);
            disappearTimer = 0f;
        }
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetDisappearTimer(float disappearTimerMax)
    {
        this.disappearTimerMax = disappearTimerMax;
        disappearTimer = 0f;
    }
}
