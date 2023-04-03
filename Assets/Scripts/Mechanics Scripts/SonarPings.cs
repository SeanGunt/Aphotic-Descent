using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SonarPings : MonoBehaviour
{
    private SpriteRenderer pingRenderer;
    private Color color, pingColor;
    private bool colorSet;
    public int type;

    private void Awake()
    {
        pingRenderer = this.GetComponent<SpriteRenderer>();
        StartCoroutine(PingFade(2.75f));
        colorSet = false;
    }

    private void Update()
    {
        if (!colorSet)
        {
            switch (type)
            {
                case 1:
                    SetColor(new Color(.43f, 1f, 1f, 1f));
                    colorSet = true;
                    break;
                case 2:
                    SetColor(new Color(1f, 0f, 0f, 1f));
                    colorSet = true;
                    break;
                case 3:
                    SetColor(new Color(1f, 1f, 0f, 1f));
                    colorSet = true;
                    break;
            }

        }
    }

    public void SetColor(Color color)
    {
        pingColor = color;
    }

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
