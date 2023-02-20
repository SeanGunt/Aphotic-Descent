using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psShootObjects : MonoBehaviour
{
    [SerializeField] private int bombHealth = 1;
    [SerializeField] private int lampHealth = 3;
    [SerializeField] public GameObject[] lampGroup;
    [SerializeField] public bool lampIsOn;
    [SerializeField] public bool bombActive = false;
    private int healthUsed;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "psBomb")
        {
            healthUsed = bombHealth;
            bombActive = false;
        }
        else if(this.gameObject.tag == "psLamp")
        {
            healthUsed = lampHealth;
            lampIsOn = false;
        }
        Debug.Log(healthUsed);
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
                    
                    this.gameObject.SetActive(false);
                }
                else if(this.gameObject.tag == "psLamp")
                {
                    lampIsOn = false;
                }
            }
        }
    }

    public void turnOnLamp()
    {
        lampIsOn = true;
    }
}