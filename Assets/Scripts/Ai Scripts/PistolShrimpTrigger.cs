using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpTrigger : MonoBehaviour
{
    [SerializeField] private int perchedPositionIndex;
    [SerializeField] private psEnemyAI ps;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ChangePosition(perchedPositionIndex);
            ps.SetSelectedTarget(other.gameObject.transform);
            ps.SwitchTarget(0,1);
            Destroy(this.gameObject);
        }
    }

    private void ChangePosition(int position)
    {
        ps.closestPoint = ps.perchedPositions[position];
        ps.state = psEnemyAI.State.isPerching;
    }
}
