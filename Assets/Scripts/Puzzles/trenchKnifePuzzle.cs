using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trenchKnifePuzzle : MonoBehaviour


{

    [SerializeField] public GameObject skull;
    [SerializeField] public Rigidbody skullRB;

    [SerializeField] public GameObject cage;

    [SerializeField] NewBaitPuzzle baitPuzzle;


    void Start()
    {
        skullRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (baitPuzzle.puzzleCheck)
        {
            Debug.Log("something hit!");
            StartCoroutine(FallDown());
        }
    }

    /*void OnColliderEnter(Collision other)
    {
        if (other.gameObject.tag == "Knife")
        {
            //StartCoroutine(FallDown());
            //Debug.Log("something hit!");
            //Destroy(this.gameObject);
        }
    }*/
    
    private IEnumerator FallDown()
    {
        yield return new WaitForSeconds(.2f);
        skullRB.isKinematic = false;

        yield return new WaitForSeconds(1f);
        Destroy(skull);
        Destroy(this.gameObject);

    }
}
