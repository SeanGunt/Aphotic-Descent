using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterLabDisappear : MonoBehaviour
{

    [SerializeField] private GameObject CrabLabImposter;
    [SerializeField] private GameObject walkPoint;
    private bool isWalking = false;
    private float speed = 2f;
    private float distance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWalking = true;
        }
    }

    private void Update()
    {
        if (isWalking == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, walkPoint.transform.position, Time.deltaTime * speed);
            distance = Vector3.Distance(this.transform.position, walkPoint.transform.position);
            if (distance <= 0.1)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
