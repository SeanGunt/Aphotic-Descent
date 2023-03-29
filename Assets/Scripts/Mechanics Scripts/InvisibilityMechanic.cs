using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InvisibilityMechanic : MonoBehaviour, IDataPersistence
{
    private PlayerInput playerInput;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Material[] originalMaterials;
    [SerializeField] private Material invisMaterial;
    Shader invisShader;
    private float fresnelPower = 3.0f;
    public float timeInvisible = 5.0f;
    public float invisibleTimer;
    private float invisibleCooldown, maxInvisCooldown;
    [SerializeField]private bool coolingDown;
    public int invisibilityCharges = 3;
    public int maxCharges = 3;
    public Text interactionText;
    [SerializeField] private bool invisibilityAcquired, canGoInvis;
    [HideInInspector] public bool isSafe, isInvisible;
    [SerializeField] private Image fullInvisCharge;
    [SerializeField] private GameObject Player, PlayerMesh, invisibilityUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip invisSound;
    public static event Action OnChargeUsed;
    [SerializeField] private ParticleSystem invisParticle;

    private void Start()
    {
      if (GameDataHolder.invisibilityAcquired == true)
      {
        invisibilityUI.SetActive(true);
        invisibilityAcquired = true;
      }
      else
      {
        invisibilityUI.SetActive(false);
        invisibilityAcquired = false;
      }

      if (invisibilityAcquired)
      {
        canGoInvis = true;
      }
    }
    void Awake()
    {
      isInvisible = false;
      isSafe = false;
      interactionText.text = "";
      Player = GameObject.FindWithTag("Player");
      playerInput = Player.GetComponent<PlayerInput>();
      invisibleCooldown = 0f;
      maxInvisCooldown = 7f;

      skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
      originalMaterials = new Material[skinnedMeshRenderers.Length];
      for (int i = 0; i < skinnedMeshRenderers.Length; i++)
      {
        originalMaterials[i] = skinnedMeshRenderers[i].material;
      }
    }

    public void LoadData(GameData data)
    {
      invisibilityCharges = data.invisCharges;
    }

    public void SaveData(GameData data)
    {
      data.invisCharges = invisibilityCharges;
    }
    void Update()
    {
        invisibleTimer = Mathf.Clamp(invisibleTimer, 0f, timeInvisible);
        if (playerInput.actions["Invisibility"].triggered && GameDataHolder.invisibilityAcquired)
        {
          GoInvis();
        }
        if (isInvisible)
        {
          for (int i = 0; i < skinnedMeshRenderers.Length; i++)
          {
            skinnedMeshRenderers[i].material = invisMaterial;
          }
          fresnelPower -= Time.deltaTime;
          invisMaterial.SetFloat("_FresnelPower", fresnelPower);
          invisibleTimer -= Time.deltaTime;
          fullInvisCharge.fillAmount = invisibleTimer/timeInvisible;

          if (invisibleTimer <= 0)
          {
            for (int i = 0; i <skinnedMeshRenderers.Length; i++)
            {
              skinnedMeshRenderers[i].material = originalMaterials[i];
            }
            fresnelPower = 3f;
            invisMaterial.SetFloat("_FresnelPower", fresnelPower);
            invisParticle.gameObject.SetActive(false);
            invisParticle.Stop();
            isInvisible = false;
            isSafe = false;
            interactionText.text = $"You are no longer invisible! Charges: {invisibilityCharges}";
            coolingDown = true;
            //Invoke ("RefillInvis", 7);
          }
        }
        

        invisibleCooldown = Mathf.Clamp(invisibleCooldown, 0, maxInvisCooldown);
        if(coolingDown)
        {
          invisibleCooldown += Time.deltaTime;
          fullInvisCharge.fillAmount = invisibleCooldown/maxInvisCooldown;
          if (invisibleCooldown >= maxInvisCooldown)
          {
            invisibleCooldown = 0;
            RefillInvis();
            coolingDown = false;
          }
        }

        if (Player.GetComponent<PlayerHealthController>().isBleeding)
        {
          isSafe = false;
        }
    }

    void OnTriggerStay(Collider col)
    {
      if (col.gameObject.tag == ("InvisPickup"))
      {
        CancelInvoke("ClearUI");
        //invisibilityCharges++;
        OnChargeUsed?.Invoke();
        //interactionText.text = $"You picked up a charge! Invis charges: {invisibilityCharges}";
        Debug.Log("InvisPicked Up");
        //Invoke ("ClearUI", 3);
      }
    }

    private void GoInvis()
    {
      if (canGoInvis && !isSafe)
          {
            CancelInvoke("ClearUI");
            isInvisible = true;
            canGoInvis = false;
            audioSource.PlayOneShot(invisSound);
            fullInvisCharge.enabled = true;
            fullInvisCharge.fillAmount = 1;
            invisibleTimer = timeInvisible;
            invisParticle.gameObject.SetActive(true);
            invisParticle.Play();
            isSafe = true;
            //invisibilityCharges--;
            OnChargeUsed?.Invoke();
            interactionText.text = "You are invisible!";
          }
          else if (invisibilityCharges <= 0)
          {
            interactionText.text = "You have no charges!";
            //Invoke ("ClearUI", 3);
          }
    }

    public void SetInvisUIActive()
    {
      invisibilityUI.SetActive(true);
      canGoInvis = true;
    }

    void RefillInvis()
    {
      canGoInvis = true;
      fullInvisCharge.enabled = true;
      fullInvisCharge.fillAmount = 1;
    }
}
