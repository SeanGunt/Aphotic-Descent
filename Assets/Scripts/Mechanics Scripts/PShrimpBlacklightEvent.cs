using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShrimpBlacklightEvent : MonoBehaviour
{
    [SerializeField] private psEnemyAI pistolShrimpAI;
    [SerializeField] private int index;
    [SerializeField] private float weight;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void MarkForDeletion()
    {
        pistolShrimpAI.SetSelectedTarget(this.transform);
        pistolShrimpAI.SwitchTarget(index, weight);
        pistolShrimpAI.FindClosestPosition();
    }

    public void Delete()
    {
        pistolShrimpAI.SetSelectedTarget(player.transform);
        pistolShrimpAI.SwitchTarget(0,1);
        Destroy(this.gameObject);
    }
}
