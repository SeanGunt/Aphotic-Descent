using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psGunRotate : MonoBehaviour
{
    [SerializeField] private GameObject thePlayer;
    [SerializeField] public GameObject psTarget;
    [SerializeField] private GameObject psGunHead;
    [SerializeField] private float gunRange;
    [SerializeField] private float gunDamage;
    [SerializeField] private LayerMask doNotIgnoreLayer;
    public bool rotateToPlayer;
    public bool rotateToTarget;
    private Vector3 startPos;
    RaycastHit hit;
    PlayerHealthController pHC;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;

        thePlayer = GameObject.FindWithTag("Player");
        pHC = GameObject.FindWithTag("Player").GetComponent<PlayerHealthController>();
        if(pHC != null && thePlayer != null)
        {
            Debug.Log("player found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateToPlayer)
        {
            transform.LookAt(thePlayer.transform.position);
        }
        else if(rotateToTarget)
        {
            transform.LookAt(psTarget.transform.position);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void shoot()
    {
        //uses raycast
        Vector3 centerRay = transform.TransformDirection(Vector3.forward) * gunRange;
        Debug.DrawRay(psGunHead.transform.position, centerRay, Color.red);

        if(Physics.Raycast(psGunHead.transform.position, centerRay, out hit, gunRange, doNotIgnoreLayer))
        {
            Debug.Log(hit.collider.gameObject.name + " was hit");

            if(hit.collider.gameObject == thePlayer)
            {
                pHC.ChangeHealth(-(pHC.maxHealth / gunDamage)); //essentially, 'what fraction of health (based on total maxHealth) will the gun remove from the player?'
                                                                //maxHealth is 15.0 atm, lower number for damage means more health is taken away
            }
        }
    }
}
