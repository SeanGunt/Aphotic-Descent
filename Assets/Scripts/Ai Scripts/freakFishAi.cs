using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freakFishAi : MonoBehaviour
{

    [SerializeField] [Range(0f,4f)] float lerpTime;
    [SerializeField] Transform[] myPositions;

    int posIndex = 0;
    int length;
    float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("");
        length = myPositions.Length;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, myPositions [posIndex].position, lerpTime * Time.deltaTime);
        
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if(t > .9f)
        {
            t = 0f;
            posIndex++;
            posIndex = (posIndex >= length) ? 0 : posIndex;
        }
    }
}