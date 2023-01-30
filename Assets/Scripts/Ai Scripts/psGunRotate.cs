using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psGunRotate : MonoBehaviour
{
    //[SerializeField] private GameObject gunRotator;
    [SerializeField] private GameObject thePlayer;
    public bool rotateToPlayer;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateToPlayer)
        {
            transform.LookAt(thePlayer.transform.position);
        }
    }
}
