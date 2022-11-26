using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisChargeManager : MonoBehaviour
{
    public GameObject chargePrefab;
    public InvisibilityMechanic invisCharges;
    List<InvisCharge> charges = new List<InvisCharge>();

    private void OnEnable()
    {
        InvisibilityMechanic.OnChargeUsed += DrawCharges;
    }

    private void OnDisable()
    {
        InvisibilityMechanic.OnChargeUsed -= DrawCharges;
    }
    
    private void Start()
    {
        DrawCharges();
    }
    
    public void DrawCharges()
    {
        ClearCharges();
        int chargesToMake = (int)(invisCharges.invisibilityCharges);
        for(int i = 0; i < chargesToMake; i++)
        {
            CreateEmptyCharge();
        }

        for(int i = 0; i < charges.Count; i++)
        {
            int chargeStatusRemainder = (int)Mathf.Clamp(invisCharges.invisibilityCharges - (i*1), 0, 1);
            charges[i].SetChargeImage((ChargeStatus)chargeStatusRemainder);
        }
    }
    
    public void CreateEmptyCharge()
    {
        GameObject newCharge = Instantiate(chargePrefab);
        newCharge.transform.SetParent(transform);

        InvisCharge chargeComponent = newCharge.GetComponent<InvisCharge>();
        chargeComponent.SetChargeImage(ChargeStatus.Empty);
        charges.Add(chargeComponent);
    }
    
    public void ClearCharges()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        charges = new List<InvisCharge>();
    }
}
