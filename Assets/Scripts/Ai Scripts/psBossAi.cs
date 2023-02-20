using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psBossAi : MonoBehaviour
{
    [SerializeField] private GameObject[] lightObjects1, lightObjects2, lightObjects3;
    [SerializeField] private bool allLampsOn1, allLampsOn2, allLampsOn3;
    [SerializeField] private bool lampCheck1 = true, lampCheck2 = false, lampCheck3 = false;
    [SerializeField] private GameObject bombObject1, bombObject2, bombObject3;
    [SerializeField] private bool bombs1 = false, bombs2 = false, bombs3 = false;
    [SerializeField] public int bossPhase;
    [HideInInspector] public GameObject currentTarget;
    [SerializeField] public bool nowChecking;
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
        if(nowChecking)
        {
            checkingLamps();
        }
    }

    void checkLight()
    {
        if(lampCheck1)
        {
            lightsOn += 1;
            if(lightsOn == 3)
            {
                allLampsOn1 = true;
                bombObject1.GetComponent<psShootObjects>().bombActive = true;
                lampCheck2 = true;
                lightsOn = 0;
                lampCheck1 = false;
            }
        }

        if(lampCheck2)
        {
            lightsOn += 1;
            if(lightsOn == 3)
            {
                allLampsOn2 = true;
                bombObject2.GetComponent<psShootObjects>().bombActive = true;
                lampCheck3 = true;
                lightsOn = 0;
                lampCheck2 = false;
            }
        }

        if(lampCheck3)
        {
            lightsOn += 1;
            if(lightsOn == 3)
            {
                bombObject3.GetComponent<psShootObjects>().bombActive = true;
                allLampsOn3 = true;
                lampCheck3 = false;
            }
        }
    }

    void checkingLamps()
    {
        if(lampCheck1)
        {
            foreach(GameObject light1 in lightObjects1)
            {
                if(!light1.activeSelf) //activeSelf checks the state of the object (basically the ReadOnly ver of SetActive())
                {
                    checkLight();
                }
                else
                {
                    eAi.distBtwn = Vector3.Distance(light1.transform.position, this.transform.position);
                    gRot.psTarget = light1; //if any of the lamps are active, aim at them
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
                    eAi.distBtwn = Vector3.Distance(light2.transform.position, this.transform.position);
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
                    eAi.distBtwn = Vector3.Distance(light3.transform.position, this.transform.position);
                    gRot.psTarget = light3;
                }
            }
        }
    }

    void shootBomb()
    {
        if(bombs1)
        {
            eAi.distBtwn = Vector3.Distance(bombObject1.transform.position, this.transform.position);

            gRot.psTarget = bombObject1;
            
            bombs1 = false;
        }

        if(bombs2)
        {
            eAi.distBtwn = Vector3.Distance(bombObject2.transform.position, this.transform.position);

            gRot.psTarget = bombObject2;

            bombs2 = false;
        }

        if(bombs3)
        {
            eAi.distBtwn = Vector3.Distance(bombObject3.transform.position, this.transform.position);

            gRot.psTarget = bombObject3;

            bombs3 = false;
        }
    }

    void targetAim()
    {
        if(lightsOn == 0)
        {
            gRot.psTarget = gRot.thePlayer;
        }
        else if(lightsOn > 0 && lightsOn < 3)
        {
            //gRot.psTarget = ; target closest active lamp
        }
        else
        {
            //gRot.psTarget = ; target closest active bomb
        }
    }
}
