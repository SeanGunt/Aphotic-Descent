using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psShootObjects : MonoBehaviour
{
    [SerializeField] private int bombHealth;
    [SerializeField] private int lampHealth;
    [SerializeField] private GameObject[] lampGroup;
    private int healthUsed;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "psBomb")
        {
            healthUsed = bombHealth;
        }
        else if(this.gameObject.tag == "psLamp")
        {
            healthUsed = lampHealth;
        }
    }

    public void subtractHealth()
    {
        healthUsed -= 1;
        {
            if(healthUsed <= 0)
            {
                if(this.gameObject.tag == "psBomb")
                {
                    foreach (GameObject lamp in lampGroup)
                    {
                        lamp.SetActive(false);
                    }
                }
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if tag is lamp then if other is blacklight
        if(this.gameObject.tag == "psLamp")
        {
            
        }
    }
}