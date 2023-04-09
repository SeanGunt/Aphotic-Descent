using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class hermitCrabLegs : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint[] twoBonesIKConstraints;
    EatTheShrimp hermitMoveScr;

    // Start is called before the first frame update
    void Awake()
    {
        hermitMoveScr = this.gameObject.GetComponent<EatTheShrimp>();
        SetWeight(0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hermitMoveScr.isMoving == true)
        {
            SetWeight(0, 1);
        }
    }
    private void SetWeight(int index, float weight)
    {
        //WeightedTransformArray arrayOfTransforms = twoBonesIKConstraints.data.sourceObjects;
        //twoBonesIKConstraints.SetWeight(index, weight);
        SetWeight(index, weight);
        //twoBonesIKConstraints.data.sourceObjects = arrayOfTransforms;
    }

}
