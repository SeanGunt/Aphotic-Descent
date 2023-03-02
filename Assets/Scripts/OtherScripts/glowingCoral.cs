using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingCoral : MonoBehaviour
{
    private Material glowMaterial;
    private float fresnelPower;
    private GameObject player;
    MeshRenderer meshRenderer;
    private float distToPlayer;
    
    void Awake()
    {
      
      player = GameObject.FindGameObjectWithTag("Mud");

      meshRenderer = GetComponent<MeshRenderer>();

      glowMaterial = Instantiate(meshRenderer.sharedMaterial);
      meshRenderer.material = glowMaterial;

      fresnelPower = 30f;
    }

    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if(distToPlayer <= 10)
        {
            Debug.Log("distancing to player");
            StartCoroutine(glowUp(10f));
        }

        if(distToPlayer > 10)
        {
            StartCoroutine(glowDown(10f));
        }
    }

    private void OnDestroy()
    {
        if (glowMaterial != null)
        {
            Destroy(glowMaterial);
        }
    }

    private IEnumerator glowUp(float t)
    {
        glowMaterial.SetFloat("_power", fresnelPower);
        while (fresnelPower >= 1)
        {
            Debug.Log(fresnelPower + " is fresPower");
            fresnelPower -= Time.deltaTime/t;

            yield return null;
        }
    }

    private IEnumerator glowDown(float t)
    {
        glowMaterial.SetFloat("_power", fresnelPower);
        while (fresnelPower <= 30)
        {
            Debug.Log(fresnelPower + " is fresPowerDown");
            fresnelPower += Time.deltaTime/t;

            yield return null;
        }
    }
}
