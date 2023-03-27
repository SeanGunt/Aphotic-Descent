using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShrimpBlacklightEvent : MonoBehaviour
{
    [SerializeField] private psEnemyAI pistolShrimpAI;
    [SerializeField] private int index;
    [SerializeField] private float weight;
    [SerializeField] private Material lilGuyMaterial;
    [SerializeField] private Light glowLight;
    [SerializeField] private MeshRenderer lilGuyRenderer;
    [HideInInspector] public bool markedForDeletion;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void MarkForDeletion()
    {
        if(pistolShrimpAI.inPhase2)
        {
            markedForDeletion = true;
            pistolShrimpAI.SetSelectedTarget(this.transform);
            pistolShrimpAI.SwitchTarget(index, weight);
            pistolShrimpAI.FindClosestPosition();
        }
        else
        {
            return;
        }
    }

    public void Delete()
    {
        GameDataHolder.biolampsAlive -= 1;
        lilGuyRenderer.material = lilGuyMaterial;
        glowLight.color = new Color(0.6641f, 1f, 0.9769f, 1f);
        pistolShrimpAI.SetSelectedTarget(player.transform);
        pistolShrimpAI.SwitchTarget(0,1);
        Destroy(this.gameObject);
    }
}
