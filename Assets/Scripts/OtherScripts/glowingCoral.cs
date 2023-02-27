using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingCoral : MonoBehaviour
{
    private bool isGlowing;
    private GameObject theCoral;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Material[] originalMaterials;
    [SerializeField] private Material glowMaterial;
    Shader glowShader;
    private float fresnelPower = 3.0f;
    [SerializeField] private ParticleSystem glowParticle;

    // Start is called before the first frame update
    void Awake()
    {
      isGlowing = false;
      theCoral = GameObject.FindWithTag("");

      skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
      originalMaterials = new Material[skinnedMeshRenderers.Length];
      
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            originalMaterials[i] = skinnedMeshRenderers[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            goGlowy();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            goUnGlowy();
        }
    }

    void RefillInvis()
    {
        //canGoInvis = true;
        //fullInvisCharge.enabled = true;
        //fullInvisCharge.fillAmount = 1;
    }

    void goGlowy()
    {
        //invisibleTimer = Mathf.Clamp(invisibleTimer, 0f, timeInvisible);

        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            skinnedMeshRenderers[i].material = glowMaterial;
        }
        //fresnelPower -= Time.deltaTime;
        fresnelPower = 0f;
        glowMaterial.SetFloat("_FresnelPower", fresnelPower);
    }

    void goUnGlowy()
    {
        for (int i = 0; i <skinnedMeshRenderers.Length; i++)
        {
            skinnedMeshRenderers[i].material = originalMaterials[i];
        }
        fresnelPower = 3f;
        glowMaterial.SetFloat("_FresnelPower", fresnelPower);
        glowParticle.gameObject.SetActive(false);
        glowParticle.Stop();
        //isInvisible = false;
        //fullInvisCharge.enabled = false;
        Invoke ("RefillInvis", 7);
    }
}
