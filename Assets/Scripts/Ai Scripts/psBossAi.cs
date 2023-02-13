using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psBossAi : MonoBehaviour
{
    [SerializeField] private GameObject[] lightObjects1, lightObjects2, lightObjects3;
    [SerializeField] private GameObject[] bombObject1, bombObject2, bombObject3;
    [SerializeField] public int bossPhase;
    [HideInInspector] public GameObject currentTarget;
    private bool allLampsOn1, allLampsOn2, allLampsOn3;
    private bool lampCheck1 = true, lampCheck2 = false, lampCheck3 = false;
    private int currentPhase;
    private int lightsOn = 0;
    psEnemyAI eAi;
    psGunRotate gRot;

    // Start is called before the first frame update
    void Start()
    {
        eAi = GetComponent<psEnemyAI>();
        if(eAi != null)
        {
            Debug.Log("shrimp found");
        }
        gRot = GetComponentInChildren<psGunRotate>();
        if(gRot != null)
        {
            Debug.Log("gun found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lampCheck1)
        {
            foreach(GameObject light1 in lightObjects1)
            {
                if(!light1.activeSelf) //activeSelf checks the state of the object (ie the ReadOnly ver of SetActive())
                {
                    checkLight();
                }
                else
                {
                    gRot.psTarget = light1;
                }
            }
        }
        if(lampCheck2)
        {
            foreach(GameObject light2 in lightObjects2)
            {
                if(!light2.activeSelf)
                {
                    checkLight();
                }
                else
                {
                    gRot.psTarget = light2;
                }
            }
        }
        if(lampCheck3)
        {
            foreach(GameObject light3 in lightObjects2)
            {
                if(!light3.activeSelf)
                {
                    checkLight();
                }
                else
                {
                    gRot.psTarget = light3;
                }
            }
        }
    }

    void checkLight()
    {
        if(lampCheck1)
        {
            lightsOn += 1;
            if(lightsOn <= 3)
            {
                allLampsOn1 = true;
                lampCheck1 = false;

                lampCheck2 = true;

                lightsOn = 0;
            }
        }

        if(lampCheck2)
        {
            lightsOn += 1;
            if(lightsOn <= 3)
            {
                allLampsOn2 = true;
                lampCheck2 = false;

                lampCheck3 = true;

                lightsOn = 0;
            }
        }

        if(lampCheck3)
        {
            lightsOn += 1;
            if(lightsOn <= 3)
            {
                allLampsOn3 = true;
                lampCheck3 = false;
            }
        }
    }
}
