using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeedX;
    public float scrollSpeedY;
    private MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * scrollSpeedX, Time.realtimeSinceStartup * scrollSpeedY);
    }
}
