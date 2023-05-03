using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EatTheShrimp : MonoBehaviour
{
    public GameObject[] pathPoints;
    [HideInInspector] public bool isMoving;
    public float speed;
    [SerializeField] private GameObject shrimpToBeEaten;
    NavMeshAgent hermitAgent;

    RigBuilder hermitRig;
    [SerializeField] private Animator HermitCrab;

    void Awake()
    {

        hermitAgent = this.gameObject.GetComponent<NavMeshAgent>();
        hermitAgent.speed = speed;

        hermitRig = this.gameObject.GetComponent<RigBuilder>();
        hermitRig.enabled = !hermitRig.enabled;
        
    }

    private void Update()
    {
        if(isMoving)
        {
            HermitCrab.SetBool("isMoving", true);
            hermitMove();
        }
    }
     void hermitMove()
     {
        hermitAgent.destination = pathPoints[0].transform.position;
     }
      
    public void MakeMove()
    {
        hermitRig.enabled = !hermitRig.enabled;
        isMoving = true;
    }

    public void ShrimpEaten()
    {
        shrimpToBeEaten.SetActive(false);
    }
}