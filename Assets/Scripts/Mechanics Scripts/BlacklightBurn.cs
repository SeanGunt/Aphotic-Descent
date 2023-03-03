using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlacklightBurn : MonoBehaviour
{
    private Material material;
    private MeshCollider meshCollider;
    [SerializeField] private float deleteTime, rippleSpeed;
    private bool activated;

    private void Awake()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        material = Instantiate(renderer.sharedMaterial);
        renderer.material = material;
    }

    private void Update()
    {
        if (activated)
        {
            deleteTime -= Time.deltaTime;
        }
        if (deleteTime <= 0)
        {
            Destroy(this.gameObject);
            deleteTime = 5f;
        }
    }

    private void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }

    public void Burn()
    {
        material.SetVector("_RippleCenter", flashlightMechanic.hit.point);
        material.SetFloat("_RippleStartTime", Time.time);
        material.SetFloat("_RippleSpeed", rippleSpeed);
        meshCollider.enabled = false;
        activated = true;
    }
}
