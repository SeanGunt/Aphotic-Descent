using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBaitPuzzle : MonoBehaviour
{

    [SerializeField] private GameObject ropeToCheck;
    [SerializeField] private BoxCollider[] boxColliders;

    //private bool hasDestroyed = false;

    public bool puzzleCheck = false;

    [SerializeField] private ShrimpPath? shrimpPathModifier;



    void Start()
    {
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();
    }

    private void Update()
    {
        if (!ropeToCheck.activeInHierarchy)
        {
            puzzleCheck = true;

            foreach (BoxCollider col in boxColliders)
            {
                col.enabled = false;

            }
            if(shrimpPathModifier != null)
            {
				shrimpPathModifier.CanBeBlacklit();
			}
            Destroy(this.gameObject, 2f);

            //hasDestroyed = true;
        }

        
    }

}

